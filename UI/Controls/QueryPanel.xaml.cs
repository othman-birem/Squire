using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Squire.UI.Controls
{
    /// <summary>
    /// Interaction logic for UserQuery.xaml
    /// </summary>
   public partial class QueryPanel : DockPanel
   {
      public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(QueryPanel), new PropertyMetadata("NULL"));
      public string Text
      {
         get { return (string)GetValue(TextProperty); }
         set { SetValue(TextProperty, value); }
      }
      internal enum Peers { Squire, You }

      public QueryPanel()
      {
         InitializeComponent();
      }
      internal QueryPanel(string text, Peers peer)
      {
         InitializeComponent();
         PeerLine.Text = peer.ToString();
         Text = text;
      }

      private void StackPanel_Loaded(object sender, RoutedEventArgs e)
      {
         Storyboard storyboard = new();
         CircleEase ease = new(){ EasingMode = EasingMode.EaseOut };
         SolidColorBrush bkg = new(Colors.Transparent);
         Background = bkg;

         DoubleAnimation animation = new()
         {
            EasingFunction = ease,
            Duration = new(TimeSpan.FromMilliseconds(300)),
            From = 0,
            To = ActualHeight
         };
         ColorAnimation colorAnimation = new()
         {
            Duration = new(TimeSpan.FromMilliseconds(350)),
            From = Color.FromRgb(41, 41, 41),
            To = Colors.Transparent
         };

         Storyboard.SetTarget(colorAnimation, this);
         Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("(DockPanel.Background).(SolidColorBrush.Color)"));

         Storyboard.SetTarget(animation, this);
         Storyboard.SetTargetProperty(animation, new PropertyPath("(DockPanel.Height)"));

         storyboard.Children.Add(colorAnimation);
         storyboard.Children.Add(animation);

         storyboard.Begin();
      }
   }
}
