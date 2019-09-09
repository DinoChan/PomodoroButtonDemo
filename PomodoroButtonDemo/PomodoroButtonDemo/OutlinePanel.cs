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

namespace PomodoroButtonDemo
{
    [TemplatePartAttribute(Name = OutlineBorderName, Type = typeof(Border))]
    public class OutlinePanel : ContentControl
    {
        private const string OutlineBorderName = "OutlineBorder";

        private Border _outlineBorder;
        private readonly Compositor _compositor;
        private SpriteVisual _visual;
        private CompositionMaskBrush _maskBrush;

        public OutlinePanel()
        {
            this.DefaultStyleKey = typeof(OutlinePanel);
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _visual = _compositor.CreateSpriteVisual();
            _maskBrush = _compositor.CreateMaskBrush();
            _visual.Brush = _maskBrush;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _outlineBorder = GetTemplateChild(OutlineBorderName) as Border;
            if (_outlineBorder == null)
                return;

            ElementCompositionPreview.SetElementChildVisual(_outlineBorder, _visual);

            ConfigureShadowVisualForCastingElement();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (oldContent != null)
            {
                if (oldContent is FrameworkElement oldElement)
                {
                    oldElement.SizeChanged -= OnSizeChanged;
                }
            }

            if (newContent != null)
            {
                if (newContent is FrameworkElement newElement)
                {
                    newElement.SizeChanged += OnSizeChanged;
                }
            }

            base.OnContentChanged(oldContent, newContent);
        }

        private void ConfigureShadowVisualForCastingElement()
        {
            UpdateOutlineMask();

            UpdateOutlineSize();
        }

        private void UpdateOutlineMask()
        {
            if (Content is Shape shape)
            {
                var mask = shape.GetAlphaMask();
                _maskBrush.Mask = mask;
                _maskBrush.Source = _compositor.CreateColorBrush(Colors.White);
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateOutlineSize();
        }

        private void UpdateOutlineSize()
        {
            if (_visual != null)
            {
                Vector2 newSize = new Vector2(0, 0);
                if (Content is FrameworkElement frameworkElement)
                {
                    newSize = new Vector2((float)frameworkElement.ActualWidth, (float)frameworkElement.ActualHeight);
                }

                _visual.Size = newSize;
            }
        }
    }
}
