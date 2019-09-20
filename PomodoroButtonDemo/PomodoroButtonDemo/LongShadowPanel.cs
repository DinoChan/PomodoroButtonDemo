using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Shapes;

namespace PomodoroButtonDemo
{
    public class LongShadowPanel : ElementDecorator
    {
        private readonly Compositor _compositor;
        private CompositionMaskBrush _maskBrush;
        private readonly DropShadow _dropShadow;

        public LongShadowPanel() : base()
        {
            DefaultStyleKey = typeof(LongShadowPanel);
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _dropShadow = _compositor.CreateDropShadow();
            //_dropShadow.BlurRadius = 4;
            //_dropShadow.Color = Colors.White;
            //_dropShadow.Opacity = 0.7f;
            //Visual.Shadow = _dropShadow;
            //Visual.Scale = new System.Numerics.Vector3(0.91f);
        }

        protected override void UpdateOutlineMask()
        {

            if (RelativeElement is Shape shape)
            {
                var markBrush = _compositor.CreateMaskBrush();
                var mask = shape.GetAlphaMask();
                var maskVisual = _compositor.CreateSpriteVisual();
                maskVisual.Brush = mask;
                var containerVisual = _compositor.CreateContainerVisual();
                containerVisual.Children.InsertAtTop(maskVisual);
                
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
