using CandySugar.App.Controls.Views.AcgAnime;
using CandySugar.Xam.Common;
using CandySugar.Xam.Common.DTO;
using CandySugar.Xam.Core.Service;
using Prism.Commands;
using Prism.Navigation;
using SDKColloction.AcgAnimeSDK;
using SDKColloction.AcgAnimeSDK.ViewModel;
using SDKColloction.AcgAnimeSDK.ViewModel.Enums;
using SDKColloction.AcgAnimeSDK.ViewModel.Request;
using SDKColloction.AcgAnimeSDK.ViewModel.Response;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using XF.Material.Forms.UI.Dialogs;

namespace CandySugar.App.Controls.ViewModels.AcgAnimeModel
{
    public class CandyAcgAnimeDetailViewModel : ViewModelNavigatBase
    {
        private readonly ILoger CandyLog;
        private readonly AcgAnimeProxy Proxy;
        public CandyAcgAnimeDetailViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = new AcgAnimeProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
            CandyLog = Resolve<ILoger>();
        }

        #region Property
        private ObservableCollection<AcgAnimePlayResult> _PlayResult;
        public ObservableCollection<AcgAnimePlayResult> PlayResult
        {
            get => _PlayResult;
            set => SetProperty(ref _PlayResult, value);
        }
        #endregion

        #region Override
        public override void Initialize(INavigationParameters parameters)
        {
            var input = parameters.GetValue<string>("Watch");
            Play(input);
        }
        #endregion

        #region Command
        public ICommand DetailCommand => new DelegateCommand<AcgAnimePlayResult>(input => Play(input.WatchRoute));
        public ICommand PlayCommand => new DelegateCommand<string>(input => PlayAcg(input));
        #endregion

        #region Method
        private async Task<string> Tip(string Method, Exception ex)
        {
            await CandyLog.Insert(new CandyGlobalLogDto
            {
                Location = $"{nameof(CandyAcgAnimeDetailViewModel)}_{Method}",
                ErrorMsg = ex.Message,
                ErrorStack = ex.StackTrace
            });
            return String.Format(Soft.Toast, nameof(CandyAcgAnimeDetailViewModel), Method);
        }

        public async void Play(string input)
        {
            try
            {
                var AcgPlay = await AcgAnimeFactory.AcgAnime(opt =>
                {
                    opt.RequestParam = new AcgAnimeRequestInput
                    {
                        AcgType = AcgAnimeEnum.AcgPlay,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.CacheTime,
                        Play = new AcgAnimePlay
                        {
                            Route = input
                        }
                    };
                }).RunsAsync();
                PlayResult = new ObservableCollection<AcgAnimePlayResult>(AcgPlay.PlayResults);

            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Play", ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }

        public async void PlayAcg(string input)
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("WatchAddress", input);
            await NavigationService.NavigateAsync(new Uri(nameof(CandyAcgAnimePlayView), UriKind.Relative), param);
        }
        #endregion
    }
}
