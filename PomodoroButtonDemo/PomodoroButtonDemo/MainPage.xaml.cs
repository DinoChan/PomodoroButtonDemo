using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
            _index++;

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
