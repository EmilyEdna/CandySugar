﻿using CandySugar.CandyWindows;
using CandySugar.Controls.UIElementHelper;
using CandySugar.Properties;
using HandyControl.Controls;
using MaterialDesignThemes.Wpf;
using NAudio.Wave;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XExten.Advance.LinqFramework;


namespace CandySugar
{
    public class BootResource
    {
        /// <summary>
        /// 播放定时器
        /// </summary>
        public static Timer Timer
        {
            get; set;
        }
        /// <summary>
        /// 歌词定时器
        /// </summary>
        public static Timer LyricTimer
        {
            get; set;
        }
        /// <summary>
        /// NAudio
        /// </summary>
        public static WaveOutEvent Wave
        {
            get; set;
        }
        /// <summary>
        /// Reader
        /// </summary>
        public static MediaFoundationReader Reader
        {
            get; set;
        }


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
            Soft.Default.Save();
        }

        public static void SaveSettingProerty()
        {
            Soft.Default.Save();
        }

        private static ConcurrentDictionary<string, CandyMangaReaderWin> MangaReaderWindow = new ConcurrentDictionary<string, CandyMangaReaderWin>();
        private static ConcurrentDictionary<string, CandyVLCWin> AnimeVLCWindow = new ConcurrentDictionary<string, CandyVLCWin>();
        private static ConcurrentDictionary<string, CandyDPlayWin> AnimeDPlayWindow = new ConcurrentDictionary<string, CandyDPlayWin>();
        private static ConcurrentDictionary<string, CandyNovelWin> CandyNovelWindow = new ConcurrentDictionary<string, CandyNovelWin>();
        private static ConcurrentDictionary<string, CandyLightNovelWin> CandyLightNovelWindow = new ConcurrentDictionary<string, CandyLightNovelWin>();
        private static ConcurrentDictionary<string, CandyLyricWin> CandyLyricWindow = new ConcurrentDictionary<string, CandyLyricWin>();
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
        public static void Clear()
        {
            //清理
            Wave?.Stop();
            Wave?.Dispose();
            Timer?.Close();
            LyricTimer?.Close();
        }
    }


}