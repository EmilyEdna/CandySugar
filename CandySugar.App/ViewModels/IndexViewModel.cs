using CandySugar.App.Controls;
using CandySugar.Xam.Common.AppDTO;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CandySugar.App.ViewModels
{
    public class IndexViewModel : ViewModelNavigatBase
    {
        public IndexViewModel(INavigationService navigationService) : base(navigationService)
        {
            base.Title = "测试";
            this.Menu = MenuOption.InitMenu();
        }

        private ObservableCollection<MenuOption> _Menu;
        public ObservableCollection<MenuOption> Menu
        {
            get { return _Menu; }
            set { SetProperty(ref _Menu, value); }
        }

    }
}
