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
        private string UA = "Mozilla/5.0 (Linux; Android 10; SM-G981B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.162 Mobile Safari/537.36 Edg/99.0.4844.74";
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