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
using CandySugar.App.Controls.Views.About;
using CandySugar.App.Controls.Views.Logger;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;
using Syncfusion.XForms.BadgeView;
using CandySugar.Xam.Common.Platform;
using CandySugar.Xam.Common.CrossDownManager;
using XF.Material.Forms.UI;
using System.Threading.Tasks;

namespace CandySugar.App.ViewModels
{
    public class CandyIndexViewModel : ViewModelNavigatBase
    {
        public CandyIndexViewModel(INavigationService navigationService) : base(navigationService)
        {
            base.Title = "首页";
            this.CurrentVersion = "New";
            this.Badge = BadgeType.Error;
            this.Menu = MenuOption.InitMenu();
        }
        #region Property
        private ObservableCollection<MenuOption> _Menu;
        public ObservableCollection<MenuOption> Menu
        {
            get { return _Menu; }
            set { SetProperty(ref _Menu, value); }
        }
        private string _Version;
        public string Version
        {
            get => _Version;
            set =>SetProperty(ref _Version, value);
        }
        private string _CurrentVersion;
        public string CurrentVersion
        {
            get => _CurrentVersion;
            set => SetProperty(ref _CurrentVersion, value);
        }
        private View _Views;
        public View Views
        {
            get { return _Views; }
            set { SetProperty(ref _Views, value); }
        }
        private BadgeType _Badge;
        public BadgeType Badge
        {
            get => _Badge;
            set => SetProperty(ref _Badge, value);
        }
        #endregion

        #region Command
        public ICommand ContentCommand => new Command<MenuOption>(input =>
        {
            GotoContent(input);
        });
        #endregion

        #region Override
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
            Version = Extension.VersionCode;
            RefreshView();
            CheckVersion();
        }
        #endregion

        #region Method
        public async void GotoContent(MenuOption input) 
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
                    if (await PINK())
                        Arrived(nameof(CandyKonachanView));
                    break;
                case MenuFuncEnum.Axgle:
                    base.Title = input.CommandParam.ToDes();
                    if(await PINK())
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
                case MenuFuncEnum.Loger:
                    base.Title = input.CommandParam.ToDes();
                    Arrived(nameof(CandyLogView));
                    break;
                case MenuFuncEnum.About:
                    base.Title = input.CommandParam.ToDes();
                    Arrived(nameof(CandyAboutView));
                    break;
                default:
                    break;
            }
        }

        public async Task<bool> PINK() 
        {
            if (Soft.PINK) return true;
            else {
                var res = await MaterialDialog.Instance.InputAsync("提示", "请输入PIN码", null, null, "确定", "取消", new MaterialInputDialogConfiguration
                {
                    InputType = MaterialTextFieldInputType.Password,
                    TintColor = Color.FromHex("FF9999"),
                    CornerRadius = 8,
                    ButtonAllCaps = false
                });
                if (res.ToUpper().Equals("EMILY"))
                {
                    Soft.PINK= true;
                    return true;
                }
                else return false;
            }
        }
        public async void Arrived(string input)
        {
            await NavigationService.NavigateAsync(new Uri(input, UriKind.Relative));
        }
        public void RefreshView()
        {
            Views = new CandyContentIndexView();
            base.Title = "首页";
        }
        public  async void CheckVersion()
        {
            if (Extension.CheckVersion())
            {
                var result =await  MaterialDialog.Instance.ConfirmAsync("检测到新版本，是否进行升级!", "提示", "确定", "取消", new MaterialAlertDialogConfiguration
                {
                    TintColor = Color.FromHex("FF9999"),
                    CornerRadius = 8,
                    ButtonAllCaps = false
                }) ;

                if (result.HasValue && result.Value)
                {
                    var Platform = ContainerLocator.Container.Resolve<IAndroidPlatform>();
                    //升级
                    var manager = Platform.UpdateApk();
                    var File = manager.CreateDownloadFile("https://ghproxy.com/https://github.com/EmilyEdna/KuRuMi/releases/download/1.0/CandySugar.apk");
                    File.PropertyChanged += (sender, obj) =>
                    {
                        var IsCompleted = ((IDownloadFile)sender).Status == DownloadFileStatus.COMPLETED;
                        if (obj.PropertyName == "Status" && IsCompleted)
                        {
                            Platform.InstallApk();
                        }
                    };
                    AuthorizeHelper.Instance.ApplyPermission(() =>
                    {
                        manager.Start(File);
                    });

                }
            }
            else {
                this.CurrentVersion = "Cur";
                this.Badge = BadgeType.Primary;
            }
        }
        #endregion
    }
}
