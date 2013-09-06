using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MTDBCreator.Data;
using PNNL.Controls;
using MTDBCreator.Algorithms;
using MTDBCreator.IO;

namespace MTDBCreator.Forms
{

    public partial class AnalysisControl : UserControl
    {
        private const int CONST_WAITING    = 2;
        private const int CONST_COMPLETE   = 1;
        private const int CONST_PROCESSING = 0;

        public event EventHandler<StatusEventArgs> Status;
        public event EventHandler<AnalysisCompletedEventArgs> AnalysisCompleted;
        public event EventHandler<DatabaseCreatedEventArgs>   DatabaseCreated;
        private const string CONST_DEFAULT_DATABASE_NAME = "untitled";

        /// <summary>
        /// List of rendered analysis.
        /// </summary>
        private Dictionary<Analysis, clsSeries> m_displayCache;

        /// <summary>
        /// Database processor
        /// </summary>
        private MTDBProcessor m_processor;
        private SaveFileDialog m_saveMTDBDialog;
        delegate void DelegateUpdateMessage(string message);
        private ChartPostRenderingProcessor mobjTransformPlotter;
        private Options m_options = new Options();        
        private AnalysisManager m_manager;
        private MassTagDatabase m_database;

        /// <summary>
        /// This is the only analysis to display...
        /// </summary>
        private Analysis m_selectedAnalysis;


        public AnalysisControl()
        {
            InitializeComponent();            

            m_processor                     = new MTDBProcessor();
            m_processor.DatabaseCreated     += new EventHandler<DatabaseCreatedEventArgs>(processor_DatabaseCreated);
            m_processor.Status              += new EventHandler<StatusEventArgs>(processor_Status);
            m_processor.Error               += new EventHandler<StatusEventArgs>(processor_Error);
            m_processor.AnalysisCompleted   += new EventHandler<AnalysisCompletedEventArgs>(processor_AnalysisCompleted);
            m_processor.ProcessingAnalysis  += new EventHandler<AnalysisCompletedEventArgs>(processor_ProcessingAnalysis);
            m_processor.AnalysisFailed += new EventHandler<DatabaseCreatedEventArgs>(m_processor_AnalysisFailed);
                
            m_displayCache                  = new Dictionary<Analysis,clsSeries>();
            m_manager                       = new AnalysisManager();
            radioButtonPredictedNET.Checked = true;
            radioButtonAverageNET.Enabled   = false;
            
            m_database                      = null;
            listViewDatasets.SmallImageList = imageList1;

            // Create a save MTDB Dialog window for selecting where to output the data
            m_saveMTDBDialog = new SaveFileDialog();
            m_saveMTDBDialog.FileName = CONST_DEFAULT_DATABASE_NAME;
            
            mobjTransformPlotter = new ChartPostRenderingProcessor(this.DrawNetRegressionLine);
            ctlChartScanVsNET.AddPostProcessor(mobjTransformPlotter, PostProcessPriority.Mid);

            listViewDatasets.DoubleClick += new EventHandler(listViewDatasets_DoubleClick);            
        }


        public void AddAnalysis(List<Analysis> analyses)
        {
            AddAnalysesTolist(analyses);            
        }
        private void AddAnalysesTolist(List<Analysis> analyses)
        {
            for (int i = 0; i < analyses.Count; i++)
            {
                analyses[i].Id = i;
                AddAnalysisToList(analyses[i]);
            }
        }
        private void AddAnalysisToList(Analysis analysis)
        {
            m_manager.AddDataset(analysis.Name, analysis);
            ListViewItem listItem   = new ListViewItem();
            listItem.Tag            = analysis.Name;
            listItem.ImageIndex     = CONST_WAITING;
            listItem.SubItems.Add(new ListViewItem.ListViewSubItem(listItem, analysis.Name));
            listItem.SubItems.Add(new ListViewItem.ListViewSubItem(listItem, analysis.NumberOfUniqueMassTags.ToString()));
            listItem.SubItems.Add(new ListViewItem.ListViewSubItem(listItem, analysis.Proteins.Count.ToString()));                       
            listViewDatasets.Items.Add(listItem);
        }
        private ListViewItem FindItem(string name)
        {
            ListViewItem currentItem = null;
            foreach (ListViewItem item in listViewDatasets.Items)
            {
                if (item.Tag.ToString() == name)
                {
                    currentItem = item;
                    break;
                }
            }
            return currentItem;
        }
        private void UpdateAnalysisView(Analysis analysis)
        {
            ListViewItem currentItem = FindItem(analysis.Name);

            if (currentItem != null)
            {
                if (currentItem.SubItems.Count < 1)
                {
                    currentItem.SubItems.Add(new ListViewItem.ListViewSubItem());
                }
                currentItem.SubItems[2].Text = analysis.Targets.Count.ToString();
                currentItem.SubItems[3].Text = analysis.Proteins.Count.ToString();

                switch (analysis.ProcessedState)
                {
                    case ProcessingState.Processing:
                        currentItem.ImageIndex = CONST_PROCESSING;
                        break;
                    case ProcessingState.Processed:
                        currentItem.ImageIndex = CONST_COMPLETE;
                        break;
                    case ProcessingState.NotProcessed:
                        currentItem.ImageIndex = CONST_WAITING;
                        break;
                    default:
                        break;
                }
            }
        }
        private void UpdateAnalysisDisplay()
        {
            // Re-render the analysis.
            foreach(Analysis analysis in m_displayCache.Keys)
            {
                DisplayScansVsNet(analysis);
            }
        }
        private Analysis GetSelectedAnalysis()
        {
            Analysis analysis = null;
            try
            {
                if (listViewDatasets.SelectedItems.Count == 0)
                    return analysis;

                int selectedIndex   = listViewDatasets.SelectedIndices[0];
                ListViewItem item   = listViewDatasets.Items[selectedIndex];
                string datasetName  = item.Tag.ToString();
                analysis            = m_manager.GetAnalysis(datasetName);
            }
            catch (Exception)
            {
                OnStatus("No analysis was selected.");
            }

            return analysis;
        }


        /// <summary>
        /// Displays the selected dataset.
        /// </summary>
        private void ShowSelectedDataset()
        {
            try
            {
                Analysis analysis = GetSelectedAnalysis();
                if (analysis != null)
                {

                    m_selectedAnalysis = analysis;

                    // If the analysis has not been processed, then do so...
                    if (analysis.ProcessedState != ProcessingState.Processed)
                    {
                        //LockUserInterface(true);

                        IRetentionTimePredictor predictor = RetentionTimePredictorFactory.CreatePredictor(m_options.PredictorType);
                        m_processor.ProcessAnalysisJob(analysis,
                                                        m_options,
                                                        predictor);                        
                    }
                    else
                    {
                        ctlChartScanVsNET.SeriesCollection.Clear();
                        DisplayScansVsNet(analysis);
                    }
                }
                else
                {
                    OnStatus("No analysis was selected.");
                }
            }
            catch (Exception ex)
            {
                OnStatus(ex.Message);
            }
        }
        /// <summary>
        /// Displays the scan vs. NET values for a single analysis.
        /// </summary>
        /// <param name="analysis"></param>
        private void DisplayScansVsNet(Analysis analysis)
        {            
            // Either the display does not have the analysis, or the display series was reset and needs to be re-rendered
            if (!m_displayCache.ContainsKey(analysis) || m_displayCache[analysis] == null)
            {
                BubbleShape shape = new BubbleShape(1, false);
                clsColorIterator colors = new clsColorIterator();
                Color plotColor = colors.GetColor(analysis.Id);
                clsPlotParams parameters = new clsPlotParams(shape, plotColor, false, true, true);
                parameters.Name = analysis.Name;

                float[] ordinate = new float[analysis.Targets.Count];
                float[] abscissa = new float[analysis.Targets.Count];
                int i = 0;
                if (!mcheckBox_useResiduals.Checked)
                {
                    foreach (Target target in analysis.Targets)
                    {                        
                        if (radioButtonAverageNET.Checked)
                        {
                            
                            ordinate[i]   = Convert.ToSingle(target.ParentTarget.GaNetAverage);
                            abscissa[i++] = Convert.ToSingle(target.Scan);
                        }
                        else
                        {
                            ordinate[i]   = Convert.ToSingle(target.NetPredicted);
                            abscissa[i++] = Convert.ToSingle(target.Scan);
                        }
                    }
                }
                else
                {
                    foreach (Target target in analysis.Targets)
                    {

                        if (radioButtonAverageNET.Checked)
                        {
                            ordinate[i]   = Convert.ToSingle(target.ParentTarget.GaNetAverage - target.NetAligned);
                            abscissa[i++] = Convert.ToSingle(target.Scan);
                        }
                        else
                        {
                            ordinate[i] = Convert.ToSingle(target.NetPredicted - target.NetAligned);
                            abscissa[i++] = Convert.ToSingle(target.Scan);
                        }
                    }
                }

                clsSeries newSeries = new clsSeries(new ArrayChartDataProvider(abscissa, ordinate), parameters);
                if (!m_displayCache.ContainsKey(analysis))
                    m_displayCache.Add(analysis, null);
                m_displayCache[analysis] = newSeries;
            }

            clsSeries series = m_displayCache[analysis];
            ctlChartScanVsNET.SeriesCollection.Add(series);
            ctlChartScanVsNET.AutoViewPort();
        }
        private void DrawNetRegressionLine(ctlChartBase sender, PostRenderEventArgs args)
        {
            try
            {
                if (mcheckBox_displayRegression.Checked == false)
                {
                    return;
                }

                foreach (Analysis analysis in m_displayCache.Keys)
                {
                    // Here we only draw the regression line for the selected analysis, unless it's null, which means the user wants to see all of the analysis together.
                    if (analysis == m_selectedAnalysis || m_selectedAnalysis == null)
                    {
                        if (analysis.ProcessedState == ProcessingState.Processed)
                        {
                            IRegressionAlgorithm regressor = analysis.RegressionResults.Regressor;
                            float xFrom = sender.ViewPort.X;
                            float xFinal = sender.ViewPort.Right;

                            clsColorIterator iterator = new clsColorIterator();
                            Color color = iterator.GetColor(analysis.Id);
                            color = Color.FromArgb(120, color);

                            using (Pen mpen_line = new Pen(color, 5))
                            {
                                if (!mcheckBox_useResiduals.Checked)
                                {
                                    int numDivisions = 10;
                                    float step = (xFinal - xFrom) / numDivisions;
                                    for (; xFrom < xFinal; xFrom += step)
                                    {
                                        float yFrom = Convert.ToSingle(regressor.GetTransformedNET(Convert.ToInt32(xFrom)));
                                        float xTo = xFrom + step;
                                        float yTo = Convert.ToSingle(regressor.GetTransformedNET(Convert.ToInt32(xTo)));

                                        args.Graphics.DrawLine(mpen_line, sender.GetScreenPixelX(xFrom), sender.GetScreenPixelY(yFrom),
                                            sender.GetScreenPixelX(xTo), sender.GetScreenPixelY(yTo));
                                    }
                                }
                                else
                                {
                                    args.Graphics.DrawLine(mpen_line, sender.GetScreenPixelX(xFrom), sender.GetScreenPixelY(0),
                                        sender.GetScreenPixelX(xFinal), sender.GetScreenPixelY(0));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // who cares!
            }
        }
    
                
        #region Form Event Handlers
        private void checkBoxShowResiduals_CheckedChanged(object sender, System.EventArgs e)
        {
            if (listViewDatasets.SelectedItems.Count == 0)
                return;

            int selectedIndex   = listViewDatasets.SelectedIndices[0];
            string name         = listViewDatasets.Items[selectedIndex].Tag.ToString();
            Analysis analysis   = m_manager.GetAnalysis(name);

            if (analysis.ProcessedState == ProcessingState.Processed)
            {
                DisplayScansVsNet(analysis);
            }
            else
            {
                UpdateStatus("The selected analysis contains no data yet.");
            }
        }
        private void listViewDatasets_DoubleClick(object sender, EventArgs e)
        {
            ShowSelectedDataset();
        }
        #endregion

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAbout aboutForm = new frmAbout();
            aboutForm.Icon = ParentForm.Icon;
            aboutForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmOptions optionsForm = new frmOptions();
            optionsForm.Options = m_options;
            if (optionsForm.ShowDialog(this) == DialogResult.OK)
            {
                m_options = optionsForm.Options;
            }
        }        
        void processor_ProcessingAnalysis(object sender, AnalysisCompletedEventArgs e)
        {
            // This is a thread safe thing here.  We can never access the GUI elements except from the 
            // thread that created it...STA.
            if (InvokeRequired)
            {
                Invoke(new EventHandler<AnalysisCompletedEventArgs>(processor_ProcessingAnalysis), sender, e);
                return;
            }

            UpdateAnalysisView(e.Analysis);

        }
        
        void processor_AnalysisCompleted(object sender, AnalysisCompletedEventArgs e)
        {            
            // This is a thread safe thing here.  We can never access the GUI elements except from the 
            // thread that created it...STA.
            if (InvokeRequired)
            {
                Invoke(new EventHandler<AnalysisCompletedEventArgs>(processor_AnalysisCompleted), sender, e);
                return;
            }

            OnStatus(string.Format("...Analysis {0}/{1} completed: {2}", e.Number, e.Total, e.Analysis.Name));
            DisplayScansVsNet(e.Analysis);
            UpdateAnalysisView(e.Analysis);

            if (AnalysisCompleted != null)
            {
                AnalysisCompleted(sender, e);
            }

            mprogress_analysis.Visible  = true;
            mprogress_analysis.Minimum  = 0;
            mprogress_analysis.Maximum  = e.Total;
            mprogress_analysis.Value    = e.Number;
        }

        void processor_Error(object sender, StatusEventArgs e)
        {
            //TODO: Move this to an error event handler with own event args...make it more explicit and possibly log the errors.

            // This is a thread safe thing here.  We can never access the GUI elements except from the 
            // thread that created it...STA.
            if (InvokeRequired)
            {
                Invoke(new EventHandler<StatusEventArgs>(processor_Error), sender, e);
                return;
            }

            OnStatus(e.Message);
        }
        void processor_Status(object sender, StatusEventArgs e)
        {
            // This is a thread safe thing here.  We can never access the GUI elements except from the 
            // thread that created it...STA.
            if (InvokeRequired)
            {
                Invoke(new EventHandler<StatusEventArgs>(processor_Status), sender, e);
                return;
            }

            OnStatus(e.Message);
        }
        void m_processor_AnalysisFailed(object sender, DatabaseCreatedEventArgs e)
        {
            // This is a thread safe thing here.  We can never access the GUI elements except from the 
            // thread that created it...STA.
            if (InvokeRequired)
            {
                Invoke(new EventHandler<DatabaseCreatedEventArgs>(m_processor_AnalysisFailed), sender, e);
                return;
            }

            try
            {
                mprogress_analysis.Visible = false;
            }
            catch
            {
            }
        }
        void processor_DatabaseCreated(object sender, DatabaseCreatedEventArgs e)
        {
            // This is a thread safe thing here.  We can never access the GUI elements except from the 
            // thread that created it...STA.
            if (InvokeRequired)
            {
                Invoke(new EventHandler<DatabaseCreatedEventArgs>(processor_DatabaseCreated), sender, e);
                return;
            }

            mprogress_analysis.Visible = false;
            m_database                    = e.Database;
            mbutton_saveDatabase.Visible  = true;
            radioButtonAverageNET.Enabled = true;

            databaseControl1.UpdateDatabaseView(m_database);
            OnStatus("Database Created.");
            if (DatabaseCreated != null)
            {
                DatabaseCreated(sender, new DatabaseCreatedEventArgs(e.Database));
            }
        }
        /// <summary>
        /// Saves the database to file.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="path"></param>
        private void SaveDatabase(MassTagDatabase database, string path)
        {
            IMassTagDatabaseWriter writer = MassTagDatabaseWriterFactory.CreateWriter(MtdbWriterType.Sqlite);

            OnStatus("Filtering Database For Export Based on Options.");
            // Here we make sure that we filter the database...
            MassTagDatabase filteredDatabase = new MassTagDatabase(database, m_options);

            OnStatus("Saving Database.");
            writer.WriteDatabase(filteredDatabase, path);

            OnStatus("Database Saved.");
            mbutton_showDatabase.Visible = true;
        }

        private void UpdateError(string message)
        {
            if (Status != null)
            {
                Status(this, new StatusEventArgs(message));
            }
        }
        private void UpdateStatus(string message)
        {
            if (Status != null)
            {
                Status(this, new StatusEventArgs(message));
            }
        }

        #region Processing Event Handlers        
        /// <summary>
        /// Notify people that the analysis is complete.
        /// </summary>
        void UpdateCompleteAnalysis(List<string> failed)
        {            
            radioButtonAverageNET.Enabled = true;            

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
            radioButtonAverageNET.Enabled = true;            
        }
        #endregion

        private void mbutton_align_Click(object sender, EventArgs e)
        {
            ShowSelectedDataset();
        }

        private void OnStatus(string message)
        {
            if (Status != null)
            {
                Status(this, new StatusEventArgs(message));
            }

            UpdateStatusMessage(message);
        }

        private void mbutton_showDatabase_Click(object sender, EventArgs e)
        {
            ctlChartScanVsNET.SeriesCollection.Clear();
            m_selectedAnalysis = null;

                         
                foreach (Analysis analysis in m_manager)
                {
                    if (analysis.ProcessedState == ProcessingState.Processed)
                    {                        
                        DisplayScansVsNet(analysis);
                    }
                }
            
        }

        private void mbutton_options_Click(object sender, EventArgs e)
        {
            frmOptions options  = new frmOptions();
            options.Options     = m_options;
            options.Icon        = ParentForm.Icon;
            options.StartPosition = FormStartPosition.CenterParent;
            DialogResult result = options.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (!m_options.Equals(options.Options))
                {                    
                    UpdateAnalysis(options.Options);
                    mbutton_saveDatabase.Visible = false;
                }
            }
        }
        /// <summary>
        /// Updates the analysis and options
        /// </summary>
        /// <param name="clsOptions"></param>
        private void UpdateAnalysis(Options newOptions)
        {
            m_options = newOptions;
            ctlChartScanVsNET.SeriesCollection.Clear();

            foreach (Analysis analysis in m_manager)
            {                
                analysis.Clear();
                UpdateAnalysisView(analysis);
            }
        }

        private void mbutton_saveDatabase_Click(object sender, EventArgs e)
        {
            m_saveMTDBDialog.Title              = "Select output database";
            m_saveMTDBDialog.Filter             = "Sqlite (*.db3)|*.db3";
            m_saveMTDBDialog.FilterIndex        = 1;
            m_saveMTDBDialog.RestoreDirectory   = true;
            
            if (m_saveMTDBDialog.ShowDialog() == DialogResult.OK)
            {
                if (m_database != null)
                {
                    OnStatus("...Saving mass tag database.");
                    string databasePath = m_saveMTDBDialog.FileName;
                    SaveDatabase(m_database, databasePath);
                    mbutton_saveDatabase.BackColor = Color.White;
                    mbutton_saveDatabase.FlatAppearance.BorderColor = Color.Black;
                    mbutton_saveDatabase.ForeColor = Color.Black;
                }
                else
                {
                    OnStatus("You have to create the database first.");
                }
            }
        }
        /// <summary>
        /// Invalidates the display cache so that it will be re-rendered 
        /// </summary>
        private void InvalidateDisplayCache()
        {
            ctlChartScanVsNET.SeriesCollection.Clear();

            List<Analysis> keys = new List<Analysis>();
            foreach (Analysis analysis in m_displayCache.Keys)
                keys.Add(analysis);

            foreach (Analysis analysis in keys)
            {
                m_displayCache[analysis] = null;
                DisplayScansVsNet(analysis);
            }
        }

        private void mcheckBox_useResiduals_CheckedChanged(object sender, EventArgs e)
        {
            InvalidateDisplayCache();
        }

        private void mbutton_process_Click(object sender, EventArgs e)
        {
            OnStatus("...Creating mass tag database.");
            m_selectedAnalysis              = null;
            radioButtonAverageNET.Enabled   = false;
            mbutton_saveDatabase.Visible    = false;

            mbutton_saveDatabase.BackColor  = Color.Red;            
            mbutton_saveDatabase.FlatAppearance.BorderColor = Color.Red;
            mbutton_saveDatabase.ForeColor = Color.White;

            ctlChartScanVsNET.SeriesCollection.Clear();

            radioButtonPredictedNET.Checked = true;
            List<Analysis> allAnalysis = m_manager.GetAnalysis();
            m_processor.CreateDatabase(m_options, allAnalysis);
        }

        private void radioButtonAverageNET_CheckedChanged(object sender, EventArgs e)
        {
            ctlChartScanVsNET.YAxisLabel = "Average NET";
            InvalidateDisplayCache();
        }

        private void radioButtonPredictedNET_CheckedChanged(object sender, EventArgs e)
        {
            ctlChartScanVsNET.YAxisLabel = "Predicted NET";
            InvalidateDisplayCache();
        }

        private void LockUserInterface(bool state)
        {
            mbutton_options.Enabled         = state;
            listViewDatasets.Enabled        = state;
            mbutton_saveDatabase.Enabled    = state;
            mbutton_process.Enabled         = state;
            mbutton_showDatabase.Enabled    = state;
            radioButtonAverageNET.Enabled   = state;
            radioButtonPredictedNET.Enabled = state;
        }


        /// <summary>
        /// Displays the message on the screen.
        /// </summary>
        /// <param name="message"></param>
        private void UpdateStatusMessage(string message)
        {
            statusHover.Text = message;
        }

        private void mcheckBox_displayRegression_CheckedChanged(object sender, EventArgs e)
        {
            ctlChartScanVsNET.Refresh();
        }
    }
    
    /// <summary>
    /// Status event arguments
    /// </summary>
    public class StatusEventArgs: EventArgs
    {
        public StatusEventArgs(string message)
        {
            Message = message;
        }
        public string Message { get; private set; }
    }
}
