using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Shapes;

namespace PomodoroButtonDemo
{
    public class InsetPanel : ElementDecorator
    {
        private readonly Compositor _compositor;
        private CompositionMaskBrush _maskBrush;
        private readonly DropShadow _dropShadow;

        public InsetPanel() : base()
        {
            this.DefaultStyleKey = typeof(InsetPanel);
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _dropShadow = _compositor.CreateDropShadow();
            _dropShadow.BlurRadius = 4;
            _dropShadow.Color = Colors.White;
            _dropShadow.Opacity = 0.7f;
            Visual.Shadow = _dropShadow;
            Visual.Scale = new System.Numerics.Vector3(0.91f);
        }

        protected override void UpdateOutlineMask()
        {
            if (RelativeElement is Shape shape)
            {
                var mask = shape.GetAlphaMask();
                _dropShadow.Mask = mask;
            }
            else if (RelativeElement is TextBlock textBlock)
            {
                var mask = textBlock.GetAlphaMask();
                _dropShadow.Mask = mask;
            }
        }
    }
}
