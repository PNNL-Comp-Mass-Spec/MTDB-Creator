#region Namespaces

using System.Windows;
using System.Windows.Controls;

#endregion

namespace MTDBCreator.PageControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MTDBCreator"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MTDBCreator;assembly=MTDBCreator"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the evidence project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:MTDBCreatorTabPage/>
    ///
    /// </summary>
    public class MTDBCreatorTabPage : TabItem
    {
        public MTDBCreatorTabPage(string pageTitle, UIElement pageImage, UserControl contentControl, RoutedEventHandler closeHandler)
        {
            ContentUserControl = contentControl;

            // TabPage's Header

            var tabPageHeaderStackPanel = new StackPanel { Orientation = Orientation.Horizontal };

            var tabPageHeaderTextBlock = new TextBlock
            {
                Margin = new Thickness(0, 0, 5, 0),
                VerticalAlignment = VerticalAlignment.Center,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Text = pageTitle
            };

            // Width of the text block is set to auto at present

            var tabPageCloseButton = new Button
            {
                Content = "X",
                Tag = this,
                Style = (Style)FindResource(ToolBar.ButtonStyleKey)
            };

            tabPageCloseButton.Click += closeHandler;

            tabPageHeaderStackPanel.Children.Add(pageImage);
            tabPageHeaderStackPanel.Children.Add(tabPageHeaderTextBlock);
            tabPageHeaderStackPanel.Children.Add(tabPageCloseButton);

            Header = tabPageHeaderStackPanel;
            Content = contentControl;
        }

        public UserControl ContentUserControl { get; }
    }
}
