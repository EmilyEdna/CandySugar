using CandySugar.CandyWindows.CnadyWinViewModel;
using CandySugar.Common;
using CandySugar.Common.Enum;
using CandySugar.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using XExten.Advance.StaticFramework;

namespace CandySugar.UserControlView
{
    /// <summary>
    /// MangaHeader.xaml 的交互逻辑
    /// </summary>
    public partial class MangaHeader : UserControl
    {
        public MangaHeader()
        {
            InitializeComponent();
        }
        private void CandySystemClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var types = (SysFuncEnum)btn.CommandParameter;
            switch (types)
            {
                case SysFuncEnum.Play:
                    break;
                case SysFuncEnum.Download:
                    Copy();
                    break;
                case SysFuncEnum.Setting:
                    break;
                case SysFuncEnum.MinSize:
                    Min();
                    break;
                case SysFuncEnum.Close:
                    Close();
                    break;
                default:
                    break;
            }
        }

        private void Min()
        {
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Visibility = Visibility.Collapsed;
            }
        }
        private void Close()
        {
            Window.GetWindow(this).Close();
        }
        private void Copy()
        {
            var vm = (Window.GetWindow(this).DataContext as CandyMangaReaderViewModel);
            var root = vm.Names;
            if (root != null)
            {
                FileInfo infomation = new FileInfo(root[0].ToString());
                var ChapterInfo = vm.Chapters.FirstOrDefault(t => t.TagKey == infomation.Directory.Name);
                var dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "LoteDown", "Manga", HelpUtilty.FileNameFilter(ChapterInfo.Name), ChapterInfo.Title));
                foreach (var item in root)
                {
                    FileInfo info = new FileInfo(item.ToString());
                    var files = SyncStatic.CreateFile(Path.Combine(dir, $"{info.Name}.jpg"));
                    info.CopyTo(files, true);
                }
                Process.Start("explorer.exe", dir);
            }

        }

        private void ThemeSelected(object sender, SelectionChangedEventArgs e)
        {
            var select = (sender as ComboBox);
            var item = (ThemeFuncEnum)(select.Items[select.SelectedIndex] as ComboBoxItem).TabIndex;
            switch (item)
            {
                case ThemeFuncEnum.BaoShilv:
                    Soft.Default.Theme = "#FF2CCFA0";
                    break;
                case ThemeFuncEnum.TaoHuafen:
                    Soft.Default.Theme = "#FFFF9999";
                    break;
                case ThemeFuncEnum.XuZilan:
                    Soft.Default.Theme = "#FF10AEC2";
                    break;
                case ThemeFuncEnum.ShanChahong:
                    Soft.Default.Theme = "#FFED556A";
                    break;
                case ThemeFuncEnum.MoYuhei:
                    Soft.Default.Theme = "#FF000000";
                    break;
                case ThemeFuncEnum.ZiShuhong:
                    Soft.Default.Theme = "#FFEF4289";
                    break;
                default:
                    break;
            }
        }
    }
}
