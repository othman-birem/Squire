using System.Windows.Media.Animation;
using System.Windows;

namespace Squire.UI.UX
{
   internal class GridLengthAnimation : AnimationTimeline
   {
      public override Type TargetPropertyType => typeof(GridLength);

      protected override Freezable CreateInstanceCore()
      {
         return new GridLengthAnimation();
      }

      public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(GridLength),
            typeof(GridLengthAnimation));
      public GridLength From
      {
         get { return (GridLength)GetValue(FromProperty); }
         set { SetValue(FromProperty, value); }
      }

      public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(GridLength),
            typeof(GridLengthAnimation));
      public GridLength To
      {
         get { return (GridLength)GetValue(ToProperty); }
         set { SetValue(ToProperty, value); }
      }

      public override object GetCurrentValue(object defaultOriginValue,
            object defaultDestinationValue, AnimationClock animationClock)
      {
         double fromVal = ((GridLength)GetValue(FromProperty)).Value;

         double toVal = ((GridLength)GetValue(ToProperty)).Value;


#pragma warning disable CS8629 // Nullable value type may be null.
         double progress = animationClock.CurrentProgress.Value;
#pragma warning restore CS8629 // Nullable value type may be null.

         IEasingFunction easingFunction = EasingFunction;
         if (easingFunction != null)
         {
            progress = easingFunction.Ease(progress);
         }


         if (fromVal > toVal)
         {
            return new GridLength((1 - progress) * (fromVal - toVal) + toVal, GridUnitType.Star);
         }

         return new GridLength(progress * (toVal - fromVal) + fromVal, GridUnitType.Star);
      }

      public IEasingFunction EasingFunction
      {
         get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
         set { SetValue(EasingFunctionProperty, value); }
      }
      public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register("EasingFunction", typeof(IEasingFunction),
            typeof(GridLengthAnimation), new UIPropertyMetadata(null));
   }
}
