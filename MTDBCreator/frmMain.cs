using System;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Data.OleDb;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using PNNL.Controls ; 

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for frmMain.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
    {
        #region Members
        delegate void DelegateUpdateMessage(string message);

        private const int mintAnalysisIdColumn = 0;
        private const int mintDatasetNameColumn = 1;
        private const int mintSlopeColumn = 2;
        private const int mintInterceptColumn = 3;
        private const int mintRSquaredColumn = 4;
        private const int mintNumMassTagsColumn = 5;
        public const string PROGRAM_DATE = "August 8, 2010";
        public const string CONST_DEFAULT_DATABASE_NAME = "untitled.mdb";

		private Splitter splitter2;
		private ListView listViewDatasets;
		private ColumnHeader columnHeaderId;
		private ColumnHeader columnHeaderDataset;
		private ColumnHeader columnHeaderSlope;
		private ColumnHeader columnHeaderIntercept;
		private ColumnHeader columnHeaderRSquared;
		private ColumnHeader columnHeaderNumUniqueMassTags;
		private RadioButton radioButtonPredictedNET;
		private RadioButton radioButtonAverageNET;
        private CheckBox checkBoxShowResiduals;
        private IContainer components = null;



		private clsXTandemAnalysisReader mobjXTandemPHRPResultsReader ; 
		private clsSequestAnalysisReader mobjSequestPHRPResultsReader ; 
		private clsPlotParams mobjPlotParams ;
		private ctlScatterChart ctlChartScanVsNET; 
		private clsSeries mobjSeries ; 
		private ChartPostRenderingProcessor mobjTransformPlotter ;
        private Splitter splitter1;

        private bool mblnCreatingDB = false;
        private GroupBox GroupBox1;
        private Splitter splitter3;

		private clsOptions mobjOptions = new clsOptions() ;
        private List<clsAnalysisDescription> m_analysisDescriptions;
        
        private Thread          m_processingThread;		        
        private clsMTDB         mobjMTDB;
        private float[]         marrPeptideScans;
        private float[]         marrPeptidePredictedNETs;
        private frmStatus m_statusForm;
        private SaveFileDialog m_saveMTDBDialog;
        private StatusStrip statusBarStrip;
        private ToolStripStatusLabel statusHover;
        private ToolStripStatusLabel statusMessages;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem fileToolStripMenuItem1;
        private ToolStripMenuItem newToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem createMTDBToolStripMenuItem;
        private ToolStripMenuItem createMTDBManualImportFilesToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem exitToolStripMenuItem1;
        private ToolStripMenuItem optionsToolStripMenuItem1;
        private ToolStripMenuItem alignSelectedDatasetToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem optionsToolStripMenuItem2;
        private ToolStripMenuItem aboutToolStripMenuItem1;
        private OpenFileDialog m_openAnalysisJobsDialog;
        #endregion

        #region Constructors and Initialization
        public frmMain()
		{
			InitializeComponent();

            m_analysisDescriptions      = new List<clsAnalysisDescription>();

            // Create a save MTDB Dialog window for selecting where to output the data
            m_saveMTDBDialog = new SaveFileDialog();
            m_saveMTDBDialog.FileName = CONST_DEFAULT_DATABASE_NAME;

            // Create a dialog for locating and opening analysis job text files.
            m_openAnalysisJobsDialog = new OpenFileDialog();

            // Create a new status form and hook into when an analysis or thread is killed.
            m_statusForm                = new frmStatus();
            m_statusForm.CancelPressed += new EventHandler(m_statusForm_CancelPressed);

            BubbleShape shape               = new BubbleShape(1, false);
            mobjPlotParams                  = new clsPlotParams(shape, Color.Blue, false, true, true);
            mobjTransformPlotter            = new ChartPostRenderingProcessor(this.DrawNetRegressionLine);
            ctlChartScanVsNET.AddPostProcessor(mobjTransformPlotter, PostProcessPriority.Mid);

            marrPeptidePredictedNETs = new float[1];
            marrPeptideScans = new float[1];
            mobjMTDB = new clsMTDB(mobjOptions);

            listViewDatasets.DoubleClick += new EventHandler(listViewDatasets_DoubleClick);

            createMTDBManualImportFilesToolStripMenuItem1.Enabled = false;
            createMTDBToolStripMenuItem.Enabled = false;             
		}   
        #endregion

        void m_statusForm_CancelPressed(object sender, EventArgs e)
        {
            AbortProcessingThreads();
            m_statusForm.Hide();
        }
        /// <summary>
        /// Kills the processing thread if it exists.
        /// </summary>
        private void AbortProcessingThreads()
        {
            if (m_processingThread == null)
                return;

            try
            {
                if (m_processingThread.IsAlive)
                {
                    m_processingThread.Abort();
                }
            }
            catch
            {
                // We dont care what the error message here is since we just want the thread to die.
            }            
            statusMessages.Text = "Previous MTDB Creation Aborted.";
        }
		protected void CreateMTDBFiles(bool writeToAccessDB, string databasePath)
		{
			
			radioButtonPredictedNET.Checked = true ; 
			radioButtonAverageNET.Enabled = false ;
            
            // Here we do a quick check to see what tools the analysis jobs were created with.  This was earlier done while processing.
            // Instead we just look to see if any there are any tools that use something other than SEQUEST and X!Tandem for database searching and then
            // if not, tell the user and let them decide before we get into some weird error handling pattern in a threaded context.
            bool continueWithProcessing = true;
            foreach (clsAnalysisDescription description in m_analysisDescriptions)
            {
                string tool = description.mstrAnalysisTool.ToUpper();
                if (tool != MTDBProcessor.CONST_ANALYSIS_TOOL_SEQUEST && tool != MTDBProcessor.CONST_ANALYSIS_TOOL_XTANDEM)
                {
                    string errorMessage = string.Format("One of the analysis jobs was procssed with {0} and not SEQUEST or X!Tandem.  These will not be included in the processing.  Do you want to continue processing anyway?", tool);
                    DialogResult result = MessageBox.Show(errorMessage, "Invalid Analysis Tool Found.", MessageBoxButtons.YesNo);

                    if (result != DialogResult.Yes)
                    {
                        continueWithProcessing = false;
                        break;
                    }
                }
            }

            if (!continueWithProcessing)
            {
                statusMessages.Text = "MTDB Creation aborted.";
                return;
            }

			
            MTDBProcessor processor          = new MTDBProcessor();
            processor.Status                += new DelegateSetStatusMessage(processor_Status);
            processor.ProcessingFailed      += new EventHandler(processor_ProcessingFailed);
            processor.ProcessingComplete    += new EventHandler<ProcessingCompleteEventArgs>(processor_ProcessingComplete);
            processor.PercentCompleted      += new DelegateSetPercentComplete(processor_PercentCompleted);
            processor.Error                 += new DelegateSetErrorMessage(processor_Error);
            processor.TotalProgressComplete += new DelegateSetPercentComplete(processor_TotalProgressComplete);

            m_statusForm.TopMost        = true;            
            m_statusForm.StartPosition  = FormStartPosition.CenterParent;
            m_statusForm.Show();
            m_statusForm.BringToFront();
                        
            // Kill the previous thread!             
            AbortProcessingThreads();

            m_statusForm.SetStatusMessage("Starting processing of analysis data files.");
            
            ProcessArguments arguments        = new ProcessArguments(mobjOptions,
                                                                    m_analysisDescriptions,
                                                                    writeToAccessDB,
                                                                    databasePath);

			ParameterizedThreadStart start    = new ParameterizedThreadStart(processor.ProcessAnalysis);             
			m_processingThread                = new Thread(start) ; 
			m_processingThread.Start(arguments);            
	    }

        #region Processing Event Handlers
        void UpdatePercentComplete(int percent)
        {
            m_statusForm.SetPrecentComplete(percent);
        }
        void UpdateTotalPercentComplete(int percent)
        {
            m_statusForm.SetTotalPrecentComplete(percent);
        }
        void UpdateStatus(string message)
        {
            m_statusForm.SetStatusMessage(message);
        }
        void UpdateError(string message)
        {
            m_statusForm.SetErrorMessage(message);
            statusMessages.Text = message;
        }
        /// <summary>
        /// Delegate for displaying processing results.
        /// </summary>
        /// <param name="failed">List of datasets that failed to load.</param>
        delegate void DelegateProcessingComplete(List<string> failed);
        /// <summary>
        /// Notify people that the analysis is complete.
        /// </summary>
        void UpdateCompleteAnalysis(List<string> failed)
        {
            statusMessages.Text = "Mass Tag Database Creation Complete.";
            radioButtonAverageNET.Enabled = true;
            m_statusForm.Reset();
            m_statusForm.Hide();

            // If there are any failed datasets then show which ones failed to load.            
            if (failed.Count > 0)
            {
                Form form = new Form();
                ListBox box = new ListBox();
                box.Dock = DockStyle.Fill;
                foreach (string ioErrorDataset in failed)
                {
                    box.Items.Add(ioErrorDataset);
                }
                form.Controls.Add(box);
                form.Show();
            }
        }

        /// <summary>
        /// Notify people that the analysis is complete.
        /// </summary>
        void UpdateFailedAnalysis()
        {
            statusMessages.Text = "Mass Tag Database Creation Failed To Complete.";
            radioButtonAverageNET.Enabled = true;
            m_statusForm.Reset();
            m_statusForm.Hide();
        }
        void processor_TotalProgressComplete(int percentDone)
        {
            // Set the mass tag database and be done with it!            
            if (InvokeRequired == true)
            {
                Invoke(new DelegateSetPercentComplete(UpdateTotalPercentComplete), new object[] { percentDone });
            }
            else
            {
                UpdateTotalPercentComplete(percentDone);
            }
        }
        /// <summary>
        /// Handles updates when a sub-process is complete.
        /// </summary>
        /// <param name="percentDone"></param>
        void processor_PercentCompleted(int percentDone)
        {
            // Set the mass tag database and be done with it!            
            if (InvokeRequired == true)
            {
                Invoke(new DelegateSetPercentComplete(UpdatePercentComplete), new object[] {percentDone});
            }
            else
            {
                UpdatePercentComplete(percentDone);
                //UpdateCompleteAnalysis();
            }
        }
        /// <summary>
        /// Handles when processing is complete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void processor_ProcessingComplete(object sender, ProcessingCompleteEventArgs e)
        {
            // Set the mass tag database and be done with it!
            mobjMTDB = e.Database;
            if (InvokeRequired == true)
            {
                Invoke(new DelegateProcessingComplete(UpdateCompleteAnalysis), new object[] {e.FailedDatasets});
            }
            else
            {
                UpdateCompleteAnalysis(e.FailedDatasets);
            }
        }
        /// <summary>
        /// Handles when a proccessing job fails.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void processor_ProcessingFailed(object sender, EventArgs e)
        {
            if (InvokeRequired == true)
            {
                //Invoke(new DelegateUpdateMessage(UpdateError), new object[] { "Processing Failed." });
                //Invoke(new MethodInvoker(UpdateFailedAnalysis));
            }
            else
            {
                //UpdateCompleteAnalysis();
            }
        }
        /// <summary>
        /// Handles and displays processing errors.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void processor_Error(string message)
        {
            if (InvokeRequired == true)
            {
                Invoke(new DelegateUpdateMessage(UpdateError), new object[] { message });
            }
            else
            {
                UpdateError(message);
            }
        }
        /// <summary>
        /// Handles and displays status messages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void processor_Status(string message)
        {
            if (InvokeRequired == true)
            {
                Invoke(new DelegateUpdateMessage(UpdateStatus), new object[] {message});
            }
            else
            {
                UpdateStatus(message);
            }
        }
        #endregion

        /// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                if (mobjMTDB != null)
                {
                    try
                    {

                        mobjMTDB.Dispose();
                    }
                    catch
                    {
                        // We dont care if it fails since we are done with it!
                    }
                }

				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            PNNL.Controls.PenProvider penProvider3 = new PNNL.Controls.PenProvider();
            PNNL.Controls.PenProvider penProvider4 = new PNNL.Controls.PenProvider();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.listViewDatasets = new System.Windows.Forms.ListView();
            this.columnHeaderId = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderDataset = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderSlope = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderIntercept = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderRSquared = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderNumUniqueMassTags = new System.Windows.Forms.ColumnHeader();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxShowResiduals = new System.Windows.Forms.CheckBox();
            this.radioButtonAverageNET = new System.Windows.Forms.RadioButton();
            this.radioButtonPredictedNET = new System.Windows.Forms.RadioButton();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.ctlChartScanVsNET = new PNNL.Controls.ctlScatterChart();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.statusBarStrip = new System.Windows.Forms.StatusStrip();
            this.statusHover = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusMessages = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.createMTDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createMTDBManualImportFilesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.alignSelectedDatasetToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctlChartScanVsNET)).BeginInit();
            this.statusBarStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewDatasets
            // 
            this.listViewDatasets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderId,
            this.columnHeaderDataset,
            this.columnHeaderSlope,
            this.columnHeaderIntercept,
            this.columnHeaderRSquared,
            this.columnHeaderNumUniqueMassTags});
            this.listViewDatasets.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listViewDatasets.FullRowSelect = true;
            this.listViewDatasets.GridLines = true;
            this.listViewDatasets.HideSelection = false;
            this.listViewDatasets.Location = new System.Drawing.Point(3, 541);
            this.listViewDatasets.MultiSelect = false;
            this.listViewDatasets.Name = "listViewDatasets";
            this.listViewDatasets.Size = new System.Drawing.Size(1130, 227);
            this.listViewDatasets.TabIndex = 2;
            this.listViewDatasets.UseCompatibleStateImageBehavior = false;
            this.listViewDatasets.View = System.Windows.Forms.View.Details;
            this.listViewDatasets.MouseHover += new System.EventHandler(this.listViewDatasets_MouseHover);
            // 
            // columnHeaderId
            // 
            this.columnHeaderId.Text = "Analysis Id";
            this.columnHeaderId.Width = 80;
            // 
            // columnHeaderDataset
            // 
            this.columnHeaderDataset.Text = "Dataset Name";
            this.columnHeaderDataset.Width = 400;
            // 
            // columnHeaderSlope
            // 
            this.columnHeaderSlope.Text = "Slope";
            this.columnHeaderSlope.Width = 83;
            // 
            // columnHeaderIntercept
            // 
            this.columnHeaderIntercept.Text = "Intercept";
            this.columnHeaderIntercept.Width = 114;
            // 
            // columnHeaderRSquared
            // 
            this.columnHeaderRSquared.Text = "R Squared";
            this.columnHeaderRSquared.Width = 251;
            // 
            // columnHeaderNumUniqueMassTags
            // 
            this.columnHeaderNumUniqueMassTags.Text = "# of Unique Mass Tags";
            this.columnHeaderNumUniqueMassTags.Width = 140;
            // 
            // GroupBox1
            // 
            this.GroupBox1.BackColor = System.Drawing.Color.White;
            this.GroupBox1.Controls.Add(this.checkBoxShowResiduals);
            this.GroupBox1.Controls.Add(this.radioButtonAverageNET);
            this.GroupBox1.Controls.Add(this.radioButtonPredictedNET);
            this.GroupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.GroupBox1.Location = new System.Drawing.Point(973, 27);
            this.GroupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.GroupBox1.Size = new System.Drawing.Size(154, 503);
            this.GroupBox1.TabIndex = 2;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "NET Values";
            // 
            // checkBoxShowResiduals
            // 
            this.checkBoxShowResiduals.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxShowResiduals.Location = new System.Drawing.Point(5, 82);
            this.checkBoxShowResiduals.Name = "checkBoxShowResiduals";
            this.checkBoxShowResiduals.Size = new System.Drawing.Size(144, 24);
            this.checkBoxShowResiduals.TabIndex = 3;
            this.checkBoxShowResiduals.Text = "Show Residuals";
            this.checkBoxShowResiduals.CheckedChanged += new System.EventHandler(this.checkBoxShowResiduals_CheckedChanged);
            // 
            // radioButtonAverageNET
            // 
            this.radioButtonAverageNET.Dock = System.Windows.Forms.DockStyle.Top;
            this.radioButtonAverageNET.Location = new System.Drawing.Point(5, 50);
            this.radioButtonAverageNET.Name = "radioButtonAverageNET";
            this.radioButtonAverageNET.Size = new System.Drawing.Size(144, 32);
            this.radioButtonAverageNET.TabIndex = 1;
            this.radioButtonAverageNET.Text = "Average Observed NET";
            // 
            // radioButtonPredictedNET
            // 
            this.radioButtonPredictedNET.Checked = true;
            this.radioButtonPredictedNET.Dock = System.Windows.Forms.DockStyle.Top;
            this.radioButtonPredictedNET.Location = new System.Drawing.Point(5, 18);
            this.radioButtonPredictedNET.Name = "radioButtonPredictedNET";
            this.radioButtonPredictedNET.Size = new System.Drawing.Size(144, 32);
            this.radioButtonPredictedNET.TabIndex = 0;
            this.radioButtonPredictedNET.TabStop = true;
            this.radioButtonPredictedNET.Text = "Predicted NET";
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(1127, 27);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(6, 503);
            this.splitter2.TabIndex = 3;
            this.splitter2.TabStop = false;
            // 
            // ctlChartScanVsNET
            // 
            this.ctlChartScanVsNET.AutoViewPortXBase = 0F;
            this.ctlChartScanVsNET.AutoViewPortYBase = 0F;
            this.ctlChartScanVsNET.AxisAndLabelFont = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.ctlChartScanVsNET.AxisAndLabelMaxFontSize = 15;
            this.ctlChartScanVsNET.AxisAndLabelMinFontSize = 8;
            this.ctlChartScanVsNET.BackColor = System.Drawing.Color.White;
            this.ctlChartScanVsNET.ChartBackgroundColor = System.Drawing.Color.White;
            this.ctlChartScanVsNET.ChartLayout.LegendFraction = 0.2F;
            this.ctlChartScanVsNET.ChartLayout.LegendLocation = PNNL.Controls.ChartLegendLocation.Right;
            this.ctlChartScanVsNET.ChartLayout.MaxLegendHeight = 150;
            this.ctlChartScanVsNET.ChartLayout.MaxLegendWidth = 250;
            this.ctlChartScanVsNET.ChartLayout.MaxTitleHeight = 50;
            this.ctlChartScanVsNET.ChartLayout.MinLegendHeight = 50;
            this.ctlChartScanVsNET.ChartLayout.MinLegendWidth = 75;
            this.ctlChartScanVsNET.ChartLayout.MinTitleHeight = 15;
            this.ctlChartScanVsNET.ChartLayout.TitleFraction = 0.1F;
            this.ctlChartScanVsNET.DefaultZoomHandler.Active = true;
            this.ctlChartScanVsNET.DefaultZoomHandler.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(119)))), ((int)(((byte)(136)))), ((int)(((byte)(153)))));
            this.ctlChartScanVsNET.DefaultZoomHandler.LineColor = System.Drawing.Color.Black;
            this.ctlChartScanVsNET.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlChartScanVsNET.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            penProvider3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            penProvider3.Width = 1F;
            this.ctlChartScanVsNET.GridLinePen = penProvider3;
            this.ctlChartScanVsNET.HilightColor = System.Drawing.Color.Magenta;
            this.ctlChartScanVsNET.Legend.BackColor = System.Drawing.Color.Transparent;
            penProvider4.Color = System.Drawing.Color.Black;
            penProvider4.Width = 1F;
            this.ctlChartScanVsNET.Legend.BorderPen = penProvider4;
            this.ctlChartScanVsNET.Legend.Bounds = new System.Drawing.Rectangle(776, 80, 185, 380);
            this.ctlChartScanVsNET.Legend.ColumnWidth = 125;
            this.ctlChartScanVsNET.Legend.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ctlChartScanVsNET.Legend.MaxFontSize = 12F;
            this.ctlChartScanVsNET.Legend.MinFontSize = 6F;
            this.ctlChartScanVsNET.Location = new System.Drawing.Point(3, 27);
            this.ctlChartScanVsNET.Margins.BottomMarginFraction = 0.1F;
            this.ctlChartScanVsNET.Margins.BottomMarginMax = 72;
            this.ctlChartScanVsNET.Margins.BottomMarginMin = 30;
            this.ctlChartScanVsNET.Margins.DefaultMarginFraction = 0.05F;
            this.ctlChartScanVsNET.Margins.DefaultMarginMax = 15;
            this.ctlChartScanVsNET.Margins.DefaultMarginMin = 5;
            this.ctlChartScanVsNET.Margins.LeftMarginFraction = 0.2F;
            this.ctlChartScanVsNET.Margins.LeftMarginMax = 150;
            this.ctlChartScanVsNET.Margins.LeftMarginMin = 72;
            this.ctlChartScanVsNET.Name = "ctlChartScanVsNET";
            this.ctlChartScanVsNET.Size = new System.Drawing.Size(970, 503);
            this.ctlChartScanVsNET.TabIndex = 6;
            this.ctlChartScanVsNET.Title = "Predicted NET vs Scan for peptides";
            this.ctlChartScanVsNET.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 29F);
            this.ctlChartScanVsNET.TitleMaxFontSize = 50F;
            this.ctlChartScanVsNET.TitleMinFontSize = 6F;
            this.ctlChartScanVsNET.VerticalExpansion = 1F;
            this.ctlChartScanVsNET.ViewPort = ((System.Drawing.RectangleF)(resources.GetObject("ctlChartScanVsNET.ViewPort")));
            this.ctlChartScanVsNET.XAxisLabel = "peptide scan";
            this.ctlChartScanVsNET.YAxisLabel = "Predicted NET (Krokhin et.al.)";
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(3, 530);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1130, 6);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // splitter3
            // 
            this.splitter3.BackColor = System.Drawing.Color.Gray;
            this.splitter3.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter3.Location = new System.Drawing.Point(3, 536);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(1130, 5);
            this.splitter3.TabIndex = 9;
            this.splitter3.TabStop = false;
            // 
            // statusBarStrip
            // 
            this.statusBarStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusBarStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusHover,
            this.statusMessages});
            this.statusBarStrip.Location = new System.Drawing.Point(3, 768);
            this.statusBarStrip.Name = "statusBarStrip";
            this.statusBarStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusBarStrip.Size = new System.Drawing.Size(1130, 22);
            this.statusBarStrip.TabIndex = 11;
            this.statusBarStrip.Text = "statusStrip1";
            // 
            // statusHover
            // 
            this.statusHover.Name = "statusHover";
            this.statusHover.Size = new System.Drawing.Size(644, 17);
            this.statusHover.Text = "Use File -> Load to import a list of result paths from Peptides Hits Results Proc" +
                "essor which will be used to create a MassTag database";
            // 
            // statusMessages
            // 
            this.statusMessages.Name = "statusMessages";
            this.statusMessages.Size = new System.Drawing.Size(956, 17);
            this.statusMessages.Spring = true;
            this.statusMessages.Text = "Ready.";
            this.statusMessages.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.optionsToolStripMenuItem1,
            this.aboutToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1130, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem1,
            this.toolStripSeparator6,
            this.createMTDBToolStripMenuItem,
            this.createMTDBManualImportFilesToolStripMenuItem1,
            this.toolStripSeparator5,
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // newToolStripMenuItem1
            // 
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            this.newToolStripMenuItem1.Size = new System.Drawing.Size(244, 22);
            this.newToolStripMenuItem1.Text = "Load";
            this.newToolStripMenuItem1.Click += new System.EventHandler(this.newToolStripMenuItem1_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(241, 6);
            // 
            // createMTDBToolStripMenuItem
            // 
            this.createMTDBToolStripMenuItem.Name = "createMTDBToolStripMenuItem";
            this.createMTDBToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.createMTDBToolStripMenuItem.Text = "Create MTDB";
            this.createMTDBToolStripMenuItem.Click += new System.EventHandler(this.createMTDBToolStripMenuItem_Click);
            // 
            // createMTDBManualImportFilesToolStripMenuItem1
            // 
            this.createMTDBManualImportFilesToolStripMenuItem1.Name = "createMTDBManualImportFilesToolStripMenuItem1";
            this.createMTDBManualImportFilesToolStripMenuItem1.Size = new System.Drawing.Size(244, 22);
            this.createMTDBManualImportFilesToolStripMenuItem1.Text = "Create MTDB Manual Import Files";
            this.createMTDBManualImportFilesToolStripMenuItem1.Click += new System.EventHandler(this.createMTDBManualImportFilesToolStripMenuItem1_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(241, 6);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(244, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // optionsToolStripMenuItem1
            // 
            this.optionsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alignSelectedDatasetToolStripMenuItem1,
            this.toolStripSeparator2,
            this.optionsToolStripMenuItem2});
            this.optionsToolStripMenuItem1.Name = "optionsToolStripMenuItem1";
            this.optionsToolStripMenuItem1.Size = new System.Drawing.Size(56, 20);
            this.optionsToolStripMenuItem1.Text = "Options";
            // 
            // alignSelectedDatasetToolStripMenuItem1
            // 
            this.alignSelectedDatasetToolStripMenuItem1.Name = "alignSelectedDatasetToolStripMenuItem1";
            this.alignSelectedDatasetToolStripMenuItem1.Size = new System.Drawing.Size(193, 22);
            this.alignSelectedDatasetToolStripMenuItem1.Text = "Align Selected Dataset";
            this.alignSelectedDatasetToolStripMenuItem1.Click += new System.EventHandler(this.alignSelectedDatasetToolStripMenuItem1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(190, 6);
            // 
            // optionsToolStripMenuItem2
            // 
            this.optionsToolStripMenuItem2.Name = "optionsToolStripMenuItem2";
            this.optionsToolStripMenuItem2.Size = new System.Drawing.Size(193, 22);
            this.optionsToolStripMenuItem2.Text = "Options";
            this.optionsToolStripMenuItem2.Click += new System.EventHandler(this.optionsToolStripMenuItem2_Click);
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(48, 20);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripSeparator4,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(103, 6);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(103, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator3});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(57, 6);
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1136, 793);
            this.Controls.Add(this.ctlChartScanVsNET);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.splitter3);
            this.Controls.Add(this.listViewDatasets);
            this.Controls.Add(this.statusBarStrip);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "MTDB Creator";
            this.GroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ctlChartScanVsNET)).EndInit();
            this.statusBarStrip.ResumeLayout(false);
            this.statusBarStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
        
		#region "Helpers"
		private void AddAnalysesTolist(clsAnalysisDescription [] analyses)
		{
			for (int i = 0 ; i < analyses.Length ; i++)
			{
				AddAnalysisToList(analyses[i]) ;
			}
		}
		private void AddAnalysisToList(clsAnalysisDescription analysis) 
		{
            m_analysisDescriptions.Add(analysis);

			ListViewItem listItem = listViewDatasets.Items.Add((analysis.mintDatasetId+1).ToString()) ; 
			listItem.Tag          = analysis; 
			listItem.SubItems.Add(analysis.mstrDataset) ; 
		}
		#endregion 

		#region Data Loading Methods
        /// <summary>
        /// Loads and displays a XTandem dataset.
        /// </summary>
        /// <param name="analysis"></param>
		private void LoadDisplayXTandemDataset(clsAnalysisDescription analysis)
		{
			DateTime now        = DateTime.Now ; 
			double slope        = 0 ;
			double intercept    = 0 ;
			double rsquared     = 0 ; 
			int numScans        = 0 ; 
			
			m_statusForm.SetStatusMessage("Reading PHRP files for " + analysis.mstrDataset) ; 
			m_statusForm.Reset() ;

			mobjXTandemPHRPResultsReader = new clsXTandemAnalysisReader(analysis.mstrArchivePath, 
				                                                        analysis.mstrDataset); 

			DateTime read = DateTime.Now; 

					if (!radioButtonAverageNET.Checked)
			{
				mobjMTDB.AlignXTandemDatasetToTheoreticalNETs(  mobjXTandemPHRPResultsReader,  
					                                            ref marrPeptideScans, 
                                                                ref marrPeptidePredictedNETs,
                                                                ref slope, 
                                                                ref intercept, 
					                                            ref numScans, 
                                                                ref rsquared,
                                                                mobjOptions.UseKrokhinNET) ; 
			}
			else
			{
				mobjMTDB.AlignXTandemDatasetToAverageNETs(  mobjXTandemPHRPResultsReader, 
                                                            ref marrPeptideScans, 
					                                        ref marrPeptidePredictedNETs, 
                                                            ref slope, 
                                                            ref intercept, 
                                                            ref rsquared) ; 
			}
			DisplayScansVsNet(analysis.mstrDataset) ; 
		}
        /// <summary>
        /// Loads and displays a SEQUEST dataset.
        /// </summary>
        /// <param name="analysis"></param>
		private void LoadDisplaySequestDataset(clsAnalysisDescription analysis)
		{
			double slope      = 0 ;
            double intercept = 0;
			double rsquared = 0 ; 
			int numScans    = 0 ; 
			
			m_statusForm.SetStatusMessage("Reading PHRP files for " + analysis.mstrDataset) ;
            m_statusForm.Reset();

            try
            {
                mobjSequestPHRPResultsReader = new clsSequestAnalysisReader(analysis.mstrArchivePath,
                                                                            analysis.mstrDataset);
            }
            catch (Exception ex)
            {
                UpdateError("Could not display SEQUEST data. " + ex.Message);
                m_statusForm.Hide();
                return;
            }
			
            if (!radioButtonAverageNET.Checked)
            {
                mobjMTDB.AlignSequestDatasetToTheoreticalNETs(  mobjSequestPHRPResultsReader,
                                                                ref marrPeptideScans,
                                                                ref marrPeptidePredictedNETs,
                                                                ref slope,
                                                                ref intercept,
                                                                ref numScans, 
                                                                ref rsquared,
                                                                mobjOptions.UseKrokhinNET);
            }
            else
            {
                mobjMTDB.AlignSequestDatasetToAverageNETs(  mobjSequestPHRPResultsReader,
                                                            ref marrPeptideScans, 
                                                            ref marrPeptidePredictedNETs, 
                                                            ref slope,
                                                            ref intercept, 
                                                            ref rsquared);
            }

			DisplayScansVsNet(analysis.mstrDataset) ; 
		}

		#endregion

		#region "Processing"
        /// <summary>
        /// Process all of the datasets to create a MTDB.
        /// </summary>
		
		private void ProcessCurrentDataset()
		{
			try
			{
				if (listViewDatasets.SelectedItems.Count == 0)
					return ; 
				int selectedIndex = listViewDatasets.SelectedIndices[0] ; 

                //TODO: Fix this.
				//ProcessItem(selectedIndex) ; 
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message) ; 
			}
		}


		#endregion 

		#region "Display functions"
		private void OpenSelectedDatasetThreaded()
		{			
            m_statusForm.Reset();
            m_statusForm.SetStatusMessage("Aligning Run");

			System.Threading.ThreadStart start  = new System.Threading.ThreadStart(ShowSelectedDatasetAndCloseMessage) ; 
			System.Threading.Thread thrd        = new System.Threading.Thread(start) ; 
			thrd.Start() ;  
		}
		private void ShowSelectedDatasetAndCloseMessage() 
		{
            if (InvokeRequired == true)
            {
                Invoke(new MethodInvoker(ShowSelectedDataset));                
            }
            else
            {
                ShowSelectedDataset();                
            }            
        }        
		private void ShowSelectedDataset()
		{
			if (listViewDatasets.SelectedItems.Count == 0)
				return ; 
			int selectedIndex = listViewDatasets.SelectedIndices[0] ; 

			ListViewItem item = listViewDatasets.Items[selectedIndex] ; 
			try
			{
				clsAnalysisDescription analysis = (clsAnalysisDescription) item.Tag ; 
				switch(analysis.mstrAnalysisTool.ToUpper())
				{
					case "XTANDEM":
						LoadDisplayXTandemDataset(analysis) ; 
						break ; 
					case "SEQUEST":
						LoadDisplaySequestDataset(analysis) ; 
						break ; 
					default:
						break ; 
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + ex.StackTrace) ; 
			}
		}
		private void DisplayScansVsNet(string dataset)
		{
				
			mobjPlotParams.Name = dataset ; 

			if (!checkBoxShowResiduals.Checked)
			{
				mobjSeries = new clsSeries(new PNNL.Controls.ArrayChartDataProvider(marrPeptideScans, marrPeptidePredictedNETs), 
					mobjPlotParams) ; 
			}
			else
			{
				// remove predicted component for each part. 
				float [] residuals = new float[marrPeptideScans.Length] ; 
				for (int i = 0 ; i < marrPeptideScans.Length ; i++)
				{
					residuals[i] = marrPeptidePredictedNETs[i] - mobjMTDB.GetTransformedNET(marrPeptideScans[i]) ; 
				}
				mobjSeries = new clsSeries(new PNNL.Controls.ArrayChartDataProvider(marrPeptideScans, residuals), 
					mobjPlotParams) ; 

			}

			ctlChartScanVsNET.SeriesCollection.Clear() ; 
			ctlChartScanVsNET.SeriesCollection.Add(mobjSeries) ; 
			ctlChartScanVsNET.AutoViewPort() ; 
		}		
		private void DrawNetRegressionLine(ctlChartBase sender, PostRenderEventArgs args) 
		{
			try
			{
				float xFrom = sender.ViewPort.X  ; 
				float xFinal = sender.ViewPort.Right  ; 
				Pen mpen_line = new Pen(Brushes.Black,3) ; 
				if (!checkBoxShowResiduals.Checked)
				{
					int numDivisions = 10 ; 


					float step = (xFinal - xFrom) / numDivisions ; 


					for ( ; xFrom < xFinal ; xFrom += step)
					{
						float yFrom = mobjMTDB.GetTransformedNET(xFrom) ;
						float xTo = xFrom + step ; 
						float yTo =  mobjMTDB.GetTransformedNET(xTo) ;
						args.Graphics.DrawLine(mpen_line, sender.GetScreenPixelX(xFrom), sender.GetScreenPixelY(yFrom),
							sender.GetScreenPixelX(xTo), sender.GetScreenPixelY(yTo)) ; 
					}
				}
				else
				{
					args.Graphics.DrawLine(mpen_line, sender.GetScreenPixelX(xFrom), sender.GetScreenPixelY(0),
						sender.GetScreenPixelX(xFinal), sender.GetScreenPixelY(0)) ; 
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + " " + ex.StackTrace) ; 
			}
		}
		#endregion

		#region Form Event Handlers
		private void checkBoxShowResiduals_CheckedChanged(object sender, System.EventArgs e)
		{
			if (listViewDatasets.SelectedItems.Count == 0)
				return ; 
			int selectedIndex = listViewDatasets.SelectedIndices[0] ; 
			clsAnalysisDescription analysis = (clsAnalysisDescription) listViewDatasets.Items[selectedIndex].Tag ; 
			DisplayScansVsNet(analysis.mstrDataset) ; 
		}
        private void listViewDatasets_MouseHover(object sender, System.EventArgs e)
        {
            if (listViewDatasets.Items.Count != 0)
            {
                statusHover.Text = "Double Click a dataset to view alignment";
            }
            else
            {
                statusHover.Text = "Use File -> Load to import a list of result paths from Peptides Hits Results Processor which will be used to create a MassTag database";
            }
        }
        private void listViewDatasets_DoubleClick(object sender, EventArgs e)
        {
            if (mblnCreatingDB)
                return;
            OpenSelectedDatasetThreaded();
        }	        
        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            try
            {
                radioButtonPredictedNET.Checked = true;
                radioButtonAverageNET.Enabled = false;

                m_openAnalysisJobsDialog.Filter = "txt files (*.txt)|*.txt";
                m_openAnalysisJobsDialog.FilterIndex = 1;
                m_openAnalysisJobsDialog.RestoreDirectory = true;

                if (m_openAnalysisJobsDialog.ShowDialog() == DialogResult.OK)
                {
                    this.listViewDatasets.Items.Clear();
                    clsAnalysisDescriptionReader reader = new clsAnalysisDescriptionReader(m_openAnalysisJobsDialog.FileName);
                    clsAnalysisDescription[] analyses = reader.Analyses;
                    AddAnalysesTolist(analyses);

                    createMTDBManualImportFilesToolStripMenuItem1.Enabled = true;
                    alignSelectedDatasetToolStripMenuItem1.Enabled = true;
                    createMTDBToolStripMenuItem.Enabled = true;
                    statusHover.Text = "Double Click Item to Perform Alignment or Select File -> Create MTDB Manual Import Files for creating a mass tag database";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void createMTDBToolStripMenuItem_Click(object sender, EventArgs e)
        {

            mobjMTDB = null;
            mobjMTDB = new clsMTDB(mobjOptions);


            m_saveMTDBDialog.Title = "Select output database";
            m_saveMTDBDialog.Filter = "Microsoft Access Files (*.mdb)|*.mdb";
            m_saveMTDBDialog.FilterIndex = 1;
            m_saveMTDBDialog.RestoreDirectory = true;

            m_statusForm.ClearErrorMessages();
            m_statusForm.Reset();

            if (m_saveMTDBDialog.ShowDialog() == DialogResult.OK)
            {

                string databasePath = m_saveMTDBDialog.FileName;
                string newName = databasePath;

                // Find a new path name.  
                int i = 1;

                string directoryPath = System.IO.Path.GetDirectoryName(databasePath);
                string filename = System.IO.Path.GetFileNameWithoutExtension(databasePath);
                string extension = System.IO.Path.GetExtension(databasePath);

                while (System.IO.File.Exists(newName))
                {
                    newName = System.IO.Path.Combine(directoryPath, filename + "_" + string.Format("{0:000}", i++) + extension);
                }

                CreateMTDBFiles(true, newName);
            }		
        }

        private void createMTDBManualImportFilesToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            // Clear out the old MTDB and create a new one.
            if (mobjMTDB != null)
                mobjMTDB.Dispose();
            mobjMTDB = new clsMTDB(mobjOptions);

            DialogResult result = MessageBox.Show("Creating MT DB Files, which you can then manually import.  Continue?", "Processing",
                                                    MessageBoxButtons.YesNoCancel,
                                                    MessageBoxIcon.Question,
                                                    MessageBoxDefaultButton.Button1);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                m_statusForm.Reset();
                CreateMTDBFiles(false, "");
            }
        }

        private void alignSelectedDatasetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listViewDatasets.SelectedItems.Count == 0)
            {
                MessageBox.Show("No dataset selected. Please select a dataset and try again. Or double click the dataset you would like to open");
                return;
            }
            OpenSelectedDatasetThreaded();

        }

        private void optionsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmOptions optionsForm = new frmOptions();
            optionsForm.Options = mobjOptions;
            if (optionsForm.ShowDialog(this) == DialogResult.OK)
            {
                mobjOptions = optionsForm.Options;
                mobjMTDB.Options = mobjOptions;
            }

        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            frmAbout aboutForm = new frmAbout();
            aboutForm.ShowDialog();
        }

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new frmMain());
        }
	}
}
