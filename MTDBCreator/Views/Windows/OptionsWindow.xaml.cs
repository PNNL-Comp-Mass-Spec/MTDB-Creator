using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using MTDBCreator.ViewModels;
using MTDBFramework.Data;
using MTDBFrameworkBase.Data;

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
            if (m_ViewModel == null)
            {
                m_ViewModel = new OptionsViewModel();
                m_ViewModel.Options = options;
            }
            DataContext = m_ViewModel;
        }

        private void Control_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9.-]+");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
