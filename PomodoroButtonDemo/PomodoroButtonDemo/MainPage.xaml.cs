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
// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace PomodoroButtonDemo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int _index;
        public MainPage()
        {
            this.InitializeComponent();


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
            Button.Height += 1;
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
