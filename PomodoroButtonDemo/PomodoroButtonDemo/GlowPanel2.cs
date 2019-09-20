using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Shapes;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace PomodoroButtonDemo
{
    public class GlowPanel2 : ButtonDecorator
    {
        private readonly Compositor _compositor;
        private CompositionMaskBrush _maskBrush;

        private readonly CompositionLinearGradientBrush _foregroundBrush;
        private readonly CompositionLinearGradientBrush _backgroundBrush;
        private static readonly Color Blue = Color.FromArgb(255, 43, 210, 255);
        private static readonly Color Green = Color.FromArgb(255, 43, 255, 136);
        private static readonly Color Red = Colors.Red;
        //private static readonly Color Pink = Color.FromArgb(255, 255, 43, 212);
        private static readonly Color Pink = Color.FromArgb(255, 142, 211, 255);
        private static readonly Color Black = Colors.Black;

        private readonly CompositionColorGradientStop _topLeftradientStop;
        private readonly CompositionColorGradientStop _bottomRightGradientStop;

        private readonly CompositionColorGradientStop _bottomLeftGradientStop;
        private readonly CompositionColorGradientStop _topRightGradientStop;

        public GlowPanel2() : base()
        {
            this.DefaultStyleKey = typeof(GlowPanel2);
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _maskBrush = _compositor.CreateMaskBrush();
            Visual.Brush = _maskBrush;

            _foregroundBrush = _compositor.CreateLinearGradientBrush();
            _foregroundBrush.StartPoint = Vector2.Zero;
            _foregroundBrush.EndPoint = new Vector2(1.0f);

            _bottomRightGradientStop = _compositor.CreateColorGradientStop();
            _bottomRightGradientStop.Offset = 0.5f;
            _bottomRightGradientStop.Color = Green;
            _topLeftradientStop = _compositor.CreateColorGradientStop();
            _topLeftradientStop.Offset = 0.5f;
            _topLeftradientStop.Color = Blue;
            _foregroundBrush.ColorStops.Add(_bottomRightGradientStop);
            _foregroundBrush.ColorStops.Add(_topLeftradientStop);


            _backgroundBrush = _compositor.CreateLinearGradientBrush();
            _backgroundBrush.StartPoint = new Vector2(1.0f, 0);
            _backgroundBrush.EndPoint = new Vector2(0, 1.0f);

            _topRightGradientStop = _compositor.CreateColorGradientStop();
            _topRightGradientStop.Offset = 0.25f;
            _topRightGradientStop.Color = Black;
            _bottomLeftGradientStop = _compositor.CreateColorGradientStop();
            _bottomLeftGradientStop.Offset = 1.0f;
            _bottomLeftGradientStop.Color = Black;
            _backgroundBrush.ColorStops.Add(_topRightGradientStop);
            _backgroundBrush.ColorStops.Add(_bottomLeftGradientStop);


            var graphicsEffect = new BlendEffect()
            {
                Mode = BlendEffectMode.Screen,
                Foreground = new CompositionEffectSourceParameter("Main"),
                Background = new CompositionEffectSourceParameter("Tint"),
            };

            var effectFactory = _compositor.CreateEffectFactory(graphicsEffect);
            var brush = effectFactory.CreateBrush();

            brush.SetSourceParameter("Main", _foregroundBrush);
            brush.SetSourceParameter("Tint", _backgroundBrush);


        //    GaussianBlurEffect blurEffect = new GaussianBlurEffect()
        //    {
        //        Name = "Blur",
        //        BlurAmount = 1.0f,
        //        BorderMode = EffectBorderMode.Hard,
        //        Source = new CompositionEffectSourceParameter("source");
        //};

        //CompositionEffectFactory blurEffectFactory = _compositor.CreateEffectFactory(blurEffect);
        //CompositionEffectBrush _backdropBrush = blurEffectFactory.CreateBrush();

        //// Create a BackdropBrush and bind it to the EffectSourceParameter source

        //_backdropBrush.SetSourceParameter("source", _compositor.CreateBackdropBrush());

            _maskBrush.Source = brush;


            Loaded += async (s, e) =>
            {
                UpdateGradients();
                await Task.Delay(2000);

            };
        }

        protected override void UpdateOutlineMask()
        {
            if (RelativeElement is Shape shape)
            {
                var mask = shape.GetAlphaMask();
                _maskBrush.Mask = mask;
            }
            else if (RelativeElement is TextBlock textBlock)
            {
                var mask = textBlock.GetAlphaMask();
                _maskBrush.Mask = mask;
            }
        }

        //protected override void UpdateOutlineSize()
        //{
        //    base.UpdateOutlineSize();

        //    _foregroundBrush.CenterPoint = Visual.Size / 2;
        //    _backgroundBrush.CenterPoint = Visual.Size / 2;
        //}

        private void UpdateGradients()
        {
            if (IsInPomodoro)
            {
                StartOffsetAnimation(_bottomRightGradientStop, 0.6f);
                StartColorAnimation(_bottomRightGradientStop, Blue);

                StartOffsetAnimation(_topLeftradientStop, 0f);
                StartColorAnimation(_topLeftradientStop, Green);

                StartOffsetAnimation(_topRightGradientStop, 0.25f);
                StartColorAnimation(_topRightGradientStop, Red);

                StartOffsetAnimation(_bottomLeftGradientStop, 1f);
                StartColorAnimation(_bottomLeftGradientStop, Black);
            }
            else
            {
                StartOffsetAnimation(_bottomRightGradientStop, 1f);
                StartColorAnimation(_bottomRightGradientStop, Green);

                StartOffsetAnimation(_topLeftradientStop, 0.25f);
                StartColorAnimation(_topLeftradientStop, Blue);

                StartOffsetAnimation(_topRightGradientStop, 0f);
                StartColorAnimation(_topRightGradientStop, Red);

                StartOffsetAnimation(_bottomLeftGradientStop, 0.75f);
                StartColorAnimation(_bottomLeftGradientStop, Pink);

            }
        }

        private void StartOffsetAnimation(CompositionColorGradientStop gradientOffset, float offset)
        {
            var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.Duration = TimeSpan.FromSeconds(1);
            offsetAnimation.InsertKeyFrame(1.0f, offset);
            gradientOffset.StartAnimation(nameof(CompositionColorGradientStop.Offset), offsetAnimation);
        }

        private void StartColorAnimation(CompositionColorGradientStop gradientOffset, Color color)
        {
            var colorAnimation = _compositor.CreateColorKeyFrameAnimation();
            colorAnimation.Duration = TimeSpan.FromSeconds(2);
            colorAnimation.Direction = Windows.UI.Composition.AnimationDirection.Alternate;
            colorAnimation.InsertKeyFrame(1.0f, color);
            gradientOffset.StartAnimation(nameof(CompositionColorGradientStop.Color), colorAnimation);
        }




        /// <summary>
        /// 获取或设置IsInPomodoro的值
        /// </summary>
        public bool IsInPomodoro
        {
            get => (bool)GetValue(IsInPomodoroProperty);
            set => SetValue(IsInPomodoroProperty, value);
        }

        /// <summary>
        /// 标识 IsInPomodoro 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsInPomodoroProperty =
            DependencyProperty.Register(nameof(IsInPomodoro), typeof(bool), typeof(GlowPanel2), new PropertyMetadata(default(bool), OnIsInPomodoroChanged));

        private static void OnIsInPomodoroChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as GlowPanel2;
            target?.OnIsInPomodoroChanged(oldValue, newValue);
        }

        /// <summary>
        /// IsInPomodoro 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">IsInPomodoro 属性的旧值。</param>
        /// <param name="newValue">IsInPomodoro 属性的新值。</param>
        protected virtual void OnIsInPomodoroChanged(bool oldValue, bool newValue)
        {
            UpdateGradients();
        }
    }
}