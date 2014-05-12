﻿using System;
using System.ComponentModel;
using System.Windows;
using MTDBCreator.ViewModels;
using MTDBCreator.Windows;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.IO;
using MTDBFramework.UI;

namespace MTDBCreator.Helpers.BackgroundWork
{
    public class MTDBWriterBackgroundWorkHelper : IBackgroundWorkHelper
    {
        public MTDBWriterBackgroundWorkHelper(AnalysisJobViewModel analysisJobViewModel, string fileName)
        {
            m_Database = analysisJobViewModel.Database;
            m_DatabaseOptions = analysisJobViewModel.Options;
            m_DatabaseFileName = fileName;
        }

        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var targetDatabaseWriter = new SqLiteTargetDatabaseWriter();

            targetDatabaseWriter.ProgressChanged += targetDatabaseWriter_ProgressChanged;
            targetDatabaseWriter.Write(m_Database, m_DatabaseOptions, m_DatabaseFileName);
        }

        public void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            HostProcessWindow.MainProgressBar.IsIndeterminate = (e.ProgressPercentage == -1);

            if (e.ProgressPercentage >= 0)
            {
                var userStates = e.UserState as object[];

                if (userStates != null)
                {
                    var total = Convert.ToInt32(userStates[0].ToString());
                    var status = userStates[1].ToString();

                    HostProcessWindow.MainProgressBar.Value = e.ProgressPercentage;
                    HostProcessWindow.MainProgressBar.Maximum = total;
                    HostProcessWindow.Status = status;
                }
            }
            else
            {
                HostProcessWindow.Status = e.UserState.ToString();
            }
        }

        public void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            HostProcessWindow.Close();

            if (e.Result == null)
            {
                MessageBox.Show(String.Format("The MTDB has been created successfully.{0}{0}Path:{0}{0}{1}", Environment.NewLine, m_DatabaseFileName), Application.Current.MainWindow.Tag.ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public object Argument
        {
            get
            {
                return new object[] { m_Database, m_DatabaseFileName };
            }
        }
        public object Result
        {
            get { throw new NotSupportedException("No Result in the current BackgroundWorkHelper"); }
        }

        public ProcessWindow HostProcessWindow { get; set; }

        private void targetDatabaseWriter_ProgressChanged(object sender, MtdbProgressChangedEventArgs e)
        {
            var type = (MtdbCreationProgressType)Enum.Parse(typeof(MtdbCreationProgressType), e.UserObject.ToString());

            switch (type)
            {
                case MtdbCreationProgressType.CONSENSUS_TARGET:
                    {
                        HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, new object[] { e.Total.ToString(), String.Format("Processing Analysis Evidences...{0}%", WindowHelper.GetPercentage(e.Current, e.Total)) });

                        break;
                    }
                case MtdbCreationProgressType.SEQUENCE:
                    {
                        HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, new object[] { e.Total.ToString(), String.Format("Processing Sequence Information...{0}%", WindowHelper.GetPercentage(e.Current, e.Total)) });

                        break;
                    }
                case MtdbCreationProgressType.PEPTIDE:
                    {
                        HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, new object[] { e.Total.ToString(), String.Format("Processing Peptide Information...{0}%", WindowHelper.GetPercentage(e.Current, e.Total).ToString()) });

                        break;
                    }
                case MtdbCreationProgressType.ANALYSIS_SOURCE:
                    {
                        HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, new object[] { e.Total.ToString(), String.Format("Processing Analysis Source Information...{0}%", WindowHelper.GetPercentage(e.Current, e.Total)) });

                        break;
                    }
                case MtdbCreationProgressType.COMMIT:
                    {
                        HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, String.Concat("Writing to MTDB: ", m_DatabaseFileName));

                        break;
                    }
            }
        }

        private TargetDatabase m_Database { get; set; }
        private Options m_DatabaseOptions { get; set; }
        private string m_DatabaseFileName { get; set; }
    }
}