using CandySugar.CandyWindows;
using CandySugar.CandyWindows.CnadyWinViewModel;
using CandySugar.Common;
using CandySugar.Common.Enum;
using CandySugar.Controls.ControlViewModel;
using CandySugar.Properties;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XExten.Advance.StaticFramework;

namespace CandySugar.UserControlView.UserControlEvent
{
    public class UserBaseControl : UserControl
    {
        public virtual void CandySystemClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var types = (SysFuncEnum)btn.CommandParameter;
            switch (types)
            {
                case SysFuncEnum.Play:
                    Play();
                    break;
                case SysFuncEnum.Download:
                    Download();
                    break;
                case SysFuncEnum.Setting:
                    Setting();
                    break;
                case SysFuncEnum.MinSize:
                    MinSize();
                    break;
                case SysFuncEnum.Close:
                    Close();
                    break;
                case SysFuncEnum.ShutDown:
                    ShutDown();
                    break;
                default:
                    break;
            }
        }

        #region Event
        private void Play()
        {
            var win = Window.GetWindow(this);
            WebView2 webView = (win.FindName("webView") as WebView2);
            webView.CoreWebView2.ExecuteScriptAsync($"Play('{(win.DataContext as dynamic).WatchResult.PlayURL}')");
        }
        private void Close()
        {
            var win = Window.GetWindow(this);
            WebView2 webView = null;
            var webViews = win.FindName("webView");
            if (webViews != null)
            {
                webView = (WebView2)webViews;
                webView.CoreWebView2.ExecuteScriptAsync($"Destory()");
            }

            if (win is CandyVLCWin)
            {
                var VLCWindow = ((CandyVLCWin)win);
                VLCWindow.MediaPlayers?.Dispose();
                VLCWindow.LibVlcs?.Dispose();
                VLCWindow.Timer?.Stop();
                VLCWindow.Close();
            }

            win.Close();
        }
        private void Download()
        {
            var vm = (Window.GetWindow(this).DataContext as CandyMangaReaderViewModel);
            var root = vm.Names;
            if (root != null)
            {
                FileInfo infomation = new FileInfo(root[0].ToString());
                var ChapterInfo = vm.Chapters.FirstOrDefault(t => t.TagKey == infomation.Directory.Name);
                var dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "CandyDown", "Manga", HelpUtilty.FileNameFilter(ChapterInfo.Name), ChapterInfo.Title));
                foreach (var item in root)
                {
                    FileInfo info = new FileInfo(item.ToString());
                    var files = SyncStatic.CreateFile(Path.Combine(dir, $"{info.Name}.jpg"));
                    info.CopyTo(files, true);
                }
                Process.Start("explorer.exe", dir);
            }

        }
        private void ShutDown()
        {
            BootResource.Clear();
            Application.Current.Shutdown();
        }
        private void Setting()
        {
            CandySettingWin win = new CandySettingWin
            {
                DataContext = CandyViewModule.Container.Get<CandySettingViewModel>()
            };
            win.Show();
        }
        private void MinSize()
        {
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Visibility = Visibility.Collapsed;
            }
            Window.GetWindow(this).Visibility = Visibility.Collapsed;
        }
        #endregion

        public virtual void ThemeSelected(object sender, SelectionChangedEventArgs e)
        {
            var select = (sender as ComboBox);
            var item = (ThemeFuncEnum)(select.Items[select.SelectedIndex] as ComboBoxItem).TabIndex;
            switch (item)
            {
                case ThemeFuncEnum.BaoShilv:
                    SetTheme("#FF2CCFA0");
                    break;
                case ThemeFuncEnum.TaoHuafen:
                    SetTheme("#FFFF9999");
                    break;
                case ThemeFuncEnum.XuZilan:
                    SetTheme("#FF10AEC2");
                    break;
                case ThemeFuncEnum.ShanChahong:
                    SetTheme("#FFED556A");
                    break;
                case ThemeFuncEnum.MoYuhei:
                    SetTheme("#FF000000");
                    break;
                case ThemeFuncEnum.ZiShuhong:
                    SetTheme("#FFEF4289");
                    break;
                default:
                    break;
            }
        }
        private void SetTheme(string Colors)
        {
            Soft.Default.Theme = Colors;
            Soft.Default.Save();
        }
    }
}
