#region Namespaces

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MTDBCreator.ViewModels;
using MTDBCreator.Windows;
using MTDBFramework.Algorithms;
using MTDBFramework.Database;

#endregion

namespace MTDBCreator.Helpers.BackgroundWork
{
    public sealed class MtdbProcessorBackgroundWorkHelper : IBackgroundWorkHelper
    {
        private readonly AnalysisJobViewModel m_AnalysisJobViewModel;

        public MtdbProcessorBackgroundWorkHelper(AnalysisJobViewModel analysisJobViewModel)
        {
            m_AnalysisJobViewModel = analysisJobViewModel;
        }

        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var mtdbProcessor = new MtdbProcessor(m_AnalysisJobViewModel.Options);
            mtdbProcessor.AlignmentComplete +=  mtdbProcessor_AlignmentComplete;
            HostProcessWindow.MainBackgroundWorker.ReportProgress(0);

            try
            {

                e.Result = mtdbProcessor.Process(m_AnalysisJobViewModel.AnalysisJobItems.Select(job => job.DataSet).ToList());
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void mtdbProcessor_AlignmentComplete(object sender, AlignmentCompleteArgs e)
        {            
            //TODO: Mike cleanup....

            //int alignmentNum = 1;
            //foreach (var alignment in alignmentData)
            //{

            //    string filePath = string.Format("C:\\alignmentResults\\results{0}.csv", alignmentNum);

            //    using (var write = new StreamWriter(filePath))
            //    {
            //        write.WriteLine("Linear Net Rsquared, Alignment Time Scan, Alignment Scan Output, Alignment Net Output");
            //        write.WriteLine(string.Format("{0}, {1}, {2}, {3}", alignment.NetRsquared,
            //            alignment.AlignmentFunction.NetFuncTimeInput[0],
            //            alignment.AlignmentFunction.NetFuncTimeOutput[0],
            //            alignment.AlignmentFunction.NetFuncNetOutput[0]));
            //        for (int line = 1; line < alignment.AlignmentFunction.NetFuncTimeInput.Count; line++)
            //        {
            //            write.WriteLine(string.Format(", {0}, {1}, {2}",
            //             alignment.AlignmentFunction.NetFuncTimeInput[line],
            //             alignment.AlignmentFunction.NetFuncTimeOutput[line],
            //             alignment.AlignmentFunction.NetFuncNetOutput[line]));
            //        }
            //    }
            //}
        }

        public void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            HostProcessWindow.Status = "Processing Data...";
        }

        public void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is Exception)
            {
                var ex = e.Result as Exception;

                ErrorHelper.WriteExceptionTraceInformation(ex);

                MessageBox.Show(String.Format("The following exception has occurred:{0}{0}{1}", Environment.NewLine, ex.Message), Application.Current.MainWindow.Tag.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                m_AnalysisJobViewModel.Database = e.Result as TargetDatabase;
            }

            HostProcessWindow.DialogResult = !(e.Result is Exception);
            HostProcessWindow.Close();
        }

        public object Result
        {
            get { return m_AnalysisJobViewModel.Database; }
        }

        public ProcessWindow HostProcessWindow { get; set; }        
    }
}