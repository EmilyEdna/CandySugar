using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common
{
    public class Soft
    {
        public static string ProxyIP { get; set; } = "";
        public static int ProxyPort { get; set; } = -1;
        public static string ProxyPwd { get; set; }
        public static string ProxyAccount { get; set; }
        public static int CacheTime { get; set; } = 60;
    }
}
