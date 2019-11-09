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
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using System.Threading.Tasks;
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
        List<PomodoroStateButton> buttons = new List<PomodoroStateButton>();
        public MainPage()
        {
            this.InitializeComponent();
            var textVisual = ElementCompositionPreview.GetElementVisual(Tit);
            compositor = textVisual.Compositor;

            containerVisual = compositor.CreateContainerVisual();
            var mask = Tit.GetAlphaMask();
            for (int i = 0; i < 100; i++)
            {
                var maskBrush = compositor.CreateMaskBrush();
                maskBrush.Mask = mask;
                maskBrush.Source = compositor.CreateColorBrush(Color.FromArgb(255, 167, 71, 55));
                var visual = compositor.CreateSpriteVisual();
                visual.Brush = maskBrush;
                visual.Offset = new System.Numerics.Vector3(i, i, 0);

                var bindSizeAnimation = compositor.CreateExpressionAnimation("textVisual.Size");
                bindSizeAnimation.SetReferenceParameter("textVisual", textVisual);
                visual.StartAnimation("Size", bindSizeAnimation);

                containerVisual.Children.InsertAtTop(visual);
            }

            //containerVisual.Opacity = 0.2f;
            ElementCompositionPreview.SetElementChildVisual(LongShadow, containerVisual);
            Loaded += MainPage_Loaded;
            buttons.Add(B1);
            buttons.Add(B2);
            buttons.Add(B3);
            buttons.Add(B4);


        }



        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(2000);

            foreach (var button in buttons)
            {
                var buttonRoot = VisualTreeHelper.GetChild(button, 0) as FrameworkElement;
                var buttonGroups = VisualStateManager.GetVisualStateGroups(buttonRoot).ToList();
                VisualStateManager.GoToState(button,"Normal", true);
            }

            var root = VisualTreeHelper.GetChild(B4, 0) as FrameworkElement;
            var groups = VisualStateManager.GetVisualStateGroups(root).ToList();
            foreach (var item in groups)
            {
                item.CurrentStateChanged += (s, args) =>
                {
                    foreach (var button in buttons)
                    {
                        VisualStateManager.GoToState(button, args.NewState.Name, true);
                    }
                };
            }
            //var clip = compositor.CreateInsetClip(0, 0, ShadowBorder.ActualSize.X, ShadowBorder.ActualSize.Y);
            var geometry = compositor.CreateRectangleGeometry();
            geometry.Size = ShadowBorder.ActualSize;
            var clip = compositor.CreateGeometricClip(geometry);
            containerVisual.Clip = clip;
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
                    foreach (var item in buttons)
                    {
                        item.IsInPomodoro = true;
                        item.IsTimerInProgress = false;
                    }

                    break;
                case 1:

                    foreach (var item in buttons)
                    {
                        item.IsInPomodoro = true;
                        item.IsTimerInProgress = true;
                    }

                    break;
                case 2:
                    foreach (var item in buttons)
                    {
                        item.IsInPomodoro = false;
                        item.IsTimerInProgress = false;
                    }
                    break;
                case 3:
                    foreach (var item in buttons)
                    {
                        item.IsInPomodoro = false;
                        item.IsTimerInProgress = true;
                    }
                    break;

                default:
                    break;

            }
        }
    }
}
