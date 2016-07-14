using System.Windows;
using MTDBCreator.ViewModels;

namespace MTDBCreator.Views.Windows
{
    /// <summary>
    /// Interaction logic for DmsExporterWindow.xaml
    /// </summary>
    public partial class DmsExporterWindow : Window
    {
        public DmsExporterWindow()
        {
            InitializeComponent();
        }

        public DmsExporterWindow(WorkspaceViewModel workspaceViewModel)
            : this()
        {
            DataContext = new DmsExporterViewModel();
        }
    }
}
