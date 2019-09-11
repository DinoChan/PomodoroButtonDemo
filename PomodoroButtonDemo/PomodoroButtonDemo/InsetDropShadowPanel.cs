using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PomodoroButtonDemo
{
    public class InsetDropShadowPanel : DropShadowPanel
    {
        private const string CommonStatesName = "PromodoroStates";
        private const string NormalStateName = "Normal";
        private const string PointerOverStateName = "PointerOver";
        private const string PressedStateName = "Pressed";

        /// <summary>
        /// 标识 State 依赖属性。
        /// </summary>
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(nameof(State), typeof(ButtonState), typeof(InsetDropShadowPanel), new PropertyMetadata(ButtonState.Normal, OnStateChanged));


        public InsetDropShadowPanel()
        {
            DefaultStyleKey = typeof(InsetDropShadowPanel);
        }


        /// <summary>
        /// 获取或设置State的值
        /// </summary>
        public ButtonState State
        {
            get => (ButtonState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }


        /// <summary>
        /// State 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">State 属性的旧值。</param>
        /// <param name="newValue">State 属性的新值。</param>
        protected virtual void OnStateChanged(ButtonState oldValue, ButtonState newValue)
        {
            UpdateVisualStates(true);
        }

        protected virtual void UpdateVisualStates(bool useTransitions)
        {
            var state = NormalStateName;
            switch (State)
            {
                case ButtonState.Normal:
                    state = NormalStateName;
                    break;
                case ButtonState.PointerOver:
                    state = PointerOverStateName;
                    break;
                case ButtonState.Pressed:
                    state = PressedStateName;
                    break;
                default:
                    break;
            }
            VisualStateManager.GoToState(this, state, useTransitions);
        }

        private static void OnStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (ButtonState)args.OldValue;
            var newValue = (ButtonState)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as InsetDropShadowPanel;
            target?.OnStateChanged(oldValue, newValue);
        }

    }
}
