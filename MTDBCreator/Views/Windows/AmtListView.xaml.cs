using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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
