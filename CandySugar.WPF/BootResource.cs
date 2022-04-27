using CandySugar.WPF.CandyWindows;
using CandySugar.Common.WinDTO;
using CandySugar.Controls.UIElementHelper;
using CandySugar.Controls.UserControls;
using CandySugar.WPF.Properties;
using HandyControl.Controls;
using MaterialDesignThemes.Wpf;
using NAudio.Wave;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace CandySugar.WPF
{
    public class BootResource
    {
        private static ConcurrentDictionary<string, CandyMangaReaderWin> MangaReaderWindow = new ConcurrentDictionary<string, CandyMangaReaderWin>();
        private static ConcurrentDictionary<string, CandyVLCWin> AnimeVLCWindow = new ConcurrentDictionary<string, CandyVLCWin>();
        private static ConcurrentDictionary<string, CandyDPlayWin> AnimeDPlayWindow = new ConcurrentDictionary<string, CandyDPlayWin>();
        private static ConcurrentDictionary<string, CandyNovelWin> CandyNovelWindow = new ConcurrentDictionary<string, CandyNovelWin>();
        private static ConcurrentDictionary<string, CandyLightNovelWin> CandyLightNovelWindow = new ConcurrentDictionary<string, CandyLightNovelWin>();
        private static ConcurrentDictionary<string, CandyLyricWin> CandyLyricWindow = new ConcurrentDictionary<string, CandyLyricWin>();
        private static ConcurrentDictionary<string, CandyPreviewWin> CandyPreviewWindow = new ConcurrentDictionary<string, CandyPreviewWin>();
        private static ConcurrentDictionary<string, CandyAxgleWin> AxgleWindow = new ConcurrentDictionary<string, CandyAxgleWin>();
        private static ConcurrentDictionary<string,CandyAcgProviderWin> AcgProviderWindown = new ConcurrentDictionary<string,CandyAcgProviderWin>();

        #region 属性
        /// <summary>
        /// 播放定时器
        /// </summary>
        public static Timer Timer { get; set; }

        /// <summary>
        /// 歌词定时器
        /// </summary>
        public static Timer LyricTimer { get; set; }

        /// <summary>
        /// NAudio
        /// </summary>
        public static WaveOutEvent Wave { get; set; }

        /// <summary>
        /// Reader
        /// </summary>
        public static MediaFoundationReader Reader { get; set; }

        #endregion

        #region 保存配置
        /// <summary>
        /// 设置全局用户配置的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        internal static void SetSettingPropertyValue<T>(T param)
        {
            param.GetType().GetProperties().ForEnumerEach(t =>
            {
                Soft.Default.GetType().GetProperty(t.Name).SetValue(Soft.Default, t.GetValue(param));
            });
            Soft.Default.Save();
            SaveUserSetting();
        }

        internal static void SaveUserSetting()
        {
            RootOption root = Soft.Default.ToJson().ToModel<RootOption>();

            var XmlData = Encoding.UTF8.GetBytes(SyncStatic.XmlSerializer(root));
            SyncStatic.DeleteFolder(Path.Combine(Environment.CurrentDirectory, "UserSetting"));
            var Dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "UserSetting"));
            var File = SyncStatic.CreateFile(Path.Combine(Dir, "UserSetting.xml"));

            SyncStatic.WriteFile(XmlData, File);
        }

        internal static void ReadUserSetting()
        {
            var Dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "UserSetting"));
            var Xml = SyncStatic.ReadFile(Path.Combine(Dir, "UserSetting.xml"));
            if (Xml != null)
            {
                var setting = SyncStatic.XmlDeserialize<RootOption>(Xml);
                setting.GetType().GetProperties().ForEnumerEach(t =>
                {
                    Soft.Default.GetType().GetProperty(t.Name).SetValue(Soft.Default, t.GetValue(setting));
                });
                Soft.Default.Save();
            }
        }
        #endregion

        #region 窗体控制
        /// <summary>
        /// 控制H窗体
        /// </summary>
        /// <param name="action"></param>
        public static void AcgView(Action<CandyAcgProviderWin> action) 
        {
            CandyAcgProviderWin windows = null;
            if (AcgProviderWindown.ContainsKey(nameof(CandyAcgProviderWin)))
            {
                var old = AcgProviderWindown.Values.FirstOrDefault();
                old.Close();
                AcgProviderWindown.Clear();
                windows = new CandyAcgProviderWin();
                action(windows);
                var binding = new Binding
                {
                    Source = windows.DataContext,
                    //绑定到附加属性
                    Path = new PropertyPath("Loading"),
                    Mode = BindingMode.TwoWay
                };
                var Zone = windows.Header.Content as ColorZone;
                Zone.CornerRadius = new CornerRadius(8, 8, 0, 0);
                var content = Zone.Content as Grid;
                UlHelper.FindVisualChild<LoadingLine>(content).FirstOrDefault().SetBinding(UIElement.VisibilityProperty, binding);
                AcgProviderWindown.TryAdd(nameof(CandyAcgProviderWin), windows);
                windows.Show();
            }
            else
            {
                AcgProviderWindown.Clear();
                windows = new CandyAcgProviderWin();
                action(windows);
                var binding = new Binding
                {
                    Source = windows.DataContext,
                    //绑定到附加属性
                    Path = new PropertyPath("Loading"),
                    Mode = BindingMode.TwoWay
                };
                var Zone = windows.Header.Content as ColorZone;
                Zone.CornerRadius = new CornerRadius(8, 8, 0, 0);
                var content = Zone.Content as Grid;
                UlHelper.FindVisualChild<LoadingLine>(content).FirstOrDefault().SetBinding(UIElement.VisibilityProperty, binding);
                AcgProviderWindown.TryAdd(nameof(CandyAcgProviderWin), windows);
                windows.Show();
            }
        }
        /// <summary>
        /// 控制预览窗体打开
        /// </summary>
        /// <param name="action"></param>
        public static void View(Action<CandyPreviewWin> action)
        {
            CandyPreviewWin windows = null;
            if (CandyPreviewWindow.ContainsKey(nameof(CandyPreviewWin)))
            {
                var old = CandyPreviewWindow.Values.FirstOrDefault();
                old.Close();
                CandyPreviewWindow.Clear();
                windows = new CandyPreviewWin();
                action(windows);
                var binding = new Binding
                {
                    Source = windows.DataContext,
                    //绑定到附加属性
                    Path = new PropertyPath("Loading"),
                    Mode = BindingMode.TwoWay
                };
                var content = (windows.Header.Content as ColorZone).Content as Grid;
                UlHelper.FindVisualChild<LoadingLine>(content).FirstOrDefault().SetBinding(UIElement.VisibilityProperty, binding);
                CandyPreviewWindow.TryAdd(nameof(CandyPreviewWin), windows);
                windows.Show();
            }
            else
            {
                CandyPreviewWindow.Clear();
                windows = new CandyPreviewWin();
                action(windows);
                var binding = new Binding
                {
                    Source = windows.DataContext,
                    //绑定到附加属性
                    Path = new PropertyPath("Loading"),
                    Mode = BindingMode.TwoWay
                };
                var content = (windows.Header.Content as ColorZone).Content as Grid;
                UlHelper.FindVisualChild<LoadingLine>(content).FirstOrDefault().SetBinding(UIElement.VisibilityProperty, binding);
                CandyPreviewWindow.TryAdd(nameof(CandyPreviewWin), windows);
                windows.Show();
            }
        }
        /// <summary>
        /// 控制漫画窗体打开
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
                    Mode = BindingMode.TwoWay
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
        /// <summary>
        /// 控制小说窗体打开
        /// </summary>
        /// <param name="action"></param>
        public static void Novel(Action<CandyNovelWin> action)
        {
            CandyNovelWin windows = null;
            if (CandyNovelWindow.ContainsKey(nameof(CandyNovelWin)))
            {
                var old = CandyNovelWindow.Values.FirstOrDefault();
                old.Close();
                CandyNovelWindow.Clear();
                windows = new CandyNovelWin();
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
                CandyNovelWindow.TryAdd(nameof(CandyNovelWin), windows);
                windows.Show();
            }
            else
            {
                CandyNovelWindow.Clear();
                windows = new CandyNovelWin();
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
                CandyNovelWindow.TryAdd(nameof(CandyNovelWin), windows);
                windows.Show();
            }
        }
        /// <summary>
        /// 控制轻小说窗体打开
        /// </summary>
        /// <param name="action"></param>
        public static void LightNovel(Action<CandyLightNovelWin> action)
        {
            CandyLightNovelWin windows = null;
            if (CandyLightNovelWindow.ContainsKey(nameof(CandyLightNovelWin)))
            {
                var old = CandyLightNovelWindow.Values.FirstOrDefault();
                old.Close();
                CandyLightNovelWindow.Clear();
                windows = new CandyLightNovelWin();
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
                CandyLightNovelWindow.TryAdd(nameof(CandyLightNovelWin), windows);
                windows.Show();
            }
            else
            {
                CandyLightNovelWindow.Clear();
                windows = new CandyLightNovelWin();
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
                CandyLightNovelWindow.TryAdd(nameof(CandyLightNovelWin), windows);
                windows.Show();
            }
        }
        /// <summary>
        /// 控制VLC窗体打开
        /// </summary>
        /// <param name="action"></param>
        public static void AnimeVLC(Action<CandyVLCWin> action)
        {
            CandyVLCWin windows = null;
            if (AnimeVLCWindow.ContainsKey(nameof(CandyVLCWin)))
            {
                var old = AnimeVLCWindow.Values.FirstOrDefault();
                old.Close();
                AnimeVLCWindow.Clear();
                windows = new CandyVLCWin();
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
                AnimeVLCWindow.TryAdd(nameof(CandyVLCWin), windows);
                windows.Show();
            }
            else
            {
                AnimeVLCWindow.Clear();
                windows = new CandyVLCWin();
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
                AnimeVLCWindow.TryAdd(nameof(CandyVLCWin), windows);
                windows.Show();
            }
        }
        /// <summary>
        /// 控制DPlay窗体打开
        /// </summary>
        /// <param name="action"></param>
        public static void AnimeWEB(Action<CandyDPlayWin> action)
        {
            CandyDPlayWin windows = null;
            if (AnimeDPlayWindow.ContainsKey(nameof(CandyDPlayWin)))
            {
                var old = AnimeDPlayWindow.Values.FirstOrDefault();
                old.Close();
                AnimeDPlayWindow.Clear();
                windows = new CandyDPlayWin();
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
                AnimeDPlayWindow.TryAdd(nameof(CandyDPlayWin), windows);
                windows.Show();
            }
            else
            {
                AnimeDPlayWindow.Clear();
                windows = new CandyDPlayWin();
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
                AnimeDPlayWindow.TryAdd(nameof(CandyDPlayWin), windows);
                windows.Show();
            }
        }
        /// <summary>
        /// 控制Axgle窗体打开
        /// </summary>
        /// <param name="action"></param>
        public static void AxgleWEB(Action<CandyAxgleWin> action)
        {
            CandyAxgleWin windows = null;
            if (AxgleWindow.ContainsKey(nameof(CandyAxgleWin)))
            {
                var old = AxgleWindow.Values.FirstOrDefault();
                old.Close();
                AxgleWindow.Clear();
                windows = new CandyAxgleWin();
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
                AxgleWindow.TryAdd(nameof(CandyDPlayWin), windows);
                windows.Show();
            }
            else
            {
                AxgleWindow.Clear();
                windows = new CandyAxgleWin();
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
                AxgleWindow.TryAdd(nameof(CandyDPlayWin), windows);
                windows.Show();
            }
        }
        /// <summary>
        /// 控制歌词窗体
        /// </summary>
        /// <param name="action"></param>
        /// <param name="Root"></param>
        public static int Lyric(Action<CandyLyricWin> action, int Root = 1)
        {

            CandyLyricWin windows = null;
            if (Root == 1)
            {
                if (CandyLyricWindow.ContainsKey(nameof(CandyLyricWin)))
                {
                    var old = CandyLyricWindow.Values.FirstOrDefault();
                    old.Close();
                    CandyLyricWindow.Clear();
                }
                else
                {
                    CandyLyricWindow.Clear();
                    windows = new CandyLyricWin();
                    action(windows);
                    windows.WindowStartupLocation = WindowStartupLocation.Manual;
                    windows.Top = (SystemParameters.PrimaryScreenHeight / 10) * 7.5;
                    windows.Left = (SystemParameters.PrimaryScreenWidth / 10) * 1.9;
                    windows.Topmost = true;
                    CandyLyricWindow.TryAdd(nameof(CandyLyricWin), windows);
                    windows.Show();
                }
            }
            if (Root == 2)
            {
                if (CandyLyricWindow.ContainsKey(nameof(CandyLyricWin)))
                {
                    var old = CandyLyricWindow.Values.FirstOrDefault();
                    old.Close();
                    CandyLyricWindow.Clear();
                    Clear();
                }
            }
            if (Root == 3)
            {
                CandyLyricWindow.Clear();
                windows = new CandyLyricWin();
                action(windows);
                windows.WindowStartupLocation = WindowStartupLocation.Manual;
                windows.Top = (SystemParameters.PrimaryScreenHeight / 10) * 7.5;
                windows.Left = (SystemParameters.PrimaryScreenWidth / 10) * 1.9;
                windows.Topmost = true;
                CandyLyricWindow.TryAdd(nameof(CandyLyricWin), windows);
                windows.Show();
            }

            return CandyLyricWindow.Count;
        }
        #endregion

        #region 弹窗控制
        public static bool Popup<T>(Action<T> action=null) where T : CandyWindow, new()
        {
            T popup = new T();
            popup.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            action?.Invoke(popup);
            return (bool)popup.ShowDialog();
        }
        #endregion

        #region 清理
        public static void Clear()
        {
            //清理
            Wave?.Stop();
            Wave?.Dispose();
            Timer?.Close();
            LyricTimer?.Close();
        }
        #endregion
    }
}
