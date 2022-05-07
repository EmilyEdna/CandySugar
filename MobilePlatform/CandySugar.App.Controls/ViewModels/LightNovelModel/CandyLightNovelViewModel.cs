using CandySugar.Xam.Common;
using SDKColloction.LightNovelSDK;
using SDKColloction.LightNovelSDK.ViewModel;
using SDKColloction.LightNovelSDK.ViewModel.Enums;
using SDKColloction.LightNovelSDK.ViewModel.Request;
using SDKColloction.LightNovelSDK.ViewModel.Response;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XExten.Advance.LinqFramework;
using XF.Material.Forms.UI.Dialogs;
using Prism.Ioc;
using CandySugar.App.Controls.Views.LightNovel;
using CandySugar.Xam.Core.Service;
using CandySugar.Xam.Common.DTO;

namespace CandySugar.App.Controls.ViewModels.LightNovelModel
{
    public class CandyLightNovelViewModel : ViewModelNavigatBase
    {
        public const string Account = "kilydoll365";
        public const string Password = "sion8550";
        private readonly ILoger CandyLog;
        public CandyLightNovelViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = new LightNovelProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
            CandyLog = Resolve<ILoger>();
        }

        #region Field
        private readonly LightNovelProxy Proxy;
        private string CategoryAddress;
        private string KeyWrod;
        private bool IsSearch = false;
        #endregion

        #region Property
        private ObservableCollection<LightNovelCategoryResult> _LightNovelCategory;
        public ObservableCollection<LightNovelCategoryResult> LightNovelCategory
        {
            get { return _LightNovelCategory; }
            set { SetProperty(ref _LightNovelCategory, value); }
        }
        private ObservableCollection<LightNovelSingleCategoryResults> _LightNovelSingleCategory;
        public ObservableCollection<LightNovelSingleCategoryResults> LightNovelSingleCategory
        {
            get { return _LightNovelSingleCategory; }
            set { SetProperty(ref _LightNovelSingleCategory, value); }
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

        #region Command
        public ICommand SearchCommand => new DelegateCommand<dynamic>(input =>
        {
            PageIndex = 1;
            IsSearch = true;
            KeyWrod = input;
            Search(input, true);
        });

        public ICommand RefreshsCommand => new DelegateCommand(() =>
        {
            if (IsSearch)
            {
                PageIndex = 1;
                Search(KeyWrod, true);
            }
            else
            {
                if (!CategoryAddress.IsNullOrEmpty())
                {
                    PageIndex = 1;
                    Category(CategoryAddress, true);
                }
            }
        });

        public ICommand ShowMoreCommand => new DelegateCommand(() =>
        {
            PageIndex += 1;
            if (PageIndex <= Total)
            {
                if (IsSearch)
                    Search(KeyWrod);
                else
                    Category(CategoryAddress);
            }
        });

        public ICommand ItemCommand => new DelegateCommand<string>(input =>
        {
            PageIndex = 1;
            IsSearch = false;
            KeyWrod = string.Empty;
            Category(input, true);
        });

        public ICommand DetailCommand => new DelegateCommand<dynamic>(input =>
        {
            if (input != null)
            {
                Navigation(input);
            }
        });
        #endregion

        #region Override
        protected override void OnViewLaunch()
        {
            Init();
        }
        #endregion

        #region Method
        private async Task<string> Tip(string Method, Exception ex)
        {
            await CandyLog.Insert(new CandyGlobalLogDto
            {
                Location = $"{nameof(CandyLightNovelViewModel)}_{Method}",
                ErrorMsg = ex.Message,
                ErrorStack = ex.StackTrace
            });
            return String.Format(Soft.Toast,nameof(CandyLightNovelViewModel), Method);
        }
        public async void Init()
        {
            try
            {
                this.Refresh = true;
                await Task.Delay(Soft.WaitSpan);
                //初始化
                var LightNovelInit = await LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        CacheSpan = Soft.CacheTime,
                        LightNovelType = LightNovelEnum.LightInit,
                        Proxy = this.Proxy
                    };
                }).RunsAsync(Light =>
                {
                    Light.RefreshCookie(new LightNovelRefresh
                    {
                        UserName = Account,
                        PassWord = Password
                    }, this.Proxy);
                });
                this.Refresh = false;
                LightNovelCategory = new ObservableCollection<LightNovelCategoryResult>(LightNovelInit.CategoryResults);
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Init",ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }

        public async void Category(string input, bool IsLoadMore = false)
        {
            try
            {
                if (IsLoadMore) this.Refresh = true; else this.IsBusy = true;
                await Task.Delay(Soft.WaitSpan);
                CategoryAddress = input;
                var LightNovelCate = await LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.LightCategory,
                        CacheSpan = Soft.CacheTime,
                        Proxy = this.Proxy,
                        Category = new LightNovelCategory
                        {
                            Page = this.PageIndex,
                            CategoryAddress = CategoryAddress
                        }
                    };
                }).RunsAsync(Light =>
                {
                    Light.RefreshCookie(new LightNovelRefresh
                    {
                        UserName = Account,
                        PassWord = Password
                    }, new LightNovelProxy());
                });

                Total = LightNovelCate.SingleCategoryResult.TotalPage;
                if (!IsLoadMore)
                {
                    IsBusy = false;
                    if (this.LightNovelSingleCategory == null)
                        this.LightNovelSingleCategory = new ObservableCollection<LightNovelSingleCategoryResults>(LightNovelCate.SingleCategoryResult.Result);
                    else
                    {
                        LightNovelCate.SingleCategoryResult.Result.ForEach(item =>
                        {
                            this.LightNovelSingleCategory.Add(item);
                        });
                    }
                }
                else
                {
                    Refresh = false;
                    this.LightNovelSingleCategory = new ObservableCollection<LightNovelSingleCategoryResults>(LightNovelCate.SingleCategoryResult.Result);
                }

            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Category",ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }

        public async void Search(string input, bool IsLoadMore = false)
        {
            try
            {
                if (IsLoadMore) this.Refresh = true; else this.IsBusy = true;
                await Task.Delay(Soft.WaitSpan);
                //搜索
                var LightNovelSearch = await LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.LightSearch,
                        CacheSpan = Soft.CacheTime,
                        Proxy = this.Proxy,
                        Search = new LightNovelSearch
                        {
                            Page = this.PageIndex,
                            KeyWord = input,
                            SearchType = LightNovelSearchEnum.ArticleName
                        }
                    };
                }).RunsAsync(Light =>
                {
                    Light.RefreshCookie(new LightNovelRefresh
                    {
                        UserName = Account,
                        PassWord = Password
                    }, new LightNovelProxy());
                });

                if (LightNovelSearch.SearchResults.Result != null)
                {
                    Total = LightNovelSearch.SearchResults.TotalPage;

                    if (!IsLoadMore)
                    {
                        IsBusy = false;
                        if (this.LightNovelSingleCategory == null)
                            this.LightNovelSingleCategory = new ObservableCollection<LightNovelSingleCategoryResults>(LightNovelSearch.SearchResults.Result.ToMapest<List<LightNovelSingleCategoryResults>>());
                        else
                        {
                            LightNovelSearch.SearchResults.Result.ToMapest<List<LightNovelSingleCategoryResults>>().ForEach(item =>
                            {
                                this.LightNovelSingleCategory.Add(item);
                            });
                        }
                    }
                    else
                    {
                        Refresh = false;
                        this.LightNovelSingleCategory = new ObservableCollection<LightNovelSingleCategoryResults>(LightNovelSearch.SearchResults.Result.ToMapest<List<LightNovelSingleCategoryResults>>());
                    }

                }
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Search",ex)))
                {
                    await Task.Delay(3000);
                }
            }

        }

        public async void Navigation(LightNovelSingleCategoryResults input)
        {
            var Param = new NavigationParameters();
            Param.Add("Route", input.DetailAddress);
            Param.Add("BookName", input.BookName);
            Param.Add("Cover", input.Cover);
            await NavigationService.NavigateAsync(new Uri(nameof(CandyLightNovelDetailView), UriKind.Relative), Param);
        }
        #endregion
    }
}
