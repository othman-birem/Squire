using Squire.Helpers;
using Squire.UI.Controls;
using System.Diagnostics;
using System.IO;
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
      internal Discussion CurrentDiscussion = new();

      public Prompt()
      {
         InitializeComponent();
         PromptBox.Focus();
      }

      private void run_cmd(string args)
      {
         ProcessStartInfo start = new ProcessStartInfo
         {
            FileName = "C:\\Users\\othman\\OneDrive\\Desktop\\python codes\\dist\\testing.exe",
            Arguments = args,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardInput = true,
            CreateNoWindow = true
         };

         using Process? process = Process.Start(start);
         using StreamWriter writer = process.StandardInput;
         writer.WriteLine(start.Arguments);
         using StreamReader reader = process.StandardOutput;
         string result = reader.ReadToEnd();
         AppendResponse(result);
      }

      #region UI Functions
      private void SubmissionButton_Click(object sender, RoutedEventArgs e)
      {
         string QueryBody = PromptBox.Text;
         AppendMessage(QueryBody);
         PromptBox.Clear();

         Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Input, () => run_cmd(QueryBody));

      }
      internal void AppendResponse(string message)
      {
         QueryPanel msg = new(message, QueryPanel.Peers.SQuire);
         InteractionPanel.Children.Add(msg);
      }
      internal void AppendMessage(string message)
      {
         QueryPanel msg = new(message, QueryPanel.Peers.You);
         InteractionPanel.Children.Add(msg);
         if(CurrentDiscussion == null) { CurrentDiscussion = new Discussion(msg); }
         else CurrentDiscussion.AddMessage(msg);
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
         CurrentDiscussion.Save();
         CurrentDiscussion = new Discussion();
         PromptBox.Text = string.Empty;
         InteractionPanel.Children.Clear();
      }
      #endregion
   }
}
