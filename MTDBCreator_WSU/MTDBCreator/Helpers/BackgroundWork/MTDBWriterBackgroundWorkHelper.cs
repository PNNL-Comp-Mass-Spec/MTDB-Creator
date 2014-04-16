using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using MTDBCreator.ViewModels;
using MTDBCreator.Windows;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.UI;

namespace MTDBCreator.Helpers.BackgroundWork
{
    public class MTDBWriterBackgroundWorkHelper : IBackgroundWorkHelper
    {
        public MTDBWriterBackgroundWorkHelper(AnalysisJobViewModel analysisJobViewModel, string fileName)
        {
            this.m_Database = analysisJobViewModel.Database;
            this.m_DatabaseOptions = analysisJobViewModel.Options;
            this.m_DatabaseFileName = fileName;
        }

        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SqLiteTargetDatabaseWriter targetDatabaseWriter = new SqLiteTargetDatabaseWriter();

            targetDatabaseWriter.ProgressChanged += targetDatabaseWriter_ProgressChanged;
            targetDatabaseWriter.Write(this.m_Database, this.m_DatabaseOptions, this.m_DatabaseFileName);
        }

        public void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.HostProcessWindow.MainProgressBar.IsIndeterminate = (e.ProgressPercentage == -1);

            if (e.ProgressPercentage >= 0)
            {
                object[] userStates = e.UserState as object[];

                if (userStates != null)
                {
                    int total = Convert.ToInt32(userStates[0].ToString());
                    string status = userStates[1].ToString();

                    this.HostProcessWindow.MainProgressBar.Value = e.ProgressPercentage;
                    this.HostProcessWindow.MainProgressBar.Maximum = total;
                    this.HostProcessWindow.Status = status;
                }
            }
            else
            {
                this.HostProcessWindow.Status = e.UserState.ToString();
            }
        }

        public void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.HostProcessWindow.Close();

            if (e.Result == null)
            {
                MessageBox.Show(String.Format("The MTDB has been created successfully.{0}{0}Path:{0}{0}{1}", Environment.NewLine, this.m_DatabaseFileName), Application.Current.MainWindow.Tag.ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public object Argument
        {
            get
            {
                return new object[] { this.m_Database, this.m_DatabaseFileName };
            }
        }
        public object Result
        {
            get { throw new NotSupportedException("No Result in the current BackgroundWorkHelper"); }
        }

        public ProcessWindow HostProcessWindow { get; set; }

        private void targetDatabaseWriter_ProgressChanged(object sender, MtdbProgressChangedEventArgs e)
        {
            MtdbCreationProgressType type = (MtdbCreationProgressType)Enum.Parse(typeof(MtdbCreationProgressType), e.UserObject.ToString());

            switch (type)
            {
                case MtdbCreationProgressType.CONSENSUS_TARGET:
                    {
                        this.HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, new object[] { e.Total.ToString(), String.Format("Processing Analysis Evidences...{0}%", WindowHelper.GetPercentage(e.Current, e.Total).ToString()) });

                        break;
                    }
                case MtdbCreationProgressType.SEQUENCE:
                    {
                        this.HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, new object[] { e.Total.ToString(), String.Format("Processing Sequence Information...{0}%", WindowHelper.GetPercentage(e.Current, e.Total).ToString()) });

                        break;
                    }
                case MtdbCreationProgressType.PEPTIDE:
                    {
                        this.HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, new object[] { e.Total.ToString(), String.Format("Processing Peptide Information...{0}%", WindowHelper.GetPercentage(e.Current, e.Total).ToString().ToString()) });

                        break;
                    }
                case MtdbCreationProgressType.ANALYSIS_SOURCE:
                    {
                        this.HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, new object[] { e.Total.ToString(), String.Format("Processing Analysis Source Information...{0}%", WindowHelper.GetPercentage(e.Current, e.Total).ToString()) });

                        break;
                    }
                case MtdbCreationProgressType.COMMIT:
                    {
                        this.HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, String.Concat("Writing to MTDB: ", this.m_DatabaseFileName));

                        break;
                    }
            }
        }

        private TargetDatabase m_Database { get; set; }
        private Options m_DatabaseOptions { get; set; }
        private string m_DatabaseFileName { get; set; }
    }
}
