using CandySugar.Common.Enum;
using CandySugar.Common.WinDTO;
using CandySugar.UserControlViews.AnimeViews;
using Stylet;
using StyletIoC;
using System.Collections.ObjectModel;

namespace CandySugar.ViewModels
{
    public class RootViewModel : Conductor<IScreen>
    {
        private readonly IContainer Container;
        public RootViewModel(IContainer Container)
        {
            this.Container = Container;
            this.Menu = MenuOption.InitMenu();
        }

        private ObservableCollection<MenuOption> _Menu;
        public ObservableCollection<MenuOption> Menu
        {
            get { return _Menu; }
            set { SetAndNotify(ref _Menu, value); }
        }

        public void Redirect(MenuFuncEunm input)
        {
            switch (input)
            {
                case MenuFuncEunm.Novel:
                    ActivateItem(Container.Get<AnimeViewModel>());
                    break;
                case MenuFuncEunm.LightNovel:
                    break;
                case MenuFuncEunm.Anime:
                    break;
                case MenuFuncEunm.Manga:
                    break;
                case MenuFuncEunm.Wallpaper:
                    break;
                case MenuFuncEunm.Music:
                    break;
                case MenuFuncEunm.UserCenter:
                    break;
                default:
                    break;
            }
        }
    }
}
