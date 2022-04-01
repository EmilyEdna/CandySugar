using CandySugar.Xam.Common;
using GalActor.SDK;
using GalActor.SDK.ViewModel;
using GalActor.SDK.ViewModel.Eunms;
using GalActor.SDK.ViewModel.Request;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XF.Material.Forms.UI.Dialogs;

namespace CandySugar.App.Controls.ViewModels.AxgleModel
{
    public class CandyAxglePlayViewModel : ViewModelNavigatBase
    {
        public CandyAxglePlayViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region Property
        private string _PlayURL;
        public string PlayURL
        {
            get { return _PlayURL; }
            set { SetProperty(ref _PlayURL, value); }
        }
        #endregion

        #region Override
        public override void Initialize(INavigationParameters parameters)
        {
            Init(parameters.GetValue<string>("Route"));
        }
        #endregion

        #region Method
        private string Tip(string Method)
        {
            return String.Format(Soft.Toast, nameof(CandyAxglePlayViewModel), Method);
        }
        public async void Init(string input) {
            try
            {
                var Detail = await GalActorFactory.GalActor(opt =>
                {
                    opt.RequestParam = new GalActorRequestInput
                    {
                        Galype = GalActorEnum.Detail,
                        CacheSpan = Soft.CacheTime,
                         Detail = new  GalActorDetail
                        {
                           IFrameURL= input
                         }
                    };
                }).RunsAsync();
                PlayURL = Detail.PlayResult.Play;
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
    }
}
