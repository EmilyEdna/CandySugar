using CandySugar.Common.Enum;
using CandySugar.Common.WinDTO;
using Stylet;
using StyletIoC;
using System.Collections.ObjectModel;

namespace CandySugar.ViewModels
{
    public class RootViewModel : Conductor<IScreen>
    {
        private readonly IContainer container;
        public RootViewModel(IContainer container)
        {
            this.container = container;
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
            var x = input;
        }
    }
}
