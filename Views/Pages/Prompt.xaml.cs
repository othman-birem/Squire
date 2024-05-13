using Squire.Helpers;
using Squire.UI.Controls;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Squire.Views.Pages
{
   /// <summary>
   /// Interaction logic for MainPage.xaml
   /// </summary>
   public partial class Prompt : Page
   {
      internal Discussion? CurrentDiscussion;
      internal static List<Discussion>? Discussions;

      public Prompt()
      {
         InitializeComponent();
         DisplayDiscussions();
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

      private void SetDiscussion(Discussion disc)
      {
         ReinitializePrompt();
         foreach(Message msg in disc.Messages)
         {
            if (msg.peer == QueryPanel.Peers.You) AppendMessage(msg.Text);
            else AppendResponse(msg.Text);
         }
      }
      private void Discussion_clicked(object sender, RoutedEventArgs e)
      {
         //string? title = (sender as Button).Content.ToString();
         Discussion? disc = Discussions.Find(a => a.Title.Equals((sender as Button).Content.ToString()));
         SetDiscussion(disc);
      }
      private void DisplayDiscussions()
      {
         //throw new NullReferenceException("Discussions list cannot be null.");
         Discussions ??= Discussion.GetDB();
         DateTime today = DateTime.Now;
         foreach (Discussion discussion in Discussions)
         {
            Button btn = new Button()
            {
               Height = 30,
               Padding = new Thickness(10, 0, 10, 0),
               Margin = new Thickness(0, 5, 0, 0),
               HorizontalAlignment = HorizontalAlignment.Stretch,
               HorizontalContentAlignment = HorizontalAlignment.Left,
               Style = NewDiscussionButton.Style,
               Content = discussion.Title,
               Foreground = new SolidColorBrush(Colors.White),
               FontSize = 14
            };
            btn.Click += (s, e) => Discussion_clicked(btn, new());
            if (discussion.Date > DateTime.Now.AddDays(-7))
            {
               if (DateTime.Now.AddDays(-1) > discussion.Date && discussion.Date >= DateTime.Now.AddDays(-7)) LastWeekPanel.Children.Add(btn);
               else if (discussion.Date.Day == today.Date.Day) TodayPanel.Children.Add(btn);
            }
            else LastMonthPanel.Children.Add(btn);
         }
      }
      internal void AppendResponse(string message)
      {
         QueryPanel msg = new(message, QueryPanel.Peers.SQuire);
         InteractionPanel.Children.Add(msg);
         if (CurrentDiscussion == null) { CurrentDiscussion = new Discussion(msg); }
         else CurrentDiscussion.AddMessage(msg);
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
         if(CurrentDiscussion != null && CurrentDiscussion.Messages.Count > 0)
         {
            Discussions = CurrentDiscussion.Save();
            CurrentDiscussion = new Discussion();
            PromptBox.Text = string.Empty;
            InteractionPanel.Children.Clear();
         }
      }
      private void InteractionPanel_SizeChanged(object sender, SizeChangedEventArgs e)
      {
         if (InteractionPanel.Children.Count == 0) GreetingPanel.Visibility = Visibility.Visible;
         else GreetingPanel.Visibility = Visibility.Collapsed;
      }
      #endregion
   }
}
