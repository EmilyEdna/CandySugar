using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Common.Platform
{
    public interface IAndroidPlatform
    {
        /// <summary>
        /// 隐藏状态栏
        /// </summary>
        void HiddenStatusBar();
        /// <summary>
        /// 显示状态栏
        /// </summary>
        void ShowStatusBar();
        /// <summary>
        /// 状态栏透明
        /// </summary>
        void Transparent();
        /// <summary>
        /// 状态栏不透明
        /// </summary>
        void ClearTransparent();
        /// <summary>
        /// 下载路径
        /// </summary>
        /// <returns></returns>
        string DownPath();
    }
}
