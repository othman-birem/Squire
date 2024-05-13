using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Squire.UI.Controls
{
    /// <summary>
    /// Interaction logic for UserQuery.xaml
    /// </summary>
   public partial class QueryPanel : DockPanel
   {
      public static readonly DependencyProperty PictureProperty = DependencyProperty.Register("Picture", typeof(ImageSource), typeof(QueryPanel), new PropertyMetadata(null));
      public ImageSource Picture
      {
         get { return (ImageSource)GetValue(PictureProperty); }
         set { SetValue(PictureProperty, value); }
      }
      public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(QueryPanel), new PropertyMetadata(""));
      public string Text
      {
         get { return (string)GetValue(TextProperty); }
         set { SetValue(TextProperty, value); }
      }
      public static readonly DependencyProperty PeerProperty = DependencyProperty.Register("Peer", typeof(Peers), typeof(QueryPanel), new PropertyMetadata(Peers.You));
      public Peers Peer
      {
          get { return (Peers)GetValue(PeerProperty); }
          set { SetValue(PeerProperty, value); }
      }

      public enum Peers { SQuire, You }
      private static readonly string[] Pictures = new string[2]
      {
         "/Assets/ManUser.png",
         "/Assets/Logo/CircledWhite_Logo.png"
      };

      public QueryPanel()
      {
         InitializeComponent();
      }
      internal QueryPanel(string text, Peers peer)
      {
         InitializeComponent();
         PeerLine.Text = peer.ToString();
         if (peer == Peers.You)
         {
            Picture = new BitmapImage(new Uri(Pictures[0], UriKind.Relative));
            Text = text;
            // Loaded += (o, e) => StackPanel_Loaded(o, e);
         }
         else
         {
            Picture = new BitmapImage(new Uri(Pictures[1], UriKind.Relative));
            TypeMessage(text);
         }
      }
      private async void TypeMessage(string text)
      {
         foreach (char c in text)
         {
            Text += c;
            await Task.Delay(70);
         }
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
