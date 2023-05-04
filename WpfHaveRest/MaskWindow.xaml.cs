using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection; 
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
            string url = ConfigurationManager.AppSettings["MainPageUrl"];
            browser.Navigate(new Uri(url, UriKind.RelativeOrAbsolute));
            browser.Navigated += (a, b) => { HideScriptErrors(browser, true); };
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
            this.Width = screens.Sum(t => t.Bounds.Width);
            this.Height = screens.Max(t => t.Bounds.Height);
            this.WindowState = WindowState.Normal;
            this.Show();
        }
        protected override void OnClosed(EventArgs e)
        {
            browser.Dispose();
            base.OnClosed(e);
        }
    }
}
