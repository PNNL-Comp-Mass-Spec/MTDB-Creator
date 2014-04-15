#region Namespaces

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MTDBCreator.ViewModels;

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
        // Default static constructor - DO NOT USE!
        // Otherwise, the display of the new tab page will be abnormal

        //static MTDBCreatorTabPage()
        //{
        //    DefaultStyleKeyProperty.OverrideMetadata(typeof(MTDBCreatorTabPage), new FrameworkPropertyMetadata(typeof(MTDBCreatorTabPage)));
        //}

        public MTDBCreatorTabPage(string pageTitle, Image pageImage, UserControl contentControl, RoutedEventHandler closeHandler)
            : base()
        {
            this.ContentUserControl = contentControl;

            // TabPage's Header

            StackPanel tabPageHeaderStackPanel = new StackPanel() { Orientation = Orientation.Horizontal };

            TextBlock tabPageHeaderTextBlock = new TextBlock()
            {
                Margin = new Thickness(0, 0, 5, 0),
                VerticalAlignment = VerticalAlignment.Center,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Text = pageTitle
            };

            // Width of the textblock is set to auto at present

            Button tabPageCloseButton = new Button()
            {
                Content = "X",
                Tag = this,
                Style = (Style)this.FindResource(ToolBar.ButtonStyleKey)
            };

            tabPageCloseButton.Click += closeHandler;

            tabPageHeaderStackPanel.Children.Add(pageImage);
            tabPageHeaderStackPanel.Children.Add(tabPageHeaderTextBlock);
            tabPageHeaderStackPanel.Children.Add(tabPageCloseButton);

            this.Header = tabPageHeaderStackPanel;
            this.Content = contentControl;
        }

        public UserControl ContentUserControl { get; private set; }
    }
}
