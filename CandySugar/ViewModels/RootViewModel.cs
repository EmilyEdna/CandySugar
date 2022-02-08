using CandySugar.Common.Enum;
using CandySugar.Common.Navigations;
using CandySugar.Common.WinDTO;
using CandySugar.UserControlViews.AnimeViews;
using CandySugar.UserControlViews.LigthNovelViews;
using CandySugar.UserControlViews.MangaViews;
using CandySugar.UserControlViews.MusicViews;
using CandySugar.UserControlViews.NovelViews;
using CandySugar.UserControlViews.UserViews;
using CandySugar.UserControlViews.WallpaperViews;
using Stylet;
using StyletIoC;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;

namespace CandySugar.ViewModels
{
    public class RootViewModel : Conductor<IScreen>, IActiveNotify
    {
        private readonly IContainer Container;
        public RootViewModel(IContainer Container)
        {
            this.Container = Container;
            this.Menu = MenuOption.InitMenu();
            this.SoftName= $"甜糖V_{Assembly.GetExecutingAssembly().GetName().Version}";
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
        public void Redirect(MenuFuncEunm input)
        {
            switch (input)
            {
                case MenuFuncEunm.Novel:
                    ActivateItem(Container.Get<NovelViewModel>());
                    break;
                case MenuFuncEunm.LightNovel:
                    ActivateItem(Container.Get<LightNovelViewModel>());
                    break;
                case MenuFuncEunm.Anime:
                    ActivateItem(Container.Get<AnimeViewModel>());
                    break;
                case MenuFuncEunm.Manga:
                    ActivateItem(Container.Get<MangaViewModel>());
                    break;
                case MenuFuncEunm.Wallpaper:
                    ActivateItem(Container.Get<WallpaperViewModel>());
                    break;
                case MenuFuncEunm.Music:
                    ActivateItem(Container.Get<MusicViewModel>());
                    break;
                case MenuFuncEunm.UserCenter:
                    ActivateItem(Container.Get<UserViewModel>());
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
            }
        }

        public void NavigateTo(IScreen screen)
        {
            ActivateItem(screen);
        }
        #endregion
    }
}
