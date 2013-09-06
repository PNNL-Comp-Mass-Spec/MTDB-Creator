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
using MTDBCreator.IO;
using MTDBCreator.Algorithms;
using MTDBCreator.Data;
using MTDBCreator.Forms;
using System.IO; 

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for frmMain.
	/// </summary>
	public partial class frmMain : System.Windows.Forms.Form
    {
        #region Members
        public const string PROGRAM_DATE                 = "August 28, 2013";
        private OpenFileDialog  m_openAnalysisJobsDialog;
        #endregion

        #region Constructors and Initialization
       public frmMain()
		{
			InitializeComponent();

            BackColor = Color.White;

            // Create a dialog for locating and opening analysis job text files.
            m_openAnalysisJobsDialog = new OpenFileDialog();                                   
		}   
        #endregion
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                InputForm inputForm = new InputForm();
                inputForm.Icon = Icon;
                inputForm.StartPosition = FormStartPosition.CenterParent;
                if (inputForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    AnalysisControl analysisView = new AnalysisControl(); 
                    List<Analysis> analyses      = inputForm.Analysis;
                    analysisView.Dock            = DockStyle.Fill;

                    // Add the analysis to the view
                    analysisView.AddAnalysis(analyses);

                    // Add to the UI list
                    TabPage page    = new TabPage();
                    page.BackColor  = Color.White;
                    page.Text       = "Database-" + mtabs_analysis.TabCount.ToString();
                    page.Controls.Add(analysisView);
                    mtabs_analysis.TabPages.Add(page);
                    mtabs_analysis.Dock = DockStyle.Fill;
                    mtabs_analysis.BringToFront();
                    mtabs_analysis.Visible = true;
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void m_aboutLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAbout about      = new frmAbout();
            about.Icon          = Icon;
            about.StartPosition = FormStartPosition.CenterParent;
            about.ShowDialog();
        }


	}
}
