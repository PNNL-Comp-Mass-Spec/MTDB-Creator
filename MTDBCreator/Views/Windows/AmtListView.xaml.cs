using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MTDBCreator.ViewModels;

namespace MTDBCreator.Views.Windows
{
    /// <summary>
    /// Interaction logic for AmtListView.xaml
    /// </summary>
    public partial class AmtListView : UserControl
    {
        private AmtPeptideOptionsViewModel m_qualityStats;

        public AmtPeptideOptionsViewModel QualityStats { get; set; }

        public Action StatAction { get; set; }

        public AmtListView()
        {
            InitializeComponent();
        }
        private void MtdbList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = MtdbList.SelectedItem;
            if (item != null)
            {
            }
        }
    }
}
