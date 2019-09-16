using System;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace PomodoroButtonDemo
{
    public class PomodoroButtonBrush : XamlCompositionBrushBase
    {
        /// <summary>
        /// 标识 IsInPomodoro 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsInPomodoroProperty =
            DependencyProperty.Register(nameof(IsInPomodoro), typeof(bool), typeof(PomodoroButtonBrush), new PropertyMetadata(default(bool), OnIsInPomodoroChanged));

        /// <summary>
        /// 标识 PomodoroColor 依赖属性。
        /// </summary>
        public static readonly DependencyProperty PomodoroColorProperty =
            DependencyProperty.Register(nameof(PomodoroColor), typeof(Color), typeof(PomodoroButtonBrush), new PropertyMetadata(Colors.White, OnPomodoroColorChanged));

        /// <summary>
        /// 标识 BreakColor 依赖属性。
        /// </summary>
        public static readonly DependencyProperty BreakColorProperty =
            DependencyProperty.Register(nameof(BreakColor), typeof(Color), typeof(PomodoroButtonBrush), new PropertyMetadata(Colors.White, OnBreakColorChanged));

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(PomodoroButtonBrush), new PropertyMetadata(TimeSpan.FromSeconds(0.3d), (s, a) =>
            {
                if (a.NewValue != a.OldValue)
                {
                    if (s is PomodoroButtonBrush sender)
                    {
                        if (sender._clorAnimation != null)
                        {
                            sender._clorAnimation.Duration = (TimeSpan)a.NewValue;
                        }
                    }
                }
            }));


        private Compositor _compositor;

        private ColorKeyFrameAnimation _clorAnimation;

        private bool _isConnected;


        public PomodoroButtonBrush()
        {
            _compositor = Window.Current.Compositor;
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
        /// 获取或设置PomodoroColor的值
        /// </summary>
        public Color PomodoroColor
        {
            get => (Color)GetValue(PomodoroColorProperty);
            set => SetValue(PomodoroColorProperty, value);
        }

        /// <summary>
        /// 获取或设置BreakColor的值
        /// </summary>
        public Color BreakColor
        {
            get => (Color)GetValue(BreakColorProperty);
            set => SetValue(BreakColorProperty, value);
        }

        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// IsInPomodoro 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">IsInPomodoro 属性的旧值。</param>
        /// <param name="newValue">IsInPomodoro 属性的新值。</param>
        protected virtual void OnIsInPomodoroChanged(bool oldValue, bool newValue)
        {
            StartColorAnimation();
        }

        //被设置到控件属性时触发，例RootGrid.Background=new FluentSolidColorBrush();
        protected override void OnConnected()
        {
            if (CompositionBrush == null)
            {
                _isConnected = true;
                _clorAnimation = _compositor.CreateColorKeyFrameAnimation();
                //进度为0的关键帧，表达式为起始颜色。
                _clorAnimation.InsertExpressionKeyFrame(0f, "this.StartingValue");

                //进度为0的关键帧，表达式为参数名为Color的参数。
                _clorAnimation.InsertExpressionKeyFrame(1f, "Color");

                //创建颜色笔刷
                CompositionBrush = _compositor.CreateColorBrush(IsInPomodoro ? PomodoroColor : BreakColor);
            }
        }

        //从属性中移除时触发，例RootGrid.Background=null;
        protected override void OnDisconnected()
        {
            if (CompositionBrush != null)
            {
                _isConnected = false;
                _clorAnimation.Dispose();
                _clorAnimation = null;
                CompositionBrush.Dispose();
                CompositionBrush = null;
            }
        }

        private static void OnIsInPomodoroChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as PomodoroButtonBrush;
            target?.OnIsInPomodoroChanged(oldValue, newValue);
        }

        private void StartColorAnimation()
        {
            if (_isConnected == false)
                return;

            _clorAnimation.SetColorParameter("Color", IsInPomodoro ? PomodoroColor : BreakColor);
            //创建一个动画批，CompositionAnimation使用批控制动画完成。
            var batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            //批内所有动画完成事件，完成时如果画刷没有Disconnected，则触发ColorChanged
            CompositionBrush.StartAnimation("Color", _clorAnimation);
            batch.End();
        }
    }
}
