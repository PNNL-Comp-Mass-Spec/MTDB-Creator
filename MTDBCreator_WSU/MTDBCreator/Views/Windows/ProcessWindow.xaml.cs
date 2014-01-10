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

            this.Owner = ownerWindow;

            this.MainBackgroundWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            backgroundWorkHelper.HostProcessWindow = this;

            this.MainBackgroundWorkHelper = backgroundWorkHelper;

            this.MainBackgroundWorker.DoWork += backgroundWorkHelper.BackgroundWorker_DoWork;
            this.MainBackgroundWorker.ProgressChanged += backgroundWorkHelper.BackgroundWorker_ProgressChanged;
            this.MainBackgroundWorker.RunWorkerCompleted += backgroundWorkHelper.BackgroundWorker_RunWorkerCompleted;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MainBackgroundWorker.RunWorkerAsync();
        }

        public string Status
        {
            get
            {
                return this.StatusTextBlock.Text;
            }
            set
            {
                this.StatusTextBlock.Text = value;
            }
        }

        public ProgressBar MainProgressBar
        {
            get
            {
                return this.StatusProgressBar;
            }
        }

        public BackgroundWorker MainBackgroundWorker { get; private set; }
        public IBackgroundWorkHelper MainBackgroundWorkHelper { get; set; }
    }
}
