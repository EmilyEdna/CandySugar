using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CandySugar.Xam.Common
{
    public class Extension
    {
        public static string ReadLocalHtml()
        {
            var htmlst = typeof(Extension).GetTypeInfo().Assembly.GetManifestResourceStream("CandySugar.Xam.Common.WebViewAssets.index.html");
            using (var reader = new StreamReader(htmlst))
            {
                return reader.ReadToEnd();
            }
        }
        public static string AndroidAssetsPath => "file:///android_asset/";
        public static string IOSAssetsPath => "ms-appx-web:///";
    }
}
