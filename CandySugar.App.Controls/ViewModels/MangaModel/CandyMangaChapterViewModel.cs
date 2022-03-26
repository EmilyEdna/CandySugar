using CandySugar.Xam.Common;
using Manga.SDK;
using Manga.SDK.ViewModel;
using Manga.SDK.ViewModel.Emums;
using Manga.SDK.ViewModel.Request;
using Manga.SDK.ViewModel.Response;
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
using CandySugar.App.Controls.Views.Manga;

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
        }
        #region Field
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
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(Soft.Toast))
                {
                    await Task.Delay(3000);
                }
            }
        }
        public async void Navigation(MangaChapterResult input)
        {
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
