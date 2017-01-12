﻿using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using WpfHaveRest.Properties;

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
            var url = Properties.Settings.Default.Url;
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

        protected override void OnClosed(EventArgs e)
        {
            browser.Dispose();
            base.OnClosed(e);
        }
    }
}
