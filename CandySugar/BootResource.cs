using CandySugar.Properties;
using CandySugar.UserWindows;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private static ConcurrentDictionary<string, MangaReaderWindows> MangaReaderWindow = new ConcurrentDictionary<string, MangaReaderWindows>();

        /// <summary>
        /// 控制窗体打开
        /// </summary>
        /// <param name="action"></param>
        public static void Manga(Action<MangaReaderWindows> action)
        {
            MangaReaderWindows windows = null;
            if (MangaReaderWindow.ContainsKey(nameof(MangaReaderWindows)))
            {
                var old = MangaReaderWindow.Values.FirstOrDefault();
                old.Close();
                MangaReaderWindow.Clear();
                windows = new MangaReaderWindows();
                action(windows);
                MangaReaderWindow.TryAdd(nameof(MangaReaderWindows), windows);
                windows.Show();
            }
            else
            {
                MangaReaderWindow.Clear();
                windows = new MangaReaderWindows();
                action(windows);
                MangaReaderWindow.TryAdd(nameof(MangaReaderWindows), windows);
                windows.Show();
            }
        }
    }
}
