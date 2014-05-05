using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MTDBCreator.ViewModels;
using MTDBFramework.Data;

namespace MTDBCreator.Views
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        #region Private Fields

        private OptionsViewModel m_ViewModel;

        #endregion

        public OptionsWindow(Options options)
        {
            InitializeComponent();
            m_ViewModel = new OptionsViewModel(options);

            this.DataContext = m_ViewModel;
        }

        private void Control_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9.-]+");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
