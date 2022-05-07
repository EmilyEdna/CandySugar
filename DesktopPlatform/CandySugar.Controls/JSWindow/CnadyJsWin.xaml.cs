using System.Net;
using System.Windows;
using XExten.Advance.CacheFramework;
using XExten.Advance.LinqFramework;

namespace CandySugar.Controls.JSWindow
{
    /// <summary>
    /// CnadyJsWin.xaml 的交互逻辑
    /// </summary>
    public partial class CnadyJsWin : Window
    {
        public CnadyJsWin()
        {
            InitializeComponent();
            InitializeAsync();
        }

        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);

            webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
            webView.CoreWebView2.Settings.AreDevToolsEnabled = true;

            webView.CoreWebView2.WebResourceResponseReceived += (sender, args) =>
            {
                if (args.Request.Headers.Contains("cookie"))
                {
                    var cookie = args.Request.Headers.GetHeader("cookie");
                    if (!cookie.IsNullOrEmpty())
                    {
                        Caches.RunTimeCacheRemove("AcgCookie");
                        Caches.RunTimeCacheSet("AcgCookie", cookie, 60 * 5);
                        try
                        {
                            this.DialogResult = true;
                        }
                        catch { return; }
                    }
                }
            };
        }
    }
}
