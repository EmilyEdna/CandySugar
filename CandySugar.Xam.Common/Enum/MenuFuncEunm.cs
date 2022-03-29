using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Xam.Common.Enum
{
    public enum MenuFuncEunm
    {
        /// <summary>
        /// 小说
        /// </summary>
        [Description("小说")]
        Novel = 1,
        /// <summary>
        /// 轻小说
        /// </summary>
        [Description("轻小说")]
        LightNovel,
        /// <summary>
        /// 动漫
        /// </summary>
        [Description("动漫")]
        Anime,
        /// <summary>
        /// 漫画
        /// </summary>
        [Description("漫画")]
        Manga,
        /// <summary>
        /// 画集
        /// </summary>
        [Description("画集")]
        Wallpaper,
        /// <summary>
        /// 茶杯
        /// </summary>
        [Description("茶杯")]
        Axgle,
        /// <summary>
        /// 音乐
        /// </summary>
        [Description("音乐")]
        Music,
        /// <summary>
        /// 设置
        /// </summary>
        [Description("设置")]
        Setting,
        /// <summary>
        /// 关于
        /// </summary>
        [Description("关于")]
        About
    }
}
