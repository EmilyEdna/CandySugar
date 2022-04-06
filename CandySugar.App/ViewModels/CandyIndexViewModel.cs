using CandySugar.App.Controls;
using CandySugar.App.Controls.Views;
using CandySugar.App.Controls.Views.Anime;
using CandySugar.App.Controls.Views.Konachan;
using CandySugar.App.Controls.Views.LightNovel;
using CandySugar.App.Controls.Views.Manga;
using CandySugar.App.Controls.Views.Novel;
using CandySugar.Xam.Common.AppDTO;
using CandySugar.Xam.Common.Enum;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using XExten.Advance.LinqFramework;
using Prism.Ioc;
using CandySugar.Xam.Core.Service;
using CandySugar.Xam.Common;
using CandySugar.App.Controls.Views.Option;
using CandySugar.App.Controls.Views.Axgle;
using CandySugar.App.Controls.Views.Music;

namespace CandySugar.App.ViewModels
{
    public class CandyIndexViewModel : ViewModelNavigatBase
    {
        public CandyIndexViewModel(INavigationService navigationService) : base(navigationService)
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

        private View _Views;
        public View Views
        {
            get { return _Views; }
            set { SetProperty(ref _Views, value); }
        }

        public ICommand ContentCommand => new Command<MenuOption>(input =>
        {
            switch (input.CommandParam)
            {
                case MenuFuncEnum.Novel:
                    base.Title = input.CommandParam.ToDes();
                    Arrived(nameof(CandyNovelView));
                    break;
                case MenuFuncEnum.LightNovel:
                    base.Title = input.CommandParam.ToDes();
                    Arrived(nameof(CandyLightNovelView));
                    break;
                case MenuFuncEnum.Anime:
                    base.Title = input.CommandParam.ToDes();
                    Arrived(nameof(CandyAnimeView));
                    break;
                case MenuFuncEnum.Manga:
                    base.Title = input.CommandParam.ToDes();
                    Arrived(nameof(CandyMangaView));
                    break;
                case MenuFuncEnum.Wallpaper:
                    base.Title = input.CommandParam.ToDes();
                    Arrived(nameof(CandyKonachanView));
                    break;
                case MenuFuncEnum.Axgle:
                    base.Title = input.CommandParam.ToDes();
                    Arrived(nameof(CandyAxgleView));
                    break;
                case MenuFuncEnum.Music:
                    base.Title = input.CommandParam.ToDes();
                    Arrived(nameof(CandyMusicView));
                    break;
                case MenuFuncEnum.Setting:
                    base.Title = input.CommandParam.ToDes();
                    Arrived(nameof(CandyOptionView));
                    break;
                case MenuFuncEnum.About:
                    base.Title = input.CommandParam.ToDes();
                    break;
                default:
                    break;
            }
        });

        private async void Arrived(string input)
        {
            await NavigationService.NavigateAsync(new Uri(input, UriKind.Relative));
        }

        protected override async void OnViewLaunch()
        {
            var option = await ContainerLocator.Container.Resolve<ISetting>().Query();

            Soft.AgeModule = option==null? Soft.AgeModule: option.AgeModule;
            Soft.CacheTime = option == null ? Soft.CacheTime : option.CacheTime;
            Soft.WaitSpan= option == null ? Soft.WaitSpan : option.WaitSpan;
            Soft.Blur=option == null ? Soft.Blur : option.Blur;
            Soft.ProxyAccount = option == null ? Soft.ProxyAccount : option.ProxyAccount;
            Soft.ProxyIP = option == null ? Soft.ProxyIP : option.ProxyIP;
            Soft.ProxyPort= option == null ? Soft.ProxyPort : option.ProxyPort;
            Soft.ProxyPwd =option == null ? Soft.ProxyPwd : option.ProxyPwd;

            RefreshView();
        }

        public void RefreshView()
        {
            Views = new CandyContentIndexView();
            base.Title = "首页";
        }
    }
}
