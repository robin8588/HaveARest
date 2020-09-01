using System; 
using System.Configuration;
using System.Reflection;
using System.Threading; 
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private System.Windows.Forms.NotifyIcon notifyIcon = null;
        public MainWindow()
        {
            InitializeComponent();
            countDownSecond.Text = ConfigurationManager.AppSettings["CountDownSecond"];
            intervalSecond.Text = ConfigurationManager.AppSettings["IntervalSecond"];
            InitNotify();
        }
        private void InitNotify() {

            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Text = "";
            notifyIcon.Visible = true;
            notifyIcon.Icon = ToIcon("20150422052746327_easyicon_net_128.ico");
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu();
            notifyIcon.ContextMenu.MenuItems.Add("退出",(o,e)=>Application.Current.Shutdown());  
            notifyIcon.DoubleClick += (o, e) => { this.Show(); };
        }

        private System.Drawing.Icon ToIcon(string resName)
        {
            Assembly myAssembly;
            myAssembly = Assembly.GetExecutingAssembly();
            System.Resources.ResourceManager rm = new
            System.Resources.ResourceManager(myAssembly.GetName().Name+ ".g‎", myAssembly);
            var item= rm.GetObject(resName);
            return new System.Drawing.Icon((System.IO.Stream)item);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btn.IsEnabled = false;
            countDownSecond.IsEnabled = false;
            intervalSecond.IsEnabled = false;
            this.WindowState = WindowState.Minimized;
            mw = new MaskWindow(); mw.ShowALLScreens(); this.Topmost = false; this.WindowState = WindowState.Minimized;
            //initTTime();
            //timer = new Timer(WorkCountDown, null, 1000, Timeout.Infinite);
        }

        private void WorkCountDown(object state)
        {
            countDownTime -= new TimeSpan(0, 0, 1);
            Dispatcher.Invoke(() => lbcountDownShow.Content = countDownTime.TotalSeconds);
            if (countDownTime == new TimeSpan(0))
            {
                Dispatcher.Invoke(initRTime);
                timer = new Timer(RestCountDown, null, 0, Timeout.Infinite);
                Dispatcher.Invoke(() => { mw = new MaskWindow(); mw.ShowALLScreens(); this.Topmost = false; this.WindowState = WindowState.Minimized; });
            }
            else if (countDownTime == new TimeSpan(0, 1, 0))
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        { 
            this.Hide();
            e.Cancel = true;
        }
    }
}
