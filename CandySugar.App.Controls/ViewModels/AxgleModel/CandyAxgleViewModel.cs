using CandySugar.Xam.Common;
using GalActor.SDK;
using GalActor.SDK.ViewModel;
using GalActor.SDK.ViewModel.Eunms;
using GalActor.SDK.ViewModel.Request;
using GalActor.SDK.ViewModel.Response;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
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
            return String.Format(nameof(CandyAxgleViewModel), Method, Soft.Toast);
        }
        public async void Init() {
            try
            {
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
        #endregion

        #region Command
        #endregion
    }
}
