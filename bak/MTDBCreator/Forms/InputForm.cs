using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MTDBCreator.Data;
using MTDBCreator.IO;
using System.IO;

namespace MTDBCreator.Forms
{
    public partial class InputForm : Form
    {

        private Dictionary<string, Analysis> m_datasetHash;
        private OpenFileDialog m_openAnalysisJobsDialog;

        public InputForm()
        {
            InitializeComponent();
            m_openAnalysisJobsDialog             = new OpenFileDialog();
            m_datasetHash                        = new Dictionary<string, Data.Analysis>();
            m_openAnalysisJobsDialog.Multiselect = true;
            Analysis                             = new List<Analysis>();
        }

        private void InputForm_Load(object sender, EventArgs e)
        {

        }

        private void mbutton_addPhrp_Click(object sender, EventArgs e)
        {
            m_openAnalysisJobsDialog.Filter = "Dataset Definition Files (*.txt)|*.txt";
            ReadAnalysisData(new AnalysisMetaDataReader() as IAnalysisMetaDataReader);
        }

        private void mbutton_addSequest_Click(object sender, EventArgs e)
        {            
            m_openAnalysisJobsDialog.Filter = "SEQUEST PHRP Analyzed Files (*_syn.txt)|*_syn.txt";
            ReadAnalysisData(new SequestMetaDataReader() as IAnalysisMetaDataReader);
        }

        private void mbutton_addXtandem_Click(object sender, EventArgs e)
        {
            m_openAnalysisJobsDialog.Filter = "X!Tandem PHRP Analyzed Files (*_xt.txt)|*_xt.txt";
            ReadAnalysisData(new XTandemMetaDataReader() as IAnalysisMetaDataReader);
        }

        private void mbutton_msgfPlus_Click(object sender, EventArgs e)
        {
            m_openAnalysisJobsDialog.Filter = "MSGF+ PHRP Analyzed Files (*msgfdb_syn.txt)|*msgfdb_syn.txt";
            ReadAnalysisData(new MsgfPlusMetaDataReader() as IAnalysisMetaDataReader);
        }

        private void ReadAnalysisData(IAnalysisMetaDataReader reader)
        {
            try
            {

                m_openAnalysisJobsDialog.FilterIndex = 1;
                m_openAnalysisJobsDialog.RestoreDirectory = true;

                if (m_openAnalysisJobsDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string path in m_openAnalysisJobsDialog.FileNames)
                    {                        
                        List<Analysis> analysis = reader.ReadMetaData(path);
                        this.Analysis.AddRange(analysis);
                        AddAnalysis(analysis);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Adds unique analysis jobs to the analysis list.
        /// </summary>
        /// <param name="data"></param>
        private void AddAnalysis(List<Analysis> data)
        {
            
            foreach (Analysis datum in data)
            {
                string key = Path.Combine(datum.FilePath, datum.Name);
                if (m_datasetHash.ContainsKey(key))
                    continue;

                m_datasetHash.Add(key, datum);
                ListViewItem item = new ListViewItem(datum.Name);
                item.SubItems.Add(datum.FilePath);
                listViewDatasets.Items.Add(item);
            }
        }
        /// <summary>
        /// Gets or sets the list of analysis to use.
        /// </summary>
        public List<Analysis> Analysis { get; set; }

    }
}
