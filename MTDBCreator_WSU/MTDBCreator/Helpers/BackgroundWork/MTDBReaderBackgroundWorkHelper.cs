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
    class MTDBReaderBackgroundWorkHelper : IBackgroundWorkHelper
    {
        private TargetDatabase m_Database { get; set; }
        private Options m_DatabaseOptions { get; set; }
        private string m_DatabaseFileName { get; set; }

        public MTDBReaderBackgroundWorkHelper(AnalysisJobViewModel analysisJobViewModel, string fileName)
        {
            this.m_Database = analysisJobViewModel.Database;
            this.m_DatabaseOptions = analysisJobViewModel.Options;
            this.m_DatabaseFileName = fileName;
        }

        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SQLiteTargetDatabaseReader targetDatabaseReader = new SQLiteTargetDatabaseReader();

            m_Database = targetDatabaseReader.Read(this.m_DatabaseFileName);
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
