using System;
using System.Configuration;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
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
            this.ShowInTaskbar = false;
            InitNotify();
        }
        private void InitNotify()
        {

            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Text = "";
            notifyIcon.Visible = true;
            notifyIcon.Icon = ToIcon("20150422052746327_easyicon_net_128.ico");
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu();
            notifyIcon.ContextMenu.MenuItems.Add("退出", (o, e) => Application.Current.Shutdown());
            notifyIcon.DoubleClick += (o, e) => { this.Show(); this.WindowState = WindowState.Normal; };
        }

        private System.Drawing.Icon ToIcon(string resName)
        {
            Assembly myAssembly;
            myAssembly = Assembly.GetExecutingAssembly();
            System.Resources.ResourceManager rm = new
            System.Resources.ResourceManager(myAssembly.GetName().Name + ".g‎", myAssembly);
            var item = rm.GetObject(resName);
            return new System.Drawing.Icon((System.IO.Stream)item);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btn.IsEnabled = false;
            countDownSecond.IsEnabled = false;
            intervalSecond.IsEnabled = false;
            this.Hide();
            initTTime();
            timer = new Timer(WorkCountDown, null, 1000, Timeout.Infinite);
            notifyIcon.ShowBalloonTip(1000, "后台运行", "右点图标进行更多操作", System.Windows.Forms.ToolTipIcon.Info);
        }

        private void WorkCountDown(object state)
        {
            countDownTime -= new TimeSpan(0, 0, 1);
            Dispatcher.Invoke(() => lbcountDownShow.Content = countDownTime.TotalSeconds);
            if (countDownTime == new TimeSpan(0))
            {
                Dispatcher.Invoke(initRTime);
                timer = new Timer(RestCountDown, null, 0, Timeout.Infinite);
                Dispatcher.Invoke(() =>
                {
                    mw = new MaskWindow();
                    mw.ShowALLScreens();
                });
            }
            else if (countDownTime == new TimeSpan(0, 1, 0))
            {
                Dispatcher.Invoke(() =>
                {
                    this.WindowState = WindowState.Normal;
                    Application curApp = Application.Current;
                    this.Topmost = true;
                    this.Show();
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
                Dispatcher.Invoke(() =>
                {
                    if (mw != null)
                    { mw.Close(); this.Hide(); }
                });
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
            this.Hide();
            this.WindowState = WindowState.Minimized;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x8000;
        private const int WS_MINIMIZE = -131073;
        private const int WS_MAXIMIZE = -65537;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var hwnd = new WindowInteropHelper(this).Handle;
            var value = GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU & WS_MINIMIZE & WS_MAXIMIZE;
            SetWindowLong(hwnd, GWL_STYLE, value);
        }
    }
}
