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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XExten.Advance.LinqFramework;
using XF.Material.Forms.UI.Dialogs;

namespace CandySugar.App.Controls.ViewModels.AcgAnimeModel
{
    public class CandyAcgAnimeViewModel : ViewModelNavigatBase
    {
        public CandyAcgAnimeViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = new AcgAnimeProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
            CandyLog = Resolve<ILoger>();
            CategoryRoute = string.Empty;
            this.PageIndex = 1;
        }

        #region Field
        private readonly ILoger CandyLog;
        private readonly AcgAnimeProxy Proxy;

        private string CategoryRoute;
        #endregion

        #region Property
        private ObservableCollection<AcgAnimeSearchResult> _SearchResult;
        public ObservableCollection<AcgAnimeSearchResult> SearchResult
        {
            get => _SearchResult;
            set => SetProperty(ref _SearchResult, value);
        }

        private ObservableCollection<AcgAnimeInitResult> _InitResult;
        public ObservableCollection<AcgAnimeInitResult> InitResult
        {
            get => _InitResult;
            set => SetProperty(ref _InitResult, value);
        }

        private ObservableCollection<AcgAnimeTagsResult> _TagResult;
        public ObservableCollection<AcgAnimeTagsResult> TagResult
        {
            get => _TagResult;
            set => SetProperty(ref _TagResult, value);
        }

        private ObservableCollection<AcgAnimePlayResult> _PlayResult;
        public ObservableCollection<AcgAnimePlayResult> PlayResult
        {
            get => _PlayResult;
            set => SetProperty(ref _PlayResult, value);
        }

        private AcgAnimeBrandsResult _BrandResult;
        public AcgAnimeBrandsResult BrandResult
        {
            get => _BrandResult;
            set => SetProperty(ref _BrandResult, value);
        }

        public ObservableCollection<string> _HType;
        public ObservableCollection<string> HType
        {
            get => _HType;
            set => SetProperty(ref _HType, value);
        }

        private int _Total;
        public int Total
        {
            get => _Total;
            set => SetProperty(ref _Total, value);
        }

        private int _PageIndex;
        public int PageIndex
        {
            get => _PageIndex;
            set => SetProperty(ref _PageIndex, value);
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get => _IsBusy;
            set => SetProperty(ref _IsBusy, value);
        }

        private bool _Refresh;
        public bool Refresh
        {
            get => _Refresh;
            set => SetProperty(ref _Refresh, value);
        }

        private string _SearchWord;
        public string SearchWord
        {
            get => _SearchWord;
            set => SetProperty(ref _SearchWord, value);
        }
        #endregion

        #region Command
        public ICommand ItemCommand => new DelegateCommand<string>(input =>
        {
            this.PageIndex = 1;
            SearchWord = string.Empty;
            CategoryRoute = input;
            CateAcg();
        });

        public ICommand SearchCommand => new DelegateCommand<string>(input =>
        {
            this.PageIndex = 1;
            SearchWord = input;
            CategoryRoute = string.Empty;
            SearchAcg();
        });

        public ICommand RefreshsCommand => new DelegateCommand(() =>
        {
            PageIndex = 1;
            if (CategoryRoute.IsNullOrEmpty())
                SearchAcg();
            else
                CateAcg();
        });

        public ICommand ShowMoreCommand => new DelegateCommand(() =>
        {
            this.PageIndex += 1;
            if (this.PageIndex <= Total)
            {
                if (CategoryRoute.IsNullOrEmpty())
                    SearchAcg(true);
                else
                    CateAcg(true);
            }
        });

        public ICommand DetailCommand => new DelegateCommand<AcgAnimeSearchResult>(input =>
        {
            Play(input.Watch);
        });
        #endregion

        #region Override
        protected override void OnViewLaunch()
        {
            InitAcg();
        }
        #endregion

        #region Method
        private async Task<string> Tip(string Method, Exception ex)
        {
            await CandyLog.Insert(new CandyGlobalLogDto
            {
                Location = $"{nameof(CandyAcgAnimeViewModel)}_{Method}",
                ErrorMsg = ex.Message,
                ErrorStack = ex.StackTrace
            });
            return String.Format(Soft.Toast, nameof(CandyAcgAnimeViewModel), Method);
        }

        public async void InitAcg()
        {
            try
            {
                this.Refresh = true;
                await Task.Delay(Soft.WaitSpan);
                var AcgInit = await AcgAnimeFactory.AcgAnime(opt =>
                {
                    opt.RequestParam = new AcgAnimeRequestInput
                    {
                        AcgType = AcgAnimeEnum.AcgInit,
                        CacheSpan = Soft.CacheTime,
                        Proxy = this.Proxy,
                    };
                }).RunsAsync();
                this.Refresh = false;

                HType = new ObservableCollection<string>(AcgInit.TypeResult);
                BrandResult = AcgInit.BrandResults;
                TagResult = new ObservableCollection<AcgAnimeTagsResult>(AcgInit.TagResults);
                InitResult = new ObservableCollection<AcgAnimeInitResult>(AcgInit.InitResults);
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("InitAcg", ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }

        public async void CateAcg(bool IsLoadMore = false)
        {
            try
            {
                if (IsLoadMore)
                    this.IsBusy = true;
                else
                    this.Refresh = true;

                var AcgCate = await AcgAnimeFactory.AcgAnime(opt =>
                {
                    opt.RequestParam = new AcgAnimeRequestInput
                    {
                        AcgType = AcgAnimeEnum.AcgCategory,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.CacheTime,
                        Category = new AcgAnimeCategory
                        {
                            Page = PageIndex,
                            Route = CategoryRoute
                        }
                    };
                }).RunsAsync();

                if (IsLoadMore)
                {
                    this.IsBusy = false;
                    if (this.SearchResult == null)
                        this.SearchResult = new ObservableCollection<AcgAnimeSearchResult>(AcgCate.SearchResults.SearchChildResult);
                    else
                    {
                        AcgCate.SearchResults.SearchChildResult.ForEach(item =>
                        {
                            this.SearchResult.Add(item);
                        });
                    }
                }
                else
                {
                    this.Refresh = false;
                    SearchResult = new ObservableCollection<AcgAnimeSearchResult>(AcgCate.SearchResults.SearchChildResult);
                }
                Total = AcgCate.SearchResults.Total;
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("CateAcg", ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }

        public async void SearchAcg(bool IsLoadMore = false)
        {
            try
            {
                if (IsLoadMore)
                    this.IsBusy = true;
                else
                    this.Refresh = true;

                var AcgSearch = await AcgAnimeFactory.AcgAnime(opt =>
                {
                    opt.RequestParam = new AcgAnimeRequestInput
                    {
                        AcgType = AcgAnimeEnum.AcgSearch,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.CacheTime,
                        Search = new AcgAnimeSearch
                        {
                            Page = PageIndex,
                            KeyWord = SearchWord,
                            //AnimeType = HAcgOption.Type,
                            //Brands = HAcgOption.Brands,
                            //Tags = HAcgOption.Tags
                        }
                    };
                }).RunsAsync();


                if (IsLoadMore)
                {
                    this.IsBusy = false;
                    if (this.SearchResult == null)
                        SearchResult = new ObservableCollection<AcgAnimeSearchResult>(AcgSearch.SearchResults.SearchChildResult);
                    else
                    {
                        AcgSearch.SearchResults.SearchChildResult.ForEach(item =>
                        {
                            this.SearchResult.Add(item);
                        });
                    }
                }
                else
                {
                    this.Refresh = false;
                    SearchResult = new ObservableCollection<AcgAnimeSearchResult>(AcgSearch.SearchResults.SearchChildResult);
                }
                Total = AcgSearch.SearchResults.Total;
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("CateAcg", ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }

        public async void Play(string input)
        {
            try
            {
                this.Refresh = true;
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
                this.Refresh = false;
                PlayResult = new ObservableCollection<AcgAnimePlayResult>(AcgPlay.PlayResults);

                var result = PlayResult.FirstOrDefault(t => !t.PlayRoute.IsNullOrEmpty());

                NavigationParameters param = new NavigationParameters();
                param.Add("WatchAddress", result.PlayRoute);
                await NavigationService.NavigateAsync(new Uri(nameof(CandyAcgAnimePlayView), UriKind.Relative), param);
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Play", ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }
        #endregion
    }
}
