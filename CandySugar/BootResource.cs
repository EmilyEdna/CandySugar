using CandySugar.CandyWindows;
using CandySugar.Controls.UIElementHelper;
using CandySugar.Properties;
using HandyControl.Controls;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XExten.Advance.LinqFramework;


namespace CandySugar
{
    public class BootResource
    {
        /// <summary>
        /// 设置全局用户配置的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        public static void SetSettingPropertyValue<T>(T param)
        {
            param.GetType().GetProperties().ForEnumerEach(t =>
            {
                Soft.Default.GetType().GetProperty(t.Name).SetValue(Soft.Default, t.GetValue(param));
            });
        }

        private static ConcurrentDictionary<string, CandyMangaReaderWin> MangaReaderWindow = new ConcurrentDictionary<string, CandyMangaReaderWin>();

        /// <summary>
        /// 控制窗体打开
        /// </summary>
        /// <param name="action"></param>
        public static void Manga(Action<CandyMangaReaderWin> action)
        {
            CandyMangaReaderWin windows = null;
            if (MangaReaderWindow.ContainsKey(nameof(CandyMangaReaderWin)))
            {
                var old = MangaReaderWindow.Values.FirstOrDefault();
                old.Close();
                MangaReaderWindow.Clear();
                windows = new CandyMangaReaderWin();
                action(windows);
                var binding = new Binding
                {
                    Source = windows.DataContext,
                    //绑定到附加属性
                    Path = new PropertyPath("Loading"),
                    Mode=BindingMode.TwoWay
                };
                var content = (windows.Header.Content as ColorZone).Content as Grid;
                UlHelper.FindVisualChild<LoadingLine>(content)
                    .FirstOrDefault().SetBinding(UIElement.VisibilityProperty, binding);
                MangaReaderWindow.TryAdd(nameof(CandyMangaReaderWin), windows);
                windows.Show();
            }
            else
            {
                MangaReaderWindow.Clear();
                windows = new CandyMangaReaderWin();
                windows.Loading = true;
                action(windows);
                var binding = new Binding
                {
                    Source = windows.DataContext,
                    //绑定到附加属性
                    Path = new PropertyPath("Loading"),
                    Mode = BindingMode.TwoWay
                };
                var content = (windows.Header.Content as ColorZone).Content as Grid;
                UlHelper.FindVisualChild<LoadingLine>(content)
                    .FirstOrDefault().SetBinding(UIElement.VisibilityProperty, binding);
                MangaReaderWindow.TryAdd(nameof(CandyMangaReaderWin), windows);
                windows.Show();
            }
        }
    }
}
