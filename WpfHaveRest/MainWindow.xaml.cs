using System;
using System.Collections.Generic;
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btn.IsEnabled = false;
            countDownSecond.IsEnabled = false;
            intervalSecond.IsEnabled = false;
            this.WindowState = WindowState.Minimized;
            initTTime();
            timer = new Timer(TCountDown, null, 1000, Timeout.Infinite);
        }

        private void TCountDown(object state)
        {
            countDownTime -= new TimeSpan(0,0,1);
            Dispatcher.Invoke(()=>lbcountDownShow.Content = countDownTime.TotalSeconds);
            if (countDownTime == new TimeSpan(0))
            {
                Dispatcher.Invoke(initRTime);
                timer = new Timer(RCountDown, null, 0, Timeout.Infinite);
                Dispatcher.Invoke(() => { mw = new MaskWindow(); mw.Show();   });
            }
            else
            {
                timer.Change(1000, Timeout.Infinite);
            }
        }

        private void RCountDown(object state)
        {
            restTime -= new TimeSpan(0, 0, 1);
            Dispatcher.Invoke(()=>lbcountDownShow.Content = restTime.TotalSeconds);
            if (restTime == new TimeSpan(0))
            {
                Dispatcher.Invoke(initTTime);
                timer = new Timer(TCountDown, null, 0, Timeout.Infinite);
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

       

       

    }
}
