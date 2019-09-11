﻿using System;
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

namespace PomodoroButtonDemo
{
    public class OutlinePanel : ButtonDecorator
    {
        private readonly Compositor _compositor;
        private CompositionMaskBrush _maskBrush;

        public OutlinePanel() : base()
        {
            this.DefaultStyleKey = typeof(OutlinePanel);
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _maskBrush = _compositor.CreateMaskBrush();
            Visual.Brush = _maskBrush;
        }

        protected override void UpdateOutlineMask()
        {
            if (RelativeElement is Shape shape)
            {
                var mask = shape.GetAlphaMask();
                _maskBrush.Mask = mask;
                _maskBrush.Source = _compositor.CreateColorBrush(Colors.White);
            }
            else if (RelativeElement is TextBlock textBlock)
            {
                var mask = textBlock.GetAlphaMask();
                _maskBrush.Mask = mask;
                _maskBrush.Source = _compositor.CreateColorBrush(Colors.White);
            }
        }
    }
}
