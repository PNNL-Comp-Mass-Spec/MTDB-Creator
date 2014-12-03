using System;
using MTDBCreator.ViewModels;

namespace MTDBCreator.Views.Windows
{
    /// <summary>
    /// Interaction logic for AmtListView.xaml
    /// </summary>
    public partial class AmtListView
    {

        public AmtPeptideOptionsViewModel QualityStats { get; set; }

        public Action StatAction { get; set; }

        public AmtListView()
        {
            InitializeComponent();
        }
    }
}
