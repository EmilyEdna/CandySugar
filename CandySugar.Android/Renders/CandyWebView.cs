using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CandySugar.Droid.Renders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WebView), typeof(CandyWebView))]
namespace CandySugar.Droid.Renders
{
    public class CandyWebView : WebViewRenderer
    {
        private readonly Context _context;
        private string UA = "Mozilla/5.0 (Linux; U; Android 10; zh-CN; RMX1991 Build/QKQ1.191201.002) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/78.0.3904.108 UCBrowser/13.5.5.1135 Mobile Safari/537.36";
        public CandyWebView(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> args)
        {
            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                Control.SetWebViewClient(GetWebViewClient());
                Control.Settings.UserAgentString = UA;
            }
        }
    }
}