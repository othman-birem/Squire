using Squire.UI.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Squire.Views.Pages
{
   /// <summary>
   /// Interaction logic for MainPage.xaml
   /// </summary>
   public partial class Prompt : Page
   {
      public Prompt()
      {
         InitializeComponent();
         PromptBox.Focus();
      }

      #region UI Functions
      private void SubmissionButton_Click(object sender, System.Windows.RoutedEventArgs e)
      {
         AppendMessage(PromptBox.Text);
         PromptBox.Clear();
      }
      internal void AppendMessage(string message)
      {
         QueryPanel msg = new(message, QueryPanel.Peers.You);
         InteractionPanel.Children.Add(msg);
      }
      private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
      {
         if(string.IsNullOrEmpty(PromptBox.Text)) SubmissionButton.IsEnabled = false;
         else SubmissionButton.IsEnabled = true;
      }
      private void PromptBox_KeyDown(object sender, KeyEventArgs e)
      {
         if(e.Key == Key.Enter) SubmissionButton_Click((object)sender, e);
      }
      private void NewPromptButton_Clicked(object sender, RoutedEventArgs e)
      {
         ReinitializePrompt();
      }
      private void ReinitializePrompt()
      {
         PromptBox.Text = string.Empty;
         InteractionPanel.Children.Clear();
      }
      #endregion
   }
}
