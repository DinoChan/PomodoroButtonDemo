using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace PomodoroButtonDemo
{
    [TemplateVisualState(GroupName = ProgressStatesGroupName, Name = IdleStateName)]
    [TemplateVisualState(GroupName = ProgressStatesGroupName, Name = BusyStateName)]
    [TemplateVisualState(GroupName = PromodoroStatesGroupName, Name = InworkStateName)]
    [TemplateVisualState(GroupName = PromodoroStatesGroupName, Name = BreakStateName)]

    public class PomodoroStateButton : Button
    {

        private const string ProgressStatesGroupName = "ProgressStates";
        private const string IdleStateName = "Idle";
        private const string BusyStateName = "Busy";

        private const string PromodoroStatesGroupName = "PromodoroStates";
        private const string InworkStateName = "Inwork";
        private const string BreakStateName = "Break";

        public PomodoroStateButton()
        {
            DefaultStyleKey = typeof(PomodoroStateButton);
            Click += OnClick;
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
            DependencyProperty.Register(nameof(IsInPomodoro), typeof(bool), typeof(PomodoroStateButton), new PropertyMetadata(default(bool), OnIsInPomodoroChanged));

        private static void OnIsInPomodoroChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as PomodoroStateButton;
            target?.OnIsInPomodoroChanged(oldValue, newValue);
        }

        /// <summary>
        /// IsInPomodoro 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">IsInPomodoro 属性的旧值。</param>
        /// <param name="newValue">IsInPomodoro 属性的新值。</param>
        protected virtual void OnIsInPomodoroChanged(bool oldValue, bool newValue)
        {
        }


        /// <summary>
        /// 获取或设置IsTimerInProgress的值
        /// </summary>
        public bool IsTimerInProgress
        {
            get => (bool)GetValue(IsTimerInProgressProperty);
            set => SetValue(IsTimerInProgressProperty, value);
        }

        /// <summary>
        /// 标识 IsTimerInProgress 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsTimerInProgressProperty =
            DependencyProperty.Register(nameof(IsTimerInProgress), typeof(bool), typeof(PomodoroStateButton), new PropertyMetadata(default(bool), OnIsTimerInProgressChanged));

        private static void OnIsTimerInProgressChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as PomodoroStateButton;
            target?.OnIsTimerInProgressChanged(oldValue, newValue);
        }

        /// <summary>
        /// IsTimerInProgress 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">IsTimerInProgress 属性的旧值。</param>
        /// <param name="newValue">IsTimerInProgress 属性的新值。</param>
        protected virtual void OnIsTimerInProgressChanged(bool oldValue, bool newValue)
        {
        }



        /// <summary>
        /// 获取或设置StartCommand的值
        /// </summary>
        public ICommand StartCommand
        {
            get => (ICommand)GetValue(StartCommandProperty);
            set => SetValue(StartCommandProperty, value);
        }

        /// <summary>
        /// 标识 StartCommand 依赖属性。
        /// </summary>
        public static readonly DependencyProperty StartCommandProperty =
            DependencyProperty.Register(nameof(StartCommand), typeof(ICommand), typeof(PomodoroStateButton), new PropertyMetadata(null));


        /// <summary>
        /// 获取或设置StopCommand的值
        /// </summary>
        public ICommand StopCommand
        {
            get => (ICommand)GetValue(StopCommandProperty);
            set => SetValue(StopCommandProperty, value);
        }

        /// <summary>
        /// 标识 StopCommand 依赖属性。
        /// </summary>
        public static readonly DependencyProperty StopCommandProperty =
            DependencyProperty.Register(nameof(StopCommand), typeof(ICommand), typeof(PomodoroStateButton), new PropertyMetadata(null));

        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (IsTimerInProgress)
            {
                if (StopCommand != null && StopCommand.CanExecute(this))
                    StopCommand.Execute(this);
            }
            else
            {
                if (StartCommand != null && StartCommand.CanExecute(this))
                    StartCommand.Execute(this);
            }
        }
    }
}
