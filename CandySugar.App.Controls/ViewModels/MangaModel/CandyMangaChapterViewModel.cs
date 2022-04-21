using CandySugar.Xam.Common;
using SDKColloction.MangaSDK;
using SDKColloction.MangaSDK.ViewModel;
using SDKColloction.MangaSDK.ViewModel.Emums;
using SDKColloction.MangaSDK.ViewModel.Request;
using SDKColloction.MangaSDK.ViewModel.Response;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XF.Material.Forms.UI.Dialogs;
using System.Linq;
using Prism.Ioc;
using CandySugar.App.Controls.Views.Manga;
using CandySugar.Xam.Core.Service;
using CandySugar.Xam.Common.DTO;
using XExten.Advance.LinqFramework;

namespace CandySugar.App.Controls.ViewModels.MangaModel
{
    public class CandyMangaChapterViewModel : ViewModelNavigatBase
    {
        public CandyMangaChapterViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = new MangaProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
            Refresh = false;
            CandyLog = Resolve<ILoger>();
        }
        #region Field
        private readonly ILoger CandyLog;
        private readonly MangaProxy Proxy;
        private string MangaName;
        private string Cover;
        #endregion

        #region Property
        private ObservableCollection<MangaChapterResult> _Chapters;
        public ObservableCollection<MangaChapterResult> Chapters
        {
            get { return _Chapters; }
            set { SetProperty(ref _Chapters, value); }
        }
        private bool _Refresh;
        public bool Refresh
        {
            get { return _Refresh; }
            set { SetProperty(ref _Refresh, value); }
        }
        #endregion

        #region Method
        private async Task<string> Tip(string Method, Exception ex)
        {
            await CandyLog.Insert(new CandyGlobalLogDto
            {
                Location = $"{nameof(CandyMangaChapterViewModel)}_{Method}",
                ErrorMsg = ex.Message,
                ErrorStack = ex.StackTrace
            });
            return String.Format(Soft.Toast, nameof(CandyMangaChapterViewModel), Method);
        }
        public async void Init(string input)
        {
            try
            {
                Refresh = true;
                var MangaDetail = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new MangaRequestInput
                    {
                        MangaType = MangaEnum.Detail,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.CacheTime,
                        Detail = new MangaDetail
                        {
                            Address = input
                        }
                    };
                }).RunsAsync();
                Refresh = false;
                Chapters = new ObservableCollection<MangaChapterResult>(MangaDetail.ChapterResults);
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Init", ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }
        public async void Navigation(MangaChapterResult input)
        {
            var dto = input.ToMapest<CandyMHLiShiDto>();
            dto.Cover = this.Cover;
            await Resolve<IMHLiShi>().InsertOrUpdate(dto);
            var Param = new NavigationParameters();
            Param.Add("Route", input.Address);
            await NavigationService.NavigateAsync(new Uri(nameof(CandyMangaReaderView), UriKind.Relative), Param);
        }
        #endregion

        #region Override
        public override void Initialize(INavigationParameters parameters)
        {
            this.MangaName = parameters.GetValue<string>("MangaName");
            this.Cover = parameters.GetValue<string>("Cover");
            Init(parameters.GetValue<string>("Route"));
        }
        #endregion

        #region Command
        public ICommand SelectedCommand => new DelegateCommand<dynamic>(input =>
        {
            if (input != null)
                Navigation(input);
        });
        public ICommand RefreshsCommand => new DelegateCommand(async () =>
        {
            Refresh = true;
            await Task.Delay(Soft.WaitSpan);
            Refresh = false;
        });
        #endregion
    }
}
