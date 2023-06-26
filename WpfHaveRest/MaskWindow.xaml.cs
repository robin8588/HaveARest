using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfHaveRest
{
    /// <summary>
    /// MaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MaskWindow : Window
    {
        public MaskWindow()
        {
            InitializeComponent();
        }

        public void LoadingMask(string url, string seconds)
        {
            browser.Navigate(new Uri(url, UriKind.RelativeOrAbsolute));
            browser.Navigated += (a, b) => { HideScriptErrors(browser, true); };
            browser.LoadCompleted += (a, b) => { DisplayTime(seconds); };
        }

        public void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide); //In case we are to early
                return;
            }
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }

        public void ShowALLScreens()
        {
            List<System.Windows.Forms.Screen> screens = System.Windows.Forms.Screen.AllScreens.ToList();
            this.WindowStyle = WindowStyle.None;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = 0;
            this.Top = 0;
#if DEBUG
            this.Width = 1024;
            this.Height = 768;
#else
            this.Width = screens.Sum(t => t.Bounds.Width);
            this.Height = screens.Max(t => t.Bounds.Height);
#endif
            this.WindowState = WindowState.Normal;
            this.Show();
        }
        protected override void OnClosed(EventArgs e)
        {
            browser.Dispose();
            base.OnClosed(e);
        }

        public void DisplayTime(string seconds)
        {
            string script = $@"
            
            
            let countdownSeconds = {seconds} * 60;

          
            const countdownElement = document.getElementById('sb_form_q');

            
            function updateCountdownDisplay() {{
                countdownElement.value = countdownSeconds + ' 秒';
            }}

            
            function countdown() {{
                if (countdownSeconds > 0) {{
                    countdownSeconds--;
                    updateCountdownDisplay();
                }} else {{
                    clearInterval(countdownInterval);
                }}
            }}

            
            updateCountdownDisplay();

            
            const countdownInterval = setInterval(countdown, 1000);
            
            ";
            browser.InvokeScript("eval", script);
        }
    }
}
