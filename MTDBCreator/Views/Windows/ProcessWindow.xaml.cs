#region Namespaces

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using MTDBCreator.Helpers.BackgroundWork;

#endregion

namespace MTDBCreator.Windows
{
    /// <summary>
    /// Interaction logic for ProcessWindow.xaml
    /// </summary>
    public partial class ProcessWindow : Window
    {
        public bool MultithreadingEnabled { get; private set; }

        public object MostRecentResult { get; private set; }

        public ProcessWindow(IBackgroundWorkHelper backgroundWorkHelper, Window ownerWindow)
        {
            InitializeComponent();

            MultithreadingEnabled = true;

            Owner = ownerWindow;

            MainBackgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            backgroundWorkHelper.HostProcessWindow = this;

                MainBackgroundWorkHelper = backgroundWorkHelper;

                MainBackgroundWorker.DoWork += backgroundWorkHelper.BackgroundWorker_DoWork;
                MainBackgroundWorker.ProgressChanged += backgroundWorkHelper.BackgroundWorker_ProgressChanged;
                MainBackgroundWorker.RunWorkerCompleted += backgroundWorkHelper.BackgroundWorker_RunWorkerCompleted;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (MultithreadingEnabled)
                MainBackgroundWorker.RunWorkerAsync();

        }

        private void CancelProcessing_Click(object sender, RoutedEventArgs e)
        {
            MainBackgroundWorker.CancelAsync();
            MainBackgroundWorkHelper.BackgroundWorker_AbortProcessing();
            StatusTextBlock.Text = "Cancelling Processing of data";
            IsEnabled = false;
        }

        public string Status
        {
            get
            {
                return StatusTextBlock.Text;
            }
            set
            {
                if (!MainBackgroundWorker.CancellationPending)
                {
                    StatusTextBlock.Text = value;
                }
            }
        }

        public ProgressBar MainProgressBar
        {
            get
            {
                return StatusProgressBar;
            }
        }

        public void StartProcessingNonThreaded(IBackgroundWorkHelper backgroundWorkHelper)
        {
            if (MultithreadingEnabled)
            {
                return;
            }

            var e = new DoWorkEventArgs(backgroundWorkHelper);
            backgroundWorkHelper.BackgroundWorker_DoWork(this, e);

            MostRecentResult = e.Result;

            // MainBackgroundWorkHelper.BackgroundWorker_DoWork(this, e);
            // backgroundWorkHelper = MainBackgroundWorkHelper;

        }

        public BackgroundWorker MainBackgroundWorker { get; private set; }
        public IBackgroundWorkHelper MainBackgroundWorkHelper { get; set; }

    }
}
