using System;
using System.ComponentModel;
using MTDBCreator.ViewModels;
using MTDBCreator.Windows;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.IO;

namespace MTDBCreator.Helpers.BackgroundWork
{
    class MtdbReaderBackgroundWorkHelper : IBackgroundWorkHelper
    {
        private TargetDatabase m_Database { get; set; }
        private Options m_DatabaseOptions { get; set; }
        private string m_DatabaseFileName { get; set; }

        public MtdbReaderBackgroundWorkHelper(AnalysisJobViewModel analysisJobViewModel, string fileName)
        {
            m_Database = analysisJobViewModel.Database;
            m_DatabaseOptions = analysisJobViewModel.Options;
            m_DatabaseFileName = fileName;
        }

        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var targetDatabaseReader = new SqLiteTargetDatabaseReader();

            m_Database = targetDatabaseReader.Read(m_DatabaseFileName);
        }

        public void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public object Argument
        {
            get { throw new NotImplementedException(); }
        }

        public object Result
        {
            get { throw new NotImplementedException(); }
        }

        public ProcessWindow HostProcessWindow
        { get; set; }
    }
}
