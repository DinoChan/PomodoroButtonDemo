using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Composition;
using System.Runtime.CompilerServices;
using System.Numerics;
// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace PomodoroButtonDemo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int _index;
        ContainerVisual containerVisual;
        Compositor compositor;
        public MainPage()
        {
            this.InitializeComponent();
            Vector3 a = new Vector3(1, 1, 1);
            Vector3 b = new Vector3(-1, 1, 544);
          var c=  Vector3.Max(a, b);
            Loaded += MainPage_Loaded;
            MackLongShadow(170, 0.3f);

        }

        private void MackLongShadow(int depth, float opacity)
        {
            var textVisual = ElementCompositionPreview.GetElementVisual(Tit);
            compositor = textVisual.Compositor;

            containerVisual = compositor.CreateContainerVisual();
            var mask = Tit.GetAlphaMask();
            for (int i = 0; i < depth; i++)
            {
                var maskBrush = compositor.CreateMaskBrush();

                Vector3 background = new Vector3(232, 122, 105);
                Vector3 shadowColor = background - (background - new Vector3(0, 0, 0)) * opacity;
                shadowColor.X = Math.Max(0, shadowColor.X);
                shadowColor.Y = Math.Max(0, shadowColor.Y);
                shadowColor.Z = Math.Max(0, shadowColor.Z);
                shadowColor += (background - shadowColor) * i / depth;
                maskBrush.Mask = mask;
                maskBrush.Source = compositor.CreateColorBrush(Color.FromArgb(255, (byte)shadowColor.X, (byte)shadowColor.Y, (byte)shadowColor.Z));
                var visual = compositor.CreateSpriteVisual();
                visual.Brush = maskBrush;
                visual.Offset = new System.Numerics.Vector3(i + 1, i + 1, 0);
                //visual.CompositeMode = CompositionCompositeMode.MinBlend;
                var bindSizeAnimation = compositor.CreateExpressionAnimation("textVisual.Size");
                bindSizeAnimation.SetReferenceParameter("textVisual", textVisual);
                visual.StartAnimation("Size", bindSizeAnimation);

                containerVisual.Children.InsertAtBottom(visual);
            }

            //containerVisual.Opacity = 0.2f;
            ElementCompositionPreview.SetElementChildVisual(LongShadow, containerVisual);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //var clip = compositor.CreateInsetClip(0, 0, ShadowBorder.ActualSize.X, ShadowBorder.ActualSize.Y);
            //var geometry = compositor.CreateRectangleGeometry();
            //geometry.Size = ShadowBorder.ActualSize;
            //var clip = compositor.CreateGeometricClip(geometry);
            //containerVisual.Clip = clip;
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            //var brush = EE.GetAlphaMask();
            _index++;

            //var _compositor = Window.Current.Compositor;


            //var maskBrush = _compositor.CreateMaskBrush();

            //maskBrush.Mask = brush;
            //maskBrush.Source = _compositor.CreateColorBrush(Colors.White);
            //var _backgroundVisual = _compositor.CreateSpriteVisual();
            //_backgroundVisual.Brush = maskBrush;
            //_backgroundVisual.Size = new System.Numerics.Vector2(300);
            //ElementCompositionPreview.SetElementChildVisual(RR, _backgroundVisual);

            switch (_index % 4)
            {
                case 0:
                    Button.IsInPomodoro = true;
                    Button.IsTimerInProgress = false;
                    break;
                case 1:
                    Button.IsInPomodoro = true;
                    Button.IsTimerInProgress = true;
                    break;
                case 2:
                    Button.IsInPomodoro = false;
                    Button.IsTimerInProgress = false;
                    break;
                case 3:
                    Button.IsInPomodoro = false;
                    Button.IsTimerInProgress = true;
                    break;

                default:
                    break;

            }
        }
    }
}
