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
        public ProcessWindow(IBackgroundWorkHelper backgroundWorkHelper, Window ownerWindow)
        {
            InitializeComponent();

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
            MainBackgroundWorker.RunWorkerAsync();
        }

        public string Status
        {
            get
            {
                return StatusTextBlock.Text;
            }
            set
            {
                StatusTextBlock.Text = value;
            }
        }

        public ProgressBar MainProgressBar
        {
            get
            {
                return StatusProgressBar;
            }
        }

        public BackgroundWorker MainBackgroundWorker { get; private set; }
        public IBackgroundWorkHelper MainBackgroundWorkHelper { get; set; }
    }
}
