using CandySugar.App.Controls.Views.Axgle;
using CandySugar.Xam.Common;
using GalActor.SDK;
using GalActor.SDK.ViewModel;
using GalActor.SDK.ViewModel.Eunms;
using GalActor.SDK.ViewModel.Request;
using GalActor.SDK.ViewModel.Response;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XExten.Advance.LinqFramework;
using XF.Material.Forms.UI.Dialogs;

namespace CandySugar.App.Controls.ViewModels.AxgleModel
{
    public class CandyAxgleViewModel : ViewModelNavigatBase
    {
        private readonly GalActorProxy Proxy;
        public CandyAxgleViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = new GalActorProxy
            {
                IP = Soft.ProxyIP,
                PassWord = Soft.ProxyPwd,
                Port = Soft.ProxyPort,
                UserName = Soft.ProxyAccount
            };
        }

        #region Property
        private ObservableCollection<GalActorCategory> _Categories;
        public ObservableCollection<GalActorCategory> Categories
        {
            get { return _Categories; }
            set { SetProperty(ref _Categories, value); }
        }
        private bool _Refresh;
        public bool Refresh
        {
            get { return _Refresh; }
            set { SetProperty(ref _Refresh, value); }
        }
        #endregion

        #region Override
        protected override void OnViewLaunch()
        {
            Init();
        }
        #endregion

        #region Methond
        private string Tip(string Method)
        {
            return String.Format(Soft.Toast,nameof(CandyAxgleViewModel), Method);
        }
        public async void Init()
        {
            try
            {
                this.Refresh = true;
                var Init = await GalActorFactory.GalActor(opt =>
                {
                    opt.RequestParam = new GalActorRequestInput
                    {
                        Galype = GalActorEnum.Init,
                        Proxy = this.Proxy,
                        GalInit = new GalActorInit(),
                        CacheSpan = Soft.CacheTime
                    };
                }).RunsAsync();
                this.Refresh = false;
                Categories = new ObservableCollection<GalActorCategory>(Init.CategoryResults);
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(Tip("Init")))
                {
                    await Task.Delay(3000);
                }
            }
        }
        public async void Navigation(string AId)
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("AId", AId);
            await NavigationService.NavigateAsync(new Uri(nameof(CandyAxgleCateView), UriKind.Relative), param);
        }
        #endregion

        #region Command
        public ICommand CategoryCommand => new DelegateCommand<string>(input =>
        {
            if (!input.IsNullOrEmpty())
                Navigation(input);
        });
        public ICommand RefreshsCommand => new DelegateCommand(async () =>
        {
            this.Refresh = true;
            await Task.Delay(Soft.WaitSpan);
            this.Refresh = false;
        });
        #endregion
    }
}
