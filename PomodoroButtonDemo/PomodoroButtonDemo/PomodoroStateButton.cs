﻿using System;
using System.Collections.Generic;
using Windows.UI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace PomodoroButtonDemo
{
    [TemplateVisualState(GroupName = ProgressStatesName, Name = IdleStateName)]
    [TemplateVisualState(GroupName = ProgressStatesName, Name = BusyStateName)]
    [TemplateVisualState(GroupName = PromodoroStatesName, Name = InworkStateName)]
    [TemplateVisualState(GroupName = PromodoroStatesName, Name = BreakStateName)]

    public class PomodoroStateButton : Button
    {

        private const string ProgressStatesName = "ProgressStates";
        private const string IdleStateName = "Idle";
        private const string BusyStateName = "Busy";

        private const string PromodoroStatesName = "PromodoroStates";
        private const string InworkStateName = "Inwork";
        private const string BreakStateName = "Break";

        /// <summary>
        /// 标识 IsInPomodoro 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsInPomodoroProperty =
            DependencyProperty.Register(nameof(IsInPomodoro), typeof(bool), typeof(PomodoroStateButton), new PropertyMetadata(default(bool), OnIsInPomodoroChanged));

        /// <summary>
        /// 标识 IsTimerInProgress 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsTimerInProgressProperty =
            DependencyProperty.Register(nameof(IsTimerInProgress), typeof(bool), typeof(PomodoroStateButton), new PropertyMetadata(default(bool), OnIsTimerInProgressChanged));

        /// <summary>
        /// 标识 StartCommand 依赖属性。
        /// </summary>
        public static readonly DependencyProperty StartCommandProperty =
            DependencyProperty.Register(nameof(StartCommand), typeof(ICommand), typeof(PomodoroStateButton), new PropertyMetadata(null));

        /// <summary>
        /// 标识 StopCommand 依赖属性。
        /// </summary>
        public static readonly DependencyProperty StopCommandProperty =
            DependencyProperty.Register(nameof(StopCommand), typeof(ICommand), typeof(PomodoroStateButton), new PropertyMetadata(null));



        /// <summary>
        /// 获取或设置ShadowColor的值
        /// </summary>
        public Color ShadowColor
        {
            get => (Color)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }

        /// <summary>
        /// 标识 ShadowColor 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register(nameof(ShadowColor), typeof(Color), typeof(PomodoroStateButton), new PropertyMetadata(Colors.Black, OnShadowColorChanged));

        private static void OnShadowColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (Color)args.OldValue;
            var newValue = (Color)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as PomodoroStateButton;
            target?.OnShadowColorChanged(oldValue, newValue);
        }

        /// <summary>
        /// ShadowColor 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">ShadowColor 属性的旧值。</param>
        /// <param name="newValue">ShadowColor 属性的新值。</param>
        protected virtual void OnShadowColorChanged(Color oldValue, Color newValue)
        {
        }

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
        /// 获取或设置IsTimerInProgress的值
        /// </summary>
        public bool IsTimerInProgress
        {
            get => (bool)GetValue(IsTimerInProgressProperty);
            set => SetValue(IsTimerInProgressProperty, value);
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
        /// 获取或设置StopCommand的值
        /// </summary>
        public ICommand StopCommand
        {
            get => (ICommand)GetValue(StopCommandProperty);
            set => SetValue(StopCommandProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateVisualStates(false);
        }

        /// <summary>
        /// IsInPomodoro 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">IsInPomodoro 属性的旧值。</param>
        /// <param name="newValue">IsInPomodoro 属性的新值。</param>
        protected virtual void OnIsInPomodoroChanged(bool oldValue, bool newValue)
        {
            UpdateVisualStates(true);
        }

        /// <summary>
        /// IsTimerInProgress 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">IsTimerInProgress 属性的旧值。</param>
        /// <param name="newValue">IsTimerInProgress 属性的新值。</param>
        protected virtual void OnIsTimerInProgressChanged(bool oldValue, bool newValue)
        {
            UpdateVisualStates(true);
        }



        protected virtual void UpdateVisualStates(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsInPomodoro ? InworkStateName : BreakStateName, useTransitions);
            VisualStateManager.GoToState(this, IsTimerInProgress ? BusyStateName : IdleStateName, useTransitions);
        }

        private static void OnIsInPomodoroChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as PomodoroStateButton;
            target?.OnIsInPomodoroChanged(oldValue, newValue);
        }

        private static void OnIsTimerInProgressChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as PomodoroStateButton;
            target?.OnIsTimerInProgressChanged(oldValue, newValue);
        }

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
