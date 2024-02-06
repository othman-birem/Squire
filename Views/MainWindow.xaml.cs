using Squire.Views.Pages;
using System.Windows;
using System.Windows.Media.Animation;

namespace Squire.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   public partial class MainWindow : Window
   {
      private Prompt mainPage;
      private Settings? _settings;
      public MainWindow()
      {
         InitializeComponent();
         mainPage = new();
         WorkSpaceButton.IsChecked = true;
      }

      #region Window Tools
      private void MinimizeButtonClicked(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
      private void MaximizeButtonClicked(object sender, RoutedEventArgs e) 
      {
         if(WindowState == WindowState.Normal) WindowState = WindowState.Maximized;
         else WindowState = WindowState.Normal;
      }
      private void CloseButtonClicked(object sender, RoutedEventArgs e)
      {
         Application.Current.Shutdown();
         Close();
      }
      #endregion

      #region Navigation

      private void WorkSpaceButton_Clicked(object sender, RoutedEventArgs e) { GOTO_Workspace(); }
      private void SettingsButton_Clicked(object sender, RoutedEventArgs e) { GOTO_Settings(); }
      internal void GOTO_Workspace()
      {
         MainFrame.Content = mainPage;
      }
      internal void GOTO_Settings()
      {
         _settings ??= new();
         MainFrame.Content = _settings;
         GridLength dn = new GridLength(1, GridUnitType.Pixel);
      }
      #endregion
   }
}