﻿using CandySugar.Common;
using CandySugar.Common.Enum;
using CandySugar.Common.Navigations;
using CandySugar.Common.WinDTO;
using CandySugar.Controls.UserControls;
using CandySugar.WPF.UserControlViews.AnimeViews;
using CandySugar.WPF.UserControlViews.AxgleViews;
using CandySugar.WPF.UserControlViews.LigthNovelViews;
using CandySugar.WPF.UserControlViews.MangaViews;
using CandySugar.WPF.UserControlViews.MusicViews;
using CandySugar.WPF.UserControlViews.NovelViews;
using CandySugar.WPF.UserControlViews.UserViews;
using CandySugar.WPF.UserControlViews.WallpaperViews;
using CandySugar.WPF.UserControlViews.AcgAnimeViews;
using HC = HandyControl.Controls;
using Serilog;
using Stylet;
using StyletIoC;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using XExten.Advance.LinqFramework;

namespace CandySugar.WPF.ViewModels
{
    public class RootViewModel : Conductor<IScreen>, IActiveNotify
    {
        public  IContainer Container;
        public RootViewModel(IContainer Container)
        {
            this.Container = Container;
            this.Menu = MenuOption.InitMenu();
            this.SoftName = $"甜糖V_{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        #region Property
        private ObservableCollection<MenuOption> _Menu;
        public ObservableCollection<MenuOption> Menu
        {
            get { return _Menu; }
            set { SetAndNotify(ref _Menu, value); }
        }
        private string _SoftName;
        public string SoftName
        {
            get { return _SoftName; }
            set { SetAndNotify(ref _SoftName, value); }
        }
        #endregion

        #region Action
        public void Redirect(MenuFuncEnum input)
        {
            switch (input)
            {
                case MenuFuncEnum.Novel:
                    ActivateItem(Container.Get<NovelViewModel>());
                    BootResource.Lyric(null, 2);
                    break;
                case MenuFuncEnum.LightNovel:
                    ActivateItem(Container.Get<LightNovelViewModel>());
                    BootResource.Lyric(null, 2);
                    break;
                case MenuFuncEnum.Anime:
                    ActivateItem(Container.Get<AnimeViewModel>());
                    BootResource.Lyric(null, 2);
                    break;
                case MenuFuncEnum.Manga:
                    ActivateItem(Container.Get<MangaViewModel>());
                    BootResource.Lyric(null, 2);
                    break;
                case MenuFuncEnum.Wallpaper:
                    ActivateItem(Container.Get<WallpaperViewModel>());
                    BootResource.Lyric(null, 2);
                    break;
                case MenuFuncEnum.Music:
                    ActivateItem(Container.Get<MusicViewModel>());
                    break;
                case MenuFuncEnum.Axgle:
                    ActivateItem(Container.Get<AxgleViewModel>());
                    BootResource.Lyric(null, 2);
                    break;
                case MenuFuncEnum.Hentai:
                    ActivateItem(Container.Get<AcgAnimeViewModel>());
                    BootResource.Lyric(null, 2);
                    break;
                case MenuFuncEnum.UserCenter:
                    ActivateItem(Container.Get<UserViewModel>());
                    BootResource.Lyric(null, 2);
                    break;
                default:
                    break;
            }
        }

        public void ShowWindow()
        {
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Visibility = Visibility.Visible;
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType().BaseType == typeof(CandyWindow) && window.Visibility == Visibility.Collapsed)
                        window.Visibility = Visibility.Visible;
                }
            }
        }

        public void NavigateTo(IScreen screen)
        {
            ActivateItem(screen);
        }
        #endregion

        protected override void OnViewLoaded()
        {
            ActivateItem(Container.Get<UserViewModel>());
            HelpUtilty.WirteLog("启动完成");
        }
    }
}
