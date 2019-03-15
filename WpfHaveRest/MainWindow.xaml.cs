using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfHaveRest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer timer;
        TimeSpan countDownTime;
        TimeSpan restTime;
        MaskWindow mw;
        public MainWindow()
        {
            InitializeComponent();
            countDownSecond.Text = ConfigurationManager.AppSettings["CountDownSecond"];
            intervalSecond.Text = ConfigurationManager.AppSettings["IntervalSecond"];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btn.IsEnabled = false;
            countDownSecond.IsEnabled = false;
            intervalSecond.IsEnabled = false;
            this.WindowState = WindowState.Minimized;
            initTTime();
            timer = new Timer(WorkCountDown, null, 1000, Timeout.Infinite);
        }

        private void WorkCountDown(object state)
        {
            countDownTime -= new TimeSpan(0, 0, 1);
            Dispatcher.Invoke(() => lbcountDownShow.Content = countDownTime.TotalSeconds);
            if (countDownTime == new TimeSpan(0))
            {
                Dispatcher.Invoke(initRTime);
                timer = new Timer(RestCountDown, null, 0, Timeout.Infinite);
                Dispatcher.Invoke(() => { mw = new MaskWindow(); mw.Show(); this.Topmost = false; this.WindowState = WindowState.Minimized; });
            }
            else if (countDownTime == new TimeSpan(0, 0, 10))
            {
                Dispatcher.Invoke(() =>
                {
                    this.WindowState = WindowState.Normal;
                    this.Topmost = true;
                });
                timer.Change(1000, Timeout.Infinite);
            }
            else
            {
                timer.Change(1000, Timeout.Infinite);
            }
        }

        private void RestCountDown(object state)
        {
            restTime -= new TimeSpan(0, 0, 1);
            Dispatcher.Invoke(() => lbcountDownShow.Content = restTime.TotalSeconds);
            if (restTime == new TimeSpan(0))
            {
                Dispatcher.Invoke(initTTime);
                timer = new Timer(WorkCountDown, null, 0, Timeout.Infinite);
                Dispatcher.Invoke(() => { if (mw != null) { mw.Close(); } });
            }
            else
            {
                timer.Change(1000, Timeout.Infinite);
            }
        }

        private void initTTime()
        {
            countDownTime = new TimeSpan(0, Convert.ToInt32(countDownSecond.Text), 0);
            restTime = new TimeSpan(0, Convert.ToInt32(intervalSecond.Text), 0);
            lbcountDownShow.Content = countDownTime.TotalSeconds;
        }

        private void initRTime()
        {
            countDownTime = new TimeSpan(0, Convert.ToInt32(countDownSecond.Text), 0);
            restTime = new TimeSpan(0, Convert.ToInt32(intervalSecond.Text), 0);
            lbcountDownShow.Content = restTime.TotalSeconds;
        }

        private void Delay_Button_Click(object sender, RoutedEventArgs e)
        {
            countDownTime += new TimeSpan(0, 10, 0);
            this.Topmost = false;
            this.WindowState = WindowState.Minimized;
        }
    }
}
