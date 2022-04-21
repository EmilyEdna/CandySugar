using CandySugar.Controls.ControlViewModel;
using CandySugar.Core.CandyUtily;
using CandySugar.Controls.UserControls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using CandySugar.Common.Enum;
using System;
using XExten.Advance.StaticFramework;
using XExten.Advance.LinqFramework;
using System.Diagnostics;
using CandySugar.ViewModels;

namespace CandySugar.Views
{
    public partial class RootView : CandyWindow
    {
        public RootView()
        {
            InitializeComponent();
            Header.DataContext = CandyContainer.Instance.Resolves<BasicHeaderViewModel>().Basic();
        }

        private void WindowMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ProcessClick(object sender, RoutedEventArgs e)
        {
            var Icon = (TrayFuncEnum)(sender as MenuItem).CommandParameter;
            string dir = string.Empty;
            switch (Icon)
            {
                case TrayFuncEnum.Manga:
                    dir = SyncStatic.CreateDir(System.IO.Path.Combine(Environment.CurrentDirectory, "CandyDown", "Manga"));
                    break;
                case TrayFuncEnum.Music:
                    dir = SyncStatic.CreateDir(System.IO.Path.Combine(Environment.CurrentDirectory, "CandyDown", "Music"));
                    break;
                case TrayFuncEnum.Wallpaper:
                    dir = SyncStatic.CreateDir(System.IO.Path.Combine(Environment.CurrentDirectory, "CandyDown", "Wallpaper"));
                    break;
                case TrayFuncEnum.Novel:
                    dir = SyncStatic.CreateDir(System.IO.Path.Combine(Environment.CurrentDirectory, "CandyDown", "LightNovel"));
                    break;
                default:
                    BootResource.Clear();
                    this.Close();
                    Application.Current.Shutdown();
                    break;
            }
            if (!dir.IsNullOrEmpty())
                Process.Start("explorer.exe", dir);
        }

        private void LoadEvents(object sender, RoutedEventArgs e)
        {
            BootResource.Popup<BootView>();
        }
    }
}
