using CandySugar.App.Controls.Views.Novel;
using CandySugar.Xam.Common;
using CandySugar.Xam.Core.Service;
using SDKColloction.NovelSDK;
using SDKColloction.NovelSDK.ViewModel;
using SDKColloction.NovelSDK.ViewModel.Enums;
using SDKColloction.NovelSDK.ViewModel.Request;
using SDKColloction.NovelSDK.ViewModel.Response;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;
using XF.Material.Forms.UI.Dialogs;
using Prism.Ioc;
using CandySugar.Xam.Common.DTO;

namespace CandySugar.App.Controls.ViewModels.NovelModel
{
    public class CandyNovelDetailViewModel : ViewModelNavigatBase
    {

        private readonly NovelProxy Proxy;
        private readonly ILoger CandyLog;
        public CandyNovelDetailViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = new NovelProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
            CandyLog = Resolve<ILoger>();
            this.PageIndex = 1;
            this.Asc = true;
        }
        #region Field
        private string Route;
        private string BookName;
        #endregion

        #region Overrive

        public override void Initialize(INavigationParameters parameters)
        {
            var Param = parameters.GetValue<NovelSearchResult>(nameof(NovelSearchResult));
            Route = Param.DetailAddress;
            BookName = Param.BookName;
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var Param = parameters.GetValue<NovelSearchResult>(nameof(NovelSearchResult));

            Details(Param.DetailAddress, false, true);


        }
        #endregion

        #region Property
        private NovelDetailResult _NovelDetail;
        public NovelDetailResult NovelDetail
        {
            get { return _NovelDetail; }
            set { SetProperty(ref _NovelDetail, value); }
        }

        private ObservableCollection<NovelDetailResults> _Chapter;
        public ObservableCollection<NovelDetailResults> Chapter
        {
            get { return _Chapter; }
            set { SetProperty(ref _Chapter, value); }
        }

        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { SetProperty(ref _PageIndex, value); }
        }

        private int _TotalPage;
        public int TotalPage
        {
            get { return _TotalPage; }
            set { SetProperty(ref _TotalPage, value); }
        }
        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { SetProperty(ref _IsBusy, value); }
        }

        private bool _Refresh;
        public bool Refresh
        {
            get { return _Refresh; }
            set { SetProperty(ref _Refresh, value); }
        }
        private bool _Asc;
        public bool Asc
        {
            get { return _Asc; }
            set { SetProperty(ref _Asc, value); }
        }
        #endregion

        #region Command
        public ICommand RefreshsCommand => new DelegateCommand(() =>
        {
            if (this.Asc)
            {
                PageIndex = 1;
                Details(Route, true);
            }
            else {
                this.TotalPage = NovelDetail.TotalPage;
                Details(Route, true);
            }
        });

        public ICommand ShowMoreCommand => new DelegateCommand(() =>
        {
            if (this.Asc)
            {
                this.PageIndex += 1;
                if (this.PageIndex <= this.NovelDetail.TotalPage)
                    Details(Route, false, true);
            }
            else {
                this.TotalPage -= 1;
                if(this.TotalPage>=1)
                    Details(Route, false, true);
            }
        });

        public ICommand SortTypeCommand => new DelegateCommand<string>(input =>
        {
            this.Asc = bool.Parse(input);
            this.TotalPage = NovelDetail.TotalPage;
            Details(Route, true);
        });

        public ICommand ItemSelectedCommand => new DelegateCommand<NovelDetailResults>(async input =>
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("ChapterURL", input.ChapterURL);
            param.Add("BookName", BookName);
            await NavigationService.NavigateAsync(new Uri(nameof(CandyNovelContentView), UriKind.Relative), param);
        });
        #endregion

        #region Method
        private async Task<string> Tip(string Method, Exception ex)
        {
            await CandyLog.Insert(new CandyGlobalLogDto
            {
                Location = $"{nameof(CandyNovelDetailViewModel)}_{Method}",
                ErrorMsg = ex.Message,
                ErrorStack = ex.StackTrace
            });
            return String.Format(Soft.Toast,nameof(CandyNovelDetailViewModel), Method);
        }
        public async void Details(string input, bool IsRefresh = false, bool ShowMore = false)
        {
            try
            {
                if (IsRefresh)
                    this.Refresh = true;
                else
                    this.IsBusy = true;
                await Task.Delay(Soft.WaitSpan);
                var NovelDetail = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.CacheTime,
                        NovelType = NovelEnum.Detail,
                        Proxy = this.Proxy,
                        Detail = new NovelDetail
                        {
                            Page =this.Asc?this.PageIndex: this.TotalPage,
                            NovelDetailAddress = input
                        }
                    };
                }).RunsAsync();

                this.NovelDetail = NovelDetail.Details;

                if (ShowMore)
                {
                    if (Chapter == null)
                    {
                        Chapter = new ObservableCollection<NovelDetailResults>();
                        if (!this.Asc)
                            NovelDetail.Details.Details.Reverse();
                        NovelDetail.Details.Details.ForEach(t =>
                        {
                            Chapter.Add(t);
                        });
                    }
                    else
                    {
                        if (!this.Asc)
                            NovelDetail.Details.Details.Reverse();
                        NovelDetail.Details.Details.ForEach(t =>
                        {
                            Chapter.Add(t);
                        });
                    }
                }

                if (IsRefresh)
                {
                    Chapter.Clear();
                    this.Refresh = false;
                    if(!this.Asc)
                         NovelDetail.Details.Details.Reverse();
                    Chapter = new ObservableCollection<NovelDetailResults>(NovelDetail.Details.Details);
                }
                else
                    this.IsBusy = false;
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Details",ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }
        #endregion
    }
}
