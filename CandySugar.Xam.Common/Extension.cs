using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using XExten.Advance.HttpFramework.MultiFactory;
using XExten.Advance.LinqFramework;

namespace CandySugar.Xam.Common
{
    public class Extension
    {
        public static string VersionCode { get; set; }
        public static string ReadLocalHtml(string input)
        {
            var htmlst = typeof(Extension).GetTypeInfo().Assembly.GetManifestResourceStream($"CandySugar.Xam.Common.WebViewAssets.{input}.html");
            using (var reader = new StreamReader(htmlst))
            {
                return reader.ReadToEnd();
            }
        }
        public static string FileNameFilter(string input)
        {
            string[] Filter = { ":", "\\", "/", "*", "?", "<", ">", "|", "\"" };
            Filter.ForArrayEach<string>(item =>
            {
                if (input.Contains(item))
                {
                    input = input.Replace(item, "_");
                }
            });
            return input;
        }
        public static string AndroidAssetsPath => "file:///android_asset/";
        public static string IOSAssetsPath => "ms-appx-web:///";
        public static bool CheckVersion()
        {
            var ServerVersion = IHttpMultiClient.HttpMulti.AddNode(opt =>
            {
                opt.NodePath = "https://gitee.com/EmilyEdna/CandySugar/raw/master/CandySugarOption";
            }).Build().RunStringFirst();
            var AppVersion = ServerVersion.ToModel<JObject>()["App"].ToString();
            if (AppVersion.Equals(VersionCode)) return false;
            else return true;
        }
    }
}
