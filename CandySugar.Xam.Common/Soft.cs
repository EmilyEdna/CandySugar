using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common
{
    public class Soft
    {
        public const string S12X = "rating:safe";
        public const string S15X = "rating:questionable";
        public const string S18X = "rating:explicit";
        public const string Toast = "{0}_{1}_网络有波动，请稍后再试~`(*>﹏<*)′";
        public static string ProxyIP { get; set; } = "";
        public static int ProxyPort { get; set; } = -1;
        public static string ProxyPwd { get; set; }
        public static string ProxyAccount { get; set; }
        public static int CacheTime { get; set; } = 60;
        /// <summary>
        /// 屏幕宽度
        /// </summary>
        public static double ScreenWidth { get; set; }
        /// <summary>
        /// 屏幕高度
        /// </summary>
        public static double ScreenHeight { get; set; }
        /// <summary>
        /// LoadMore加载更多
        /// </summary>
        public static int WaitSpan { get; set; } = 500;
        /// <summary>
        /// 模糊度
        /// </summary>
        public static double Blur { get; set; } = 15;
        /// <summary>
        /// 壁纸年龄模式 [5：ALL]---[10：12]---[15：15]---[20：18]
        /// </summary>
        public static int AgeModule { get; set; } = 0;
    }
}
