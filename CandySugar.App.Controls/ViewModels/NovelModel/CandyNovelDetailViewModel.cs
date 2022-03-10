using CandySugar.Xam.Common;
using Novel.SDK;
using Novel.SDK.ViewModel;
using Novel.SDK.ViewModel.Enums;
using Novel.SDK.ViewModel.Request;
using Novel.SDK.ViewModel.Response;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XF.Material.Forms.UI.Dialogs;

namespace CandySugar.App.Controls.ViewModels.NovelModel
{
    public class CandyNovelDetailViewModel : ViewModelNavigatBase
    {

        private readonly NovelProxy Proxy;
        public CandyNovelDetailViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = new NovelProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
            this.PageIndex = 1;
        }
        #region Field
        private string Route;
        #endregion

        #region Overrive

        public override void Initialize(INavigationParameters parameters)
        {
            Route = parameters.GetValue<NovelSearchResult>(nameof(NovelSearchResult)).DetailAddress;
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
        #endregion

        #region Command
        public ICommand RefreshsCommand => new DelegateCommand(() =>
        {
            PageIndex = 1;
            Details(Route, true);
        });

        public ICommand ShowMoreCommand => new DelegateCommand(() =>
        {
            PageIndex += 1;
            if (PageIndex <= NovelDetail.TotalPage)
                Details(Route, false, true);
        });

        public ICommand GoBackCommand => new DelegateCommand(async () => await NavigationService.GoBackAsync());

        public ICommand ItemSelectedCommand => new DelegateCommand<NovelDetailResults>(async input =>
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Route", input.ChapterURL);
            await NavigationService.NavigateAsync("CandyIndexView/CandyNovelDetailView/CandyNovelContentView", param);
        });
        #endregion

        #region Method
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
                            Page = this.PageIndex,
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
                        NovelDetail.Details.Details.ForEach(t =>
                        {
                            Chapter.Add(t);
                        });
                    }
                    else
                    {
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
                    Chapter = new ObservableCollection<NovelDetailResults>(NovelDetail.Details.Details);
                }
                else
                    this.IsBusy = false;
            }
            catch
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("网络有波动，请稍后再试~`(*>﹏<*)′"))
                {
                    await Task.Delay(3000);
                }
            }
        }
        #endregion
    }
}
