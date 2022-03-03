using CandySugar.App.Controls;
using CandySugar.Xam.Common.AppDTO;
using CandySugar.Xam.Common.Enum;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XExten.Advance.LinqFramework;

namespace CandySugar.App.ViewModels
{
    public class IndexViewModel : ViewModelNavigatBase
    {
        public IndexViewModel(INavigationService navigationService) : base(navigationService)
        {
            base.Title = "首页";
            this.Menu = MenuOption.InitMenu();
        }

        private ObservableCollection<MenuOption> _Menu;
        public ObservableCollection<MenuOption> Menu
        {
            get { return _Menu; }
            set { SetProperty(ref _Menu, value); }
        }
        private void ContentEvent(MenuOption obj)
        {
            
        }
        public ICommand ContentCommand => new Command<MenuOption>(input => {
            switch (input.CommandParam)
            {
                case MenuFuncEunm.Novel:
                    base.Title = input.CommandParam.ToDes();
                    break;
                case MenuFuncEunm.LightNovel:
                    base.Title = input.CommandParam.ToDes();
                    break;
                case MenuFuncEunm.Anime:
                    base.Title = input.CommandParam.ToDes();
                    break;
                case MenuFuncEunm.Manga:
                    base.Title = input.CommandParam.ToDes();
                    break;
                case MenuFuncEunm.Wallpaper:
                    base.Title = input.CommandParam.ToDes();
                    break;
                case MenuFuncEunm.Music:
                    base.Title = input.CommandParam.ToDes();
                    break;
                case MenuFuncEunm.Setting:
                    base.Title = input.CommandParam.ToDes();
                    break;
                case MenuFuncEunm.History:
                    base.Title = input.CommandParam.ToDes();
                    break;
                case MenuFuncEunm.About:
                    base.Title = input.CommandParam.ToDes();
                    break;
                default:
                    break;
            }
        });


    }
}
