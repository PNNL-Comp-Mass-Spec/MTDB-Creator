using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb ; 
using PNNL.Controls ; 
namespace MTDBCreator
{
	/// <summary>
	/// Summary description for frmMain.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private PNNL.Controls.ExpandPanel expandPanelLists;
		private PNNL.Controls.ExpandPanel expandPanelOptions;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.ListView listViewDatasets;
		private System.Windows.Forms.ColumnHeader columnHeaderId;
		private System.Windows.Forms.ColumnHeader columnHeaderDataset;
		private System.Windows.Forms.ColumnHeader columnHeaderSlope;
		private System.Windows.Forms.ColumnHeader columnHeaderIntercept;
		private System.Windows.Forms.ColumnHeader columnHeaderRSquared;
		private System.Windows.Forms.ColumnHeader columnHeaderNumUniqueMassTags;
		private System.Windows.Forms.RadioButton radioButtonPredictedNET;
		private System.Windows.Forms.RadioButton radioButtonAverageNET;
		private System.Windows.Forms.CheckBox checkBoxShowResiduals;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private clsAnalysisDescription [] marrAnalyses ; 
		private const int mintAnalysisIdColumn = 0 ; 
		private const int mintDatasetNameColumn = 1 ; 
		private const int mintSlopeColumn = 2 ; 
		private const int mintInterceptColumn = 3 ; 
		private const int mintRSquaredColumn = 4 ; 
		private const int mintNumMassTagsColumn = 5 ; 
		
		private string mstrAccessDbPath ;
		private clsMTDB mobjMTDB ; 
		private float [] marrPeptideScans ; 
		private float [] marrPeptidePredictedNETs ; 
		private double mdbl_slope ; 
		private double mdbl_intercept ; 
		private bool m_WritingToAccessDB ;

		public const string PROGRAM_DATE = "October 23, 2009" ;

		private clsXTandemAnalysisReader mobjXTandemPHRPResultsReader ; 
		private clsSequestAnalysisReader mobjSequestPHRPResultsReader ; 
		private PNNL.Controls.clsPlotParams mobjPlotParams ;
		private PNNL.Controls.ctlScatterChart ctlChartScanVsNET; 
		private PNNL.Controls.clsSeries mobjSeries ; 
		private PNNL.Controls.ChartPostRenderingProcessor mobjTransformPlotter ;
		frmStatus mfrmStatus = new frmStatus() ; 
		private string 	mstrErrors = "" ;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.StatusBar statusBarMessages; 

		private bool mblnCreatingDB = false ;
		private System.Windows.Forms.MenuItem mnuFileOpenDatasetList;
		private System.Windows.Forms.GroupBox GroupBox1;
		private System.Windows.Forms.MainMenu mnuMainMenu;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuFileSep1;
		private System.Windows.Forms.MenuItem mnuFileExit;
		private System.Windows.Forms.MenuItem mnuTools;
		private System.Windows.Forms.MenuItem mnuToolsCreateDB;
		private System.Windows.Forms.MenuItem mnuToolsAlignSelected;
		private System.Windows.Forms.MenuItem mnuToolsOptions;
		private System.Windows.Forms.MenuItem mnuAbout;
		private System.Windows.Forms.MenuItem mnuToolsCreateMTDBFiles; 
		private clsOptions mobjOptions = new clsOptions() ;
		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			Init() ;
			mnuToolsCreateDB.Enabled = false ; 
			mnuToolsCreateMTDBFiles.Enabled = false ; 
		}


		private void Init() 
		{
			PNNL.Controls.BubbleShape shape = new PNNL.Controls.BubbleShape(1, false) ; 
			mobjPlotParams = new PNNL.Controls.clsPlotParams(shape, Color.Blue, false, true, true) ; 
			mobjTransformPlotter = new ChartPostRenderingProcessor(this.DrawNetRegressionLine) ; 
			ctlChartScanVsNET.AddPostProcessor(mobjTransformPlotter, PostProcessPriority.Mid) ;
			mdbl_slope = 0 ; 
			mdbl_intercept = 0 ; 
			marrPeptidePredictedNETs = new float[1] ; 
			marrPeptideScans = new float[1] ; 
			mobjMTDB = new clsMTDB(mfrmStatus, mobjOptions) ; 
			listViewDatasets.DoubleClick +=new EventHandler(listViewDatasets_DoubleClick);
		}

		protected void CreateMTDBFiles(bool WriteToAccessDB)
		{
			
			radioButtonPredictedNET.Checked = true ; 
			radioButtonAverageNET.Enabled = false ; 

			m_WritingToAccessDB = WriteToAccessDB;

			System.Threading.ThreadStart start = new System.Threading.ThreadStart(this.ProcessAllDatasets) ; 
			System.Threading.Thread thrd = new System.Threading.Thread(start) ; 
			thrd.Start() ; 

			if (WriteToAccessDB)
				mfrmStatus.ShowStatusBox(this, "Creating Mass Tag Database") ; 
			else
				mfrmStatus.ShowStatusBox(this, "Creating MT DB Files") ; 

			if (thrd.IsAlive)
			{
				thrd.Abort() ; 
			}

			if (mstrErrors != "")
			{
				MessageBox.Show(this, mstrErrors, "Errors in processing") ; 
			}
			radioButtonAverageNET.Enabled = true ; 
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMain));
			PNNL.Controls.PenProvider penProvider1 = new PNNL.Controls.PenProvider();
			PNNL.Controls.PenProvider penProvider2 = new PNNL.Controls.PenProvider();
			this.expandPanelLists = new PNNL.Controls.ExpandPanel(228);
			this.listViewDatasets = new System.Windows.Forms.ListView();
			this.columnHeaderId = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderDataset = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderSlope = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderIntercept = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderRSquared = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderNumUniqueMassTags = new System.Windows.Forms.ColumnHeader();
			this.expandPanelOptions = new PNNL.Controls.ExpandPanel(156);
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.checkBoxShowResiduals = new System.Windows.Forms.CheckBox();
			this.radioButtonAverageNET = new System.Windows.Forms.RadioButton();
			this.radioButtonPredictedNET = new System.Windows.Forms.RadioButton();
			this.mnuMainMenu = new System.Windows.Forms.MainMenu();
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuFileOpenDatasetList = new System.Windows.Forms.MenuItem();
			this.mnuFileSep1 = new System.Windows.Forms.MenuItem();
			this.mnuFileExit = new System.Windows.Forms.MenuItem();
			this.mnuTools = new System.Windows.Forms.MenuItem();
			this.mnuToolsCreateDB = new System.Windows.Forms.MenuItem();
			this.mnuToolsAlignSelected = new System.Windows.Forms.MenuItem();
			this.mnuToolsOptions = new System.Windows.Forms.MenuItem();
			this.mnuAbout = new System.Windows.Forms.MenuItem();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.ctlChartScanVsNET = new PNNL.Controls.ctlScatterChart();
			this.statusBarMessages = new System.Windows.Forms.StatusBar();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.mnuToolsCreateMTDBFiles = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.expandPanelLists)).BeginInit();
			this.expandPanelLists.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.expandPanelOptions)).BeginInit();
			this.expandPanelOptions.SuspendLayout();
			this.GroupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ctlChartScanVsNET)).BeginInit();
			this.SuspendLayout();
			// 
			// expandPanelLists
			// 
			this.expandPanelLists.Controls.Add(this.listViewDatasets);
			this.expandPanelLists.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.expandPanelLists.ExpandImage = ((System.Drawing.Image)(resources.GetObject("expandPanelLists.ExpandImage")));
			this.expandPanelLists.HeaderRightToLeft = System.Windows.Forms.RightToLeft.No;
			this.expandPanelLists.HeaderTextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.expandPanelLists.Location = new System.Drawing.Point(0, 523);
			this.expandPanelLists.Name = "expandPanelLists";
			this.expandPanelLists.Size = new System.Drawing.Size(1136, 248);
			this.expandPanelLists.TabIndex = 5;
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
			this.listViewDatasets.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewDatasets.FullRowSelect = true;
			this.listViewDatasets.GridLines = true;
			this.listViewDatasets.HideSelection = false;
			this.listViewDatasets.Location = new System.Drawing.Point(1, 20);
			this.listViewDatasets.MultiSelect = false;
			this.listViewDatasets.Name = "listViewDatasets";
			this.listViewDatasets.Size = new System.Drawing.Size(1134, 227);
			this.listViewDatasets.TabIndex = 2;
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
			// 
			// columnHeaderIntercept
			// 
			this.columnHeaderIntercept.Text = "Intercept";
			// 
			// columnHeaderRSquared
			// 
			this.columnHeaderRSquared.Text = "R Squared";
			this.columnHeaderRSquared.Width = 80;
			// 
			// columnHeaderNumUniqueMassTags
			// 
			this.columnHeaderNumUniqueMassTags.Text = "# of Unique Mass Tags";
			this.columnHeaderNumUniqueMassTags.Width = 140;
			// 
			// expandPanelOptions
			// 
			this.expandPanelOptions.Controls.Add(this.GroupBox1);
			this.expandPanelOptions.Dock = System.Windows.Forms.DockStyle.Right;
			this.expandPanelOptions.DockPadding.All = 10;
			this.expandPanelOptions.ExpandDirection = PNNL.Controls.ExpandDirection.Left;
			this.expandPanelOptions.ExpandImage = ((System.Drawing.Image)(resources.GetObject("expandPanelOptions.ExpandImage")));
			this.expandPanelOptions.HeaderContractText = "Hide Options";
			this.expandPanelOptions.HeaderExpandText = "Show Options";
			this.expandPanelOptions.HeaderLocation = PNNL.Controls.HeaderLocation.Left;
			this.expandPanelOptions.HeaderRightToLeft = System.Windows.Forms.RightToLeft.No;
			this.expandPanelOptions.HeaderText = "Hide Options";
			this.expandPanelOptions.HeaderTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.expandPanelOptions.HeaderTextDirection = PNNL.Controls.TextDirection.Vertical;
			this.expandPanelOptions.HeaderTextMode = PNNL.Controls.HeaderTextSwitchingMode.ExpandContractText;
			this.expandPanelOptions.Location = new System.Drawing.Point(960, 0);
			this.expandPanelOptions.Name = "expandPanelOptions";
			this.expandPanelOptions.Size = new System.Drawing.Size(176, 517);
			this.expandPanelOptions.TabIndex = 4;
			// 
			// GroupBox1
			// 
			this.GroupBox1.Controls.Add(this.checkBoxShowResiduals);
			this.GroupBox1.Controls.Add(this.radioButtonAverageNET);
			this.GroupBox1.Controls.Add(this.radioButtonPredictedNET);
			this.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.GroupBox1.Location = new System.Drawing.Point(20, 1);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(154, 119);
			this.GroupBox1.TabIndex = 2;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "NET Values";
			// 
			// checkBoxShowResiduals
			// 
			this.checkBoxShowResiduals.Dock = System.Windows.Forms.DockStyle.Top;
			this.checkBoxShowResiduals.Location = new System.Drawing.Point(3, 80);
			this.checkBoxShowResiduals.Name = "checkBoxShowResiduals";
			this.checkBoxShowResiduals.Size = new System.Drawing.Size(148, 24);
			this.checkBoxShowResiduals.TabIndex = 3;
			this.checkBoxShowResiduals.Text = "Show Residuals";
			this.checkBoxShowResiduals.CheckedChanged += new System.EventHandler(this.checkBoxShowResiduals_CheckedChanged);
			// 
			// radioButtonAverageNET
			// 
			this.radioButtonAverageNET.Dock = System.Windows.Forms.DockStyle.Top;
			this.radioButtonAverageNET.Location = new System.Drawing.Point(3, 48);
			this.radioButtonAverageNET.Name = "radioButtonAverageNET";
			this.radioButtonAverageNET.Size = new System.Drawing.Size(148, 32);
			this.radioButtonAverageNET.TabIndex = 1;
			this.radioButtonAverageNET.Text = "Average Observed NET";
			// 
			// radioButtonPredictedNET
			// 
			this.radioButtonPredictedNET.Checked = true;
			this.radioButtonPredictedNET.Dock = System.Windows.Forms.DockStyle.Top;
			this.radioButtonPredictedNET.Location = new System.Drawing.Point(3, 16);
			this.radioButtonPredictedNET.Name = "radioButtonPredictedNET";
			this.radioButtonPredictedNET.Size = new System.Drawing.Size(148, 32);
			this.radioButtonPredictedNET.TabIndex = 0;
			this.radioButtonPredictedNET.TabStop = true;
			this.radioButtonPredictedNET.Text = "Predicted NET";
			// 
			// mnuMainMenu
			// 
			this.mnuMainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.mnuFile,
																						this.mnuTools,
																						this.mnuAbout});
			// 
			// mnuFile
			// 
			this.mnuFile.Index = 0;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuFileOpenDatasetList,
																					this.mnuFileSep1,
																					this.mnuFileExit});
			this.mnuFile.Text = "&File";
			// 
			// mnuFileOpenDatasetList
			// 
			this.mnuFileOpenDatasetList.Index = 0;
			this.mnuFileOpenDatasetList.Text = "&Open Dataset List";
			this.mnuFileOpenDatasetList.Click += new System.EventHandler(this.mnuFileOpenDatasetList_Click);
			// 
			// mnuFileSep1
			// 
			this.mnuFileSep1.Index = 1;
			this.mnuFileSep1.Text = "-";
			// 
			// mnuFileExit
			// 
			this.mnuFileExit.Index = 2;
			this.mnuFileExit.Text = "E&xit";
			this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
			// 
			// mnuTools
			// 
			this.mnuTools.Index = 1;
			this.mnuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuToolsCreateDB,
																					 this.mnuToolsCreateMTDBFiles,
																					 this.mnuToolsAlignSelected,
																					 this.mnuToolsOptions});
			this.mnuTools.Text = "&Tools";
			// 
			// mnuToolsCreateDB
			// 
			this.mnuToolsCreateDB.Enabled = false;
			this.mnuToolsCreateDB.Index = 0;
			this.mnuToolsCreateDB.Text = "&Create Mass Tag Database";
			this.mnuToolsCreateDB.Click += new System.EventHandler(this.mnuToolsCreateDB_Click);
			// 
			// mnuToolsAlignSelected
			// 
			this.mnuToolsAlignSelected.Enabled = false;
			this.mnuToolsAlignSelected.Index = 2;
			this.mnuToolsAlignSelected.Text = "&Align Selected Dataset";
			this.mnuToolsAlignSelected.Click += new System.EventHandler(this.mnuToolsAlignSelected_Click);
			// 
			// mnuToolsOptions
			// 
			this.mnuToolsOptions.Index = 3;
			this.mnuToolsOptions.Text = "&Options";
			this.mnuToolsOptions.Click += new System.EventHandler(this.mnuToolsOptions_Click);
			// 
			// mnuAbout
			// 
			this.mnuAbout.Index = 2;
			this.mnuAbout.Text = "&About";
			this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter2.Location = new System.Drawing.Point(954, 0);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(6, 517);
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
			this.ctlChartScanVsNET.DefaultZoomHandler.FillColor = System.Drawing.Color.FromArgb(((System.Byte)(60)), ((System.Byte)(119)), ((System.Byte)(136)), ((System.Byte)(153)));
			this.ctlChartScanVsNET.DefaultZoomHandler.LineColor = System.Drawing.Color.Black;
			this.ctlChartScanVsNET.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctlChartScanVsNET.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			penProvider1.Color = System.Drawing.Color.FromArgb(((System.Byte)(211)), ((System.Byte)(211)), ((System.Byte)(211)));
			penProvider1.Width = 1F;
			this.ctlChartScanVsNET.GridLinePen = penProvider1;
			this.ctlChartScanVsNET.HilightColor = System.Drawing.Color.Magenta;
			this.ctlChartScanVsNET.Legend.BackColor = System.Drawing.Color.Transparent;
			penProvider2.Color = System.Drawing.Color.Black;
			penProvider2.Width = 1F;
			this.ctlChartScanVsNET.Legend.BorderPen = penProvider2;
			this.ctlChartScanVsNET.Legend.Bounds = new System.Drawing.Rectangle(764, 80, 181, 392);
			this.ctlChartScanVsNET.Legend.ColumnWidth = 125;
			this.ctlChartScanVsNET.Legend.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.ctlChartScanVsNET.Legend.MaxFontSize = 12F;
			this.ctlChartScanVsNET.Legend.MinFontSize = 6F;
			this.ctlChartScanVsNET.Location = new System.Drawing.Point(0, 0);
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
			this.ctlChartScanVsNET.Size = new System.Drawing.Size(954, 517);
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
			// statusBarMessages
			// 
			this.statusBarMessages.Location = new System.Drawing.Point(0, 771);
			this.statusBarMessages.Name = "statusBarMessages";
			this.statusBarMessages.Size = new System.Drawing.Size(1136, 22);
			this.statusBarMessages.TabIndex = 7;
			this.statusBarMessages.Text = "MTDBCreator - Use File -> Open to Load Processing List";
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 517);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(1136, 6);
			this.splitter1.TabIndex = 8;
			this.splitter1.TabStop = false;
			// 
			// mnuToolsCreateMTDBFiles
			// 
			this.mnuToolsCreateMTDBFiles.Enabled = false;
			this.mnuToolsCreateMTDBFiles.Index = 1;
			this.mnuToolsCreateMTDBFiles.Text = "Create MT DB Files for &Manual Import";
			this.mnuToolsCreateMTDBFiles.Click += new System.EventHandler(this.mnuToolsCreateMTDBFiles_Click);
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1136, 793);
			this.Controls.Add(this.ctlChartScanVsNET);
			this.Controls.Add(this.splitter2);
			this.Controls.Add(this.expandPanelOptions);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.expandPanelLists);
			this.Controls.Add(this.statusBarMessages);
			this.Menu = this.mnuMainMenu;
			this.Name = "frmMain";
			this.Text = "MTDBCreator";
			((System.ComponentModel.ISupportInitialize)(this.expandPanelLists)).EndInit();
			this.expandPanelLists.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.expandPanelOptions)).EndInit();
			this.expandPanelOptions.ResumeLayout(false);
			this.GroupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ctlChartScanVsNET)).EndInit();
			this.ResumeLayout(false);

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
			ListViewItem listItem = listViewDatasets.Items.Add((analysis.mintDatasetId+1).ToString()) ; 
			listItem.Tag = analysis ; 
			listItem.SubItems.Add(analysis.mstrDataset) ; 
		}

		#endregion 

		#region "Loading functions"
		private void LoadDisplayXTandemDataset(clsAnalysisDescription analysis)
		{
			DateTime now = DateTime.Now ; 
			mdbl_slope = 0 ;
			mdbl_intercept = 0 ;
			double rsquared = 0 ; 
			int numScans = 0 ; 

			mfrmStatus.Text = analysis.mstrDataset ; 
			mfrmStatus.SetStatusMessage("Reading PHRP files") ; 
			mfrmStatus.Reset() ;

			mobjXTandemPHRPResultsReader = new clsXTandemAnalysisReader(analysis.mstrArchivePath, 
				analysis.mstrDataset, mfrmStatus) ; 
			DateTime read = DateTime.Now; 

					if (!radioButtonAverageNET.Checked)
			{
				mobjMTDB.AlignXTandemDatasetToTheoreticalNETs(mobjXTandemPHRPResultsReader,  
					ref marrPeptideScans, ref marrPeptidePredictedNETs, ref mdbl_slope, ref mdbl_intercept, 
					ref numScans, ref rsquared) ; 
			}
			else
			{
				mobjMTDB.AlignXTandemDatasetToAverageNETs(mobjXTandemPHRPResultsReader, ref marrPeptideScans, 
					ref marrPeptidePredictedNETs, ref mdbl_slope, ref mdbl_intercept, ref rsquared) ; 
			}
			DisplayScansVsNet(analysis.mstrDataset) ; 
		}

		private void LoadDisplaySequestDataset(clsAnalysisDescription analysis)
		{
			DateTime now = DateTime.Now ; 
			mdbl_slope = 0 ;
			mdbl_intercept = 0 ;
			double rsquared = 0 ; 
			int numScans = 0 ; 

			mfrmStatus.Text = analysis.mstrDataset ; 
			mfrmStatus.SetStatusMessage("Reading PHRP files") ; 
			mfrmStatus.Reset() ;

			mobjSequestPHRPResultsReader = new clsSequestAnalysisReader(analysis.mstrArchivePath, 
				analysis.mstrDataset, mfrmStatus) ; 
			DateTime read = DateTime.Now; 

			if (!radioButtonAverageNET.Checked)
				mobjMTDB.AlignSequestDatasetToTheoreticalNETs(mobjSequestPHRPResultsReader, ref marrPeptideScans, 
					ref marrPeptidePredictedNETs, ref mdbl_slope, ref mdbl_intercept, 
					ref numScans, ref rsquared) ; 
			else
				mobjMTDB.AlignSequestDatasetToAverageNETs(mobjSequestPHRPResultsReader,
					ref marrPeptideScans, ref marrPeptidePredictedNETs, ref mdbl_slope, 
					ref mdbl_intercept, ref rsquared) ; 
			DisplayScansVsNet(analysis.mstrDataset) ; 
		}

		#endregion

		#region "Processing"
		private void ProcessAllDatasets()
		{
			try
			{
				mblnCreatingDB = true ; 
				mstrErrors = "" ; 
				mfrmStatus.Reset() ;

				bool stopProcessing = false ; 
				for (int i = 0 ; i < listViewDatasets.Items.Count ; i++)
				{
					try
					{
						ProcessItem(i) ; 
					}
					catch (AnalysisToolException ex)
					{
						stopProcessing = true ; 
						break ; 
					}
					catch (Exception ex)
					{
						ListViewItem item = listViewDatasets.Items[i] ; 
						clsAnalysisDescription analysis = (clsAnalysisDescription) item.Tag ; 
						mstrErrors += "Ignoring " + analysis.mstrDataset + " because of errors\n" ; 
					}
				}
				if (!stopProcessing)
				{
					mobjMTDB.CalculateMassTagNETs() ; 
					mobjMTDB.CalculateProteinsPassingFilters() ; 
					mobjMTDB.LoadResultsIntoDB(m_WritingToAccessDB) ; 

					mfrmStatus.SetStatusMessage("Processing is complete") ;
				}

				if (mfrmStatus.HasErrorMessages)
					mfrmStatus.CloseButtonVisible = true ;
				else
					mfrmStatus.Hide() ; 

				GC.Collect();

			}
			finally
			{
				mblnCreatingDB = false ; 
			}

		}

		private void ProcessCurrentDataset()
		{
			try
			{
				if (listViewDatasets.SelectedItems.Count == 0)
					return ; 
				int selectedIndex = listViewDatasets.SelectedIndices[0] ; 
				ProcessItem(selectedIndex) ; 
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message) ; 
			}
		}

		private void ProcessXTandemFile(clsAnalysisDescription analysis, ListViewItem item)
		{
			DateTime now = DateTime.Now ; 
			mdbl_slope = 0 ;
			mdbl_intercept = 0 ;
			double rsquared = 0 ; 
			int numScans = 0 ; 

			mfrmStatus.Text = analysis.mstrDataset ; 
			mfrmStatus.SetStatusMessage("Reading PHRP files") ; 
			mobjXTandemPHRPResultsReader = new clsXTandemAnalysisReader(analysis.mstrArchivePath, 
				analysis.mstrDataset, mfrmStatus) ; 
			DateTime read = DateTime.Now; 

			mfrmStatus.SetStatusMessage("Performing Alignment") ; 
			
			mobjMTDB.AlignXTandemDatasetToTheoreticalNETs(mobjXTandemPHRPResultsReader, 
				ref marrPeptideScans, ref marrPeptidePredictedNETs, ref mdbl_slope, 
				ref mdbl_intercept, ref numScans, ref rsquared) ; 

			DisplayScansVsNet(analysis.mstrDataset) ; 
			analysis.mdbl_scan_net_intercept = mdbl_intercept ; 
			analysis.mdbl_scan_net_slope = mdbl_slope ;
			analysis.mint_num_scans = numScans ; 
			analysis.mdbl_scan_net_rsquared = rsquared ; 
			analysis.mint_num_unique_mass_tags = mobjXTandemPHRPResultsReader.marrSeqInfo.Length ; 
			mobjMTDB.AddResults(mobjXTandemPHRPResultsReader, mobjOptions.RegressionType, analysis) ; 


			item.SubItems.Add(Convert.ToString(analysis.mdbl_scan_net_slope)) ; 
			item.SubItems.Add(Convert.ToString(analysis.mdbl_scan_net_intercept)) ; 
			item.SubItems.Add(Convert.ToString(analysis.mdbl_scan_net_rsquared)) ; 
			item.SubItems.Add(Convert.ToString(analysis.mint_num_unique_mass_tags)) ; 
			DateTime add = DateTime.Now ; 
			Console.WriteLine("# of items = " + mobjXTandemPHRPResultsReader.marrXTandemResults.Length +  " Read Time = " + Convert.ToString(read.Subtract(now)) + " Insert time = " + Convert.ToString(add.Subtract(read))) ; 
		}

		private void ProcessSequestFile(clsAnalysisDescription analysis, ListViewItem item)
		{
			DateTime now = DateTime.Now ; 
			mdbl_slope = 0 ;
			mdbl_intercept = 0 ;
			double rsquared = 0 ; 
			int numScans = 0 ; 
			
			mfrmStatus.Text = analysis.mstrDataset ; 
			mfrmStatus.SetStatusMessage("Reading PHRP files") ; 
			mobjSequestPHRPResultsReader = new clsSequestAnalysisReader(analysis.mstrArchivePath, 
				analysis.mstrDataset, mfrmStatus) ; 
			DateTime read = DateTime.Now; 

			mfrmStatus.SetStatusMessage("Performing Alignment") ; 

			mobjMTDB.AlignSequestDatasetToTheoreticalNETs(mobjSequestPHRPResultsReader,  
				ref marrPeptideScans, ref marrPeptidePredictedNETs, ref mdbl_slope, 
				ref mdbl_intercept, ref numScans, ref rsquared ) ; 

			DisplayScansVsNet(analysis.mstrDataset) ; 
			analysis.mdbl_scan_net_intercept = mdbl_intercept ; 
			analysis.mdbl_scan_net_slope = mdbl_slope ;
			analysis.mint_num_scans = numScans ; 
			analysis.mdbl_scan_net_rsquared = rsquared ; 
			analysis.mint_num_unique_mass_tags = mobjSequestPHRPResultsReader.marrSeqInfo.Length ; 
			mobjMTDB.AddResults(mobjSequestPHRPResultsReader, mobjOptions.RegressionType, analysis) ; 


			item.SubItems.Add(Convert.ToString(analysis.mdbl_scan_net_slope)) ; 
			item.SubItems.Add(Convert.ToString(analysis.mdbl_scan_net_intercept)) ; 
			item.SubItems.Add(Convert.ToString(analysis.mdbl_scan_net_rsquared)) ; 
			item.SubItems.Add(Convert.ToString(analysis.mint_num_unique_mass_tags)) ; 
			DateTime add = DateTime.Now ; 
			Console.WriteLine("# of items = " + mobjSequestPHRPResultsReader.marrSequestResults.Length +  " Read Time = " + Convert.ToString(read.Subtract(now)) + " Insert time = " + Convert.ToString(add.Subtract(read))) ; 
		}

		private void ProcessItem(int itemNum)
		{
			ListViewItem item = listViewDatasets.Items[itemNum] ; 
			item.Selected = true ; 
			try
			{
				mdbl_slope = 0 ;
				mdbl_intercept = 0 ;

				clsAnalysisDescription analysis = (clsAnalysisDescription) item.Tag ; 
				analysis.mdbl_scan_net_slope = 0 ;
				analysis.mdbl_scan_net_intercept = 0 ;

				if (analysis.mstrAnalysisTool.ToUpper() == "XTANDEM")
				{
					ProcessXTandemFile(analysis, item)   ;
				}
				else if  (analysis.mstrAnalysisTool.ToUpper() == "SEQUEST")
				{
					ProcessSequestFile(analysis, item) ; 
				}
				else
				{
					DialogResult rs = MessageBox.Show(this, "Unrecognized tool. Use Sequest or XTandem. Continue with the remaining (skip current)?", "Analysis Tool Error", MessageBoxButtons.YesNo) ; 
					if (rs == DialogResult.No)
					{
						throw new AnalysisToolException("Error in analysis tool type. Skipped all files") ; 
					}
				}
			}
			catch (Exception ex)
			{
				throw ex ;
			}
		}

		#endregion 

		#region "Display functions"
		private void OpenSelectedDatasetThreaded()
		{
			mstrErrors = "" ; 
			mfrmStatus.Reset() ;

			System.Threading.ThreadStart start = new System.Threading.ThreadStart(this.ShowSelectedDatasetAndCloseMessage) ; 
			System.Threading.Thread thrd = new System.Threading.Thread(start) ; 
			thrd.Start() ; 

			mfrmStatus.ShowStatusBox(this, "Aligning Run") ;
 
			if (mstrErrors != "")
			{
				MessageBox.Show(this, mstrErrors, "Errors in processing") ; 
			}
		}

		private void ShowSelectedDatasetAndCloseMessage() 
		{
			ShowSelectedDataset() ; 
			System.Threading.Thread.Sleep(500) ; 

			if (mfrmStatus.HasErrorMessages)
				mfrmStatus.CloseButtonVisible = true ;
			else
				mfrmStatus.Hide() ; 
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

		# region "UI Responses"
		private void mnuFileOpenDatasetList_Click(object sender, System.EventArgs e)
		{
			try
			{

				radioButtonPredictedNET.Checked = true ; 
				radioButtonAverageNET.Enabled = false ; 

				OpenFileDialog openFileDialog1 = new OpenFileDialog();

				//openFileDialog1.InitialDirectory = "c:\\projects\\MTDBCreator\\data\\" ;
				openFileDialog1.Filter = "txt files (*.txt)|*.txt" ;
				openFileDialog1.FilterIndex = 1 ;
				openFileDialog1.RestoreDirectory = true ;

				if(openFileDialog1.ShowDialog() == DialogResult.OK)
				{
					this.listViewDatasets.Items.Clear() ;
					clsAnalysisDescriptionReader reader = new clsAnalysisDescriptionReader(openFileDialog1.FileName) ; 
					marrAnalyses = reader.Analyses ; 
					AddAnalysesTolist(marrAnalyses) ; 
					// Disabled in this version since Office Support has been removed mnuToolsCreateDB.Enabled = true ; 
					mnuToolsCreateMTDBFiles.Enabled = true ; 
					mnuToolsAlignSelected.Enabled = true ; 
					statusBarMessages.Text = "Double Click Item to Perform Alignment or Select Tools -> Create Mass Tag Database for creating a mass tag database" ; 
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message) ; 
			}
		}

		private void mnuToolsCreateDB_Click(object sender, System.EventArgs e)
		{
			mobjMTDB = null ; 
			mobjMTDB = new clsMTDB(mfrmStatus, mobjOptions) ; 

			SaveFileDialog saveFileDialog = new SaveFileDialog() ; 
			saveFileDialog.Title = "Select output database" ; 
			saveFileDialog.Filter =  "Microsoft Access Files (*.mdb)|*.mdb" ; 
			saveFileDialog.FilterIndex = 1 ; 
			saveFileDialog.RestoreDirectory = true ; 
			mstrErrors = "" ; 
			mfrmStatus.Reset() ;

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				mstrAccessDbPath = saveFileDialog.FileName ; 
			}
			else
			{
				MessageBox.Show("No database output selected. Defaulting to untitled.mdb") ; 
				mstrAccessDbPath = "untitled.mdb" ; 
			}
			mobjMTDB.AccessDBPath = mstrAccessDbPath ; 
			try
			{
				System.IO.File.Delete(mstrAccessDbPath) ; 
			}
			catch (Exception ex)
			{
			}

			CreateMTDBFiles(true);
		}

		private void checkBoxShowResiduals_CheckedChanged(object sender, System.EventArgs e)
		{
			if (listViewDatasets.SelectedItems.Count == 0)
				return ; 
			int selectedIndex = listViewDatasets.SelectedIndices[0] ; 
			clsAnalysisDescription analysis = (clsAnalysisDescription) listViewDatasets.Items[selectedIndex].Tag ; 
			DisplayScansVsNet(analysis.mstrDataset) ; 
		}

		private void mnuAbout_Click(object sender, System.EventArgs e)
		{
			frmAbout aboutForm = new frmAbout() ;
			aboutForm.ShowDialog() ; 
		}
		#endregion

		private void listViewDatasets_MouseHover(object sender, System.EventArgs e)
		{
			if (listViewDatasets.Items.Count != 0)
			{
				statusBarMessages.Text = "Double Click a dataset to view alignment" ;								
			}
			else 
			{
				statusBarMessages.Text = "Use File -> Open to Load List with path to results from Peptides Hits Results Processor which will be used to create a MassTag database" ; 
			}
		}

		private void listViewDatasets_DoubleClick(object sender, EventArgs e)
		{
			if (mblnCreatingDB)
				return ; 
			OpenSelectedDatasetThreaded() ; 
		}

		private void mnuToolsAlignSelected_Click(object sender, System.EventArgs e)
		{
			if (listViewDatasets.SelectedItems.Count == 0)
			{
				MessageBox.Show("No dataset selected. Please select a dataset and try again. Or double click the dataset you would like to open") ; 
				return ; 
			}
			OpenSelectedDatasetThreaded() ; 
		}

		private void mnuToolsOptions_Click(object sender, System.EventArgs e)
		{
			frmOptions optionsForm = new frmOptions() ; 
			optionsForm.Options = mobjOptions ; 
			if (optionsForm.ShowDialog(this) == DialogResult.OK)
			{
				mobjOptions = optionsForm.Options ;
				mobjMTDB.Options = mobjOptions ; 
			}
		}

		private void mnuFileExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void mnuToolsCreateMTDBFiles_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.DialogResult eResponse;

			mobjMTDB = null ; 
			mobjMTDB = new clsMTDB(mfrmStatus, mobjOptions) ; 

			eResponse = MessageBox.Show("Creating MT DB Files, which you can then manually import.  Continue?", "Processing", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button1) ; 

			if (eResponse == System.Windows.Forms.DialogResult.Yes) 
			{
				mstrErrors = "" ; 
				mfrmStatus.Reset() ;

				mstrAccessDbPath = "untitled.mdb" ;
				mobjMTDB.AccessDBPath = mstrAccessDbPath ; 

				CreateMTDBFiles(false);
			}
		}


	}
	public class AnalysisToolException : System.Exception
	{
		public AnalysisToolException(string message): base(message)
		{
		} 
	}
}
