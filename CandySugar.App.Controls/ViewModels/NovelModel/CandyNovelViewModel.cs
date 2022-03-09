using CandySugar.Xam.Common;
using Novel.SDK;
using Novel.SDK.ViewModel;
using Novel.SDK.ViewModel.Enums;
using Novel.SDK.ViewModel.Request;
using Novel.SDK.ViewModel.Response;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using XExten.Advance.LinqFramework;
using XF.Material.Forms.UI.Dialogs;

namespace CandySugar.App.Controls.ViewModels.NovelModel
{
    public class CandyNovelViewModel : ViewModelBase
    {
        private readonly NovelProxy Proxy;
        public CandyNovelViewModel() : base()
        {
            Proxy = new NovelProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
        }

        #region Override
        protected override async void OnViewLaunch()
        {
            try
            {
                var NovelInit = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.CacheTime,
                        NovelType = NovelEnum.Init,
                        Proxy = this.Proxy
                    };
                }).RunsAsync();
                this.NovelCategory = new ObservableCollection<NovelCategoryResult>(NovelInit.IndexCategories);
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

        #region Property
        private ObservableCollection<NovelCategoryResult> _NovelCategory;
        public ObservableCollection<NovelCategoryResult> NovelCategory
        {
            get { return _NovelCategory; }
            set { SetProperty(ref _NovelCategory, value); }
        }

        private ObservableCollection<NovelSearchResult> _NovelSearch;
        public ObservableCollection<NovelSearchResult> NovelSearch
        {
            get { return _NovelSearch; }
            set { SetProperty(ref _NovelSearch, value); }
        }

        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { SetProperty(ref _PageIndex, value); }
        }

        private int _Total;
        public int Total
        {
            get { return _Total; }
            set { SetProperty(ref _Total, value); }
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

        #region Field
        private string CategoryType;
        #endregion

        #region Command
        public ICommand RefreshsCommand => new DelegateCommand(() =>
        {
            PageIndex = 1;
            Refreshs(CategoryType);
        });
        public ICommand SearchCommand => new DelegateCommand<dynamic>(input =>
        {
            PageIndex = 1;
            Search(input);
        });

        public ICommand ItemCommand => new DelegateCommand<string>(input =>
        {
            PageIndex = 1;
            Category(input, false);
        });

        public ICommand ShowMoreCommand => new DelegateCommand(() =>
        {
            PageIndex += 1;
            if (PageIndex <= Total)
                Category(CategoryType, true);
        });

        public ICommand DetailCommand => new DelegateCommand<dynamic>(input => {
            Navigation(input);
        });
        #endregion

        #region Method
        private async void Category(string input, bool IsLoadMore = false)
        {
            this.Refresh = false;
            this.IsBusy = true;
            CategoryType = input;
            try
            {
                await Task.Delay(Soft.WaitSpan);
                var NovelCate = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.CacheTime,
                        NovelType = NovelEnum.Category,
                        Proxy = this.Proxy,
                        Category = new NovelCategory
                        {
                            Page = this.PageIndex,
                            NovelCategoryAddress = input
                        }
                    };
                }).RunsAsync();
                this.Total = NovelCate.SingleCategories.TotalPage;
                if (IsLoadMore)
                    NovelCate.SingleCategories.NovelSingles.ToMapest<List<NovelSearchResult>>().ForEach(item =>
                    {
                        this.NovelSearch.Add(item);
                    });
                else
                    this.NovelSearch = new ObservableCollection<NovelSearchResult>(NovelCate.SingleCategories.NovelSingles.ToMapest<List<NovelSearchResult>>());
                this.IsBusy = false;
            }
            catch
            {
                this.IsBusy = false;
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("网络有波动，请稍后再试~`(*>﹏<*)′"))
                {
                    await Task.Delay(3000);
                }
            }
        }
        private async void Refreshs(string input)
        {
            this.Refresh = true;
            CategoryType = input;
            try
            {
                await Task.Delay(Soft.WaitSpan);
                var NovelCate = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.CacheTime,
                        NovelType = NovelEnum.Category,
                        Proxy = this.Proxy,
                        Category = new NovelCategory
                        {
                            Page = this.PageIndex,
                            NovelCategoryAddress = input
                        }
                    };
                }).RunsAsync();
                this.Total = NovelCate.SingleCategories.TotalPage;
                this.NovelSearch = new ObservableCollection<NovelSearchResult>(NovelCate.SingleCategories.NovelSingles.ToMapest<List<NovelSearchResult>>());
                this.Refresh = false;
            }
            catch
            {
                this.Refresh = false;
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("网络有波动，请稍后再试~`(*>﹏<*)′"))
                {
                    await Task.Delay(3000);
                }
            }
        }
        private async void Search(dynamic input)
        {
            try
            {
                if (input == null)
                    using (await MaterialDialog.Instance.LoadingSnackbarAsync("还未选择浏览的书籍~"))
                    {
                        await Task.Delay(3000);
                    }

                NovelSearchResult? inputKey = (input as NovelSearchResult);
                this.Refresh = true;
                await Task.Delay(Soft.WaitSpan);

                var NovelSearch = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.CacheTime,
                        NovelType = NovelEnum.Search,
                        Proxy = this.Proxy,
                        Search = new NovelSearch
                        {
                            NovelSearchKeyWord = inputKey?.Author
                        }
                    };
                }).RunsAsync();
                this.NovelSearch = new ObservableCollection<NovelSearchResult>(NovelSearch.SearchResults);

                this.PageIndex = 1;
                this.Total = 0;
                this.Refresh = false;
            }
            catch
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("网络有波动，请稍后再试~`(*>﹏<*)′"))
                {
                    await Task.Delay(3000);
                }
            }
        }
        private async void Navigation(dynamic input) 
        {
            try
            {
                if (input == null)
                    using (await MaterialDialog.Instance.LoadingSnackbarAsync("还未选择浏览的书籍~"))
                    {
                        await Task.Delay(3000);
                    }

                NovelSearchResult? inputKey = (input as NovelSearchResult);

                var NavigationService = Prism.Ioc.ContainerLocator.Container.Resolve(typeof(INavigationService)) as INavigationService;

                var Param =  new NavigationParameters();
                Param.Add(nameof(NovelSearchResult), inputKey);
                await NavigationService.NavigateAsync("CandyIndexView/CandyNodelDetailView", Param);

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
