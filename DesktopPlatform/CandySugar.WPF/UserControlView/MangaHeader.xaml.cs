using CandySugar.Common;
using CandySugar.Common.Enum;
using CandySugar.WPF.UserControlView.UserControlEvent;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using XExten.Advance.StaticFramework;

namespace CandySugar.WPF.UserControlView
{
    /// <summary>
    /// MangaHeader.xaml 的交互逻辑
    /// </summary>
    public partial class MangaHeader : UserBaseControl
    {
        public MangaHeader()
        {
            InitializeComponent();
            ThemeCombox(ThemeBox);
        }
    }
}
