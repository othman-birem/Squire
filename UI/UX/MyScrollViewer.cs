using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;

namespace Squire.UI.UX
{
   internal class MyScrollViewer : ScrollViewer
   {
      private double _totalVerticalOffset;
      private bool _isScrolling;

      public static readonly DependencyProperty canMouseWheelprop = DependencyProperty.Register(
            nameof(canMouseWheel), typeof(bool), typeof(MyScrollViewer), new PropertyMetadata(true));

      public bool canMouseWheel
      {
         get { return (bool)GetValue(canMouseWheelprop); }
         set { SetValue(canMouseWheelprop, value); }
      }

      protected override void OnMouseWheel(MouseWheelEventArgs e)
      {
         base.OnMouseWheel(e);

         if (!canMouseWheel) { return; }
         e.Handled = true;
         if (!_isScrolling)
         {
            _totalVerticalOffset = VerticalOffset;
            currentVerticalOffset = VerticalOffset;
         }
         _totalVerticalOffset = Math.Min(Math.Max(0, _totalVerticalOffset - e.Delta * 2), ScrollableHeight);
         ScrollToVerticalOffsetWithAnimation(_totalVerticalOffset);
      }
      internal void scrollToTopInternal(double milliseconds = 300)
      {
         if (_isScrolling)
         {
            _totalVerticalOffset = VerticalOffset;
            currentVerticalOffset = VerticalOffset;
         }
         ScrollToVerticalOffsetWithAnimation(0, milliseconds);
      }

      internal static readonly DependencyProperty currentVerticalOffsetProp = DependencyProperty.Register(
            nameof(currentVerticalOffset), typeof(double), typeof(MyScrollViewer), new PropertyMetadata(0.0, OnCurrentVerticalOffsetChanged));
      internal double currentVerticalOffset
      {
         get => (double)GetValue(currentVerticalOffsetProp);
         set => SetValue(currentVerticalOffsetProp, value);
      }

      internal static readonly DependencyProperty currentHorizontalOffsetProp = DependencyProperty.Register(
            nameof(currentHorizontalOffset), typeof(double), typeof(MyScrollViewer), new PropertyMetadata(0.0, OnCurrentHorizontalOffsetChanged));

      internal double currentHorizontalOffset
      {
         get => (double)GetValue(currentHorizontalOffsetProp);
         set => SetValue(currentHorizontalOffsetProp, value);
      }
      public static readonly DependencyProperty IsPenetratingProperty = DependencyProperty.Register(
            "IsPenetrating", typeof(bool), typeof(MyScrollViewer), new PropertyMetadata(false));
      public bool IsPenetrating
      {
         get { return (bool)GetValue(IsPenetratingProperty); }
         set { SetValue(IsPenetratingProperty, value); }
      }

      protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters) =>
          IsPenetrating ? null : base.HitTestCore(hitTestParameters);

      public static readonly DependencyProperty IsInertiaEnabledProperty = DependencyProperty.Register(
            "IsInertiaEnabled", typeof(bool), typeof(MyScrollViewer), new PropertyMetadata(false));
      public bool IsInertiaEnabled
      {
         get => (bool)GetValue(IsInertiaEnabledProperty);
         set => SetValue(IsInertiaEnabledProperty, value);
      }

      public static void SetIsInertiaEnabled(DependencyObject element, bool value) => element.SetValue(IsInertiaEnabledProperty, value);
      public static bool GetIsInertiaEnabled(DependencyObject element) => (bool)element.GetValue(IsInertiaEnabledProperty);

      public static void SetIsPenetrating(DependencyObject element, bool value) => element.SetValue(IsInertiaEnabledProperty, value);
      public static bool GetIsPenetrating(DependencyObject element) => (bool)element.GetValue(IsPenetratingProperty);

      public static void OnCurrentVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         if (d is MyScrollViewer ctl && e.NewValue is double v)
         {
            ctl.ScrollToVerticalOffset(v);
         }
      }

      public static void OnCurrentHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         if (d is MyScrollViewer ctl && e.NewValue is double v)
         {
            ctl.ScrollToHorizontalOffset(v);
         }
      }
      public void ScrollToVerticalOffsetWithAnimation(double verticalOffset, double duration = 350)
      {
         DoubleAnimation animation = CreateAnimation(verticalOffset, duration);

         animation.EasingFunction = new CubicEase
         {
            EasingMode = EasingMode.EaseOut
         };
         animation.FillBehavior = FillBehavior.Stop;
         animation.Completed += (s, e1) =>
         {
            currentVerticalOffset = verticalOffset;
            _isScrolling = false;
         };
         _isScrolling = true;
         BeginAnimation(currentVerticalOffsetProp, animation, HandoffBehavior.SnapshotAndReplace);
      }

      public static DoubleAnimation CreateAnimation(double toValue, double milliseconds = 200)
      {
         return new DoubleAnimation(toValue, new Duration(TimeSpan.FromMilliseconds(milliseconds)))
         {
            EasingFunction = new CircleEase { EasingMode = EasingMode.EaseInOut }
         };
      }

      public void ScrollToHorizontalOffsetWithAnimation(double horizontalOffset, double duration = 300)
      {
         DoubleAnimation animation = CreateAnimation(horizontalOffset, duration);

         animation.EasingFunction = new CubicEase
         {
            EasingMode = EasingMode.EaseOut
         };
         animation.FillBehavior = FillBehavior.Stop;
         animation.Completed -= (s, e1) =>
         {
            currentVerticalOffset += horizontalOffset;
            _isScrolling = false;
         };
         _isScrolling = true;
         BeginAnimation(currentHorizontalOffsetProp, animation, HandoffBehavior.Compose);
      }

   }
}
