﻿using System;
using System.ComponentModel;
using MTDBCreator.ViewModels;
using MTDBCreator.Windows;
using MTDBFrameworkBase.Data;
using MTDBFrameworkBase.Database;
using MTDBFrameworkBase.IO;

namespace MTDBCreator.Helpers.BackgroundWork
{
    class MtdbReaderBackgroundWorkHelper : IBackgroundWorkHelper
    {
        private TargetDatabase Database { get; }
        private Options DatabaseOptions { get; }
        private string DatabaseFileName { get; }

        //private bool mAbortRequested;

        public MtdbReaderBackgroundWorkHelper(AnalysisJobViewModel analysisJobViewModel, string fileName)
        {
            Database = analysisJobViewModel.Database;
            DatabaseOptions = analysisJobViewModel.Options;
            DatabaseFileName = fileName;
        }

        public void BackgroundWorker_AbortProcessing()
        {
            //mAbortRequested = true;
        }

        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //mAbortRequested = false;

            var targetDatabaseReader = new SqLiteTargetDatabaseReader();

            //Database = targetDatabaseReader.Read(DatabaseFileName);
        }

        public void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public object Argument => throw new NotImplementedException();

        public object Result => throw new NotImplementedException();

        public ProcessWindow HostProcessWindow
        { get; set; }
    }
}
