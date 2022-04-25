using CandySugar.Xam.Common;
using CandySugar.Xam.Core.Service;
using SDKColloction.GalActorSDK;
using SDKColloction.GalActorSDK.ViewModel;
using SDKColloction.GalActorSDK.ViewModel.Eunms;
using SDKColloction.GalActorSDK.ViewModel.Request;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XF.Material.Forms.UI.Dialogs;
using Prism.Ioc;
using CandySugar.Xam.Common.DTO;

namespace CandySugar.App.Controls.ViewModels.AxgleModel
{
    public class CandyAxglePlayViewModel : ViewModelNavigatBase
    {
        private readonly ILoger CandyLog;
        public CandyAxglePlayViewModel(INavigationService navigationService) : base(navigationService)
        {
            CandyLog = Resolve<ILoger>();
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
        private async Task<string> Tip(string Method, Exception ex)
        {
            await CandyLog.Insert(new CandyGlobalLogDto
            {
                Location = $"{nameof(CandyAxglePlayViewModel)}_{Method}",
                ErrorMsg = ex.Message,
                ErrorStack = ex.StackTrace
            });
            return String.Format(Soft.Toast, nameof(CandyAxglePlayViewModel), Method);
        }
        public async void Init(string input) {
            try
            {
                var Detail = await GalActorFactory.GalActor(opt =>
                {
                    opt.RequestParam = new GalActorRequestInput
                    {
                        Galype = GalActorEnum.GalDetail,
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
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Init",ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }
        #endregion
    }
}
