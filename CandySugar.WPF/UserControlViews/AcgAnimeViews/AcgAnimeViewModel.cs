using CandySugar.WPF.CandyWindows.CandyWinViewModel;
using CandySugar.Common;
using CandySugar.Common.WinDTO;
using CandySugar.WPF.Properties;
using HandyControl.Controls;
using HandyControl.Data;
using SDKColloction.AcgAnimeSDK;
using SDKColloction.AcgAnimeSDK.ViewModel;
using SDKColloction.AcgAnimeSDK.ViewModel.Enums;
using SDKColloction.AcgAnimeSDK.ViewModel.Request;
using SDKColloction.AcgAnimeSDK.ViewModel.Response;
using SDKColloction.AnimeSDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XExten.Advance.LinqFramework;
using CandySugar.Controls.JSWindow;

namespace CandySugar.WPF.UserControlViews.AcgAnimeViews
{
    public class AcgAnimeViewModel : Screen
    {
        private readonly IContainer Container;
        private readonly AcgAnimeProxy Proxy;
        private bool IsSearch;
        private string SearchWord;
        private string CategoryWord;
        public AcgAnimeViewModel(IContainer Container)
        {
            IsSearch = false;
            this.Container = Container;
            Proxy = new AcgAnimeProxy
            {
                IP = Soft.Default.ProxyIP,
                Port = Soft.Default.ProxyPort,
                PassWord = Soft.Default.ProxyPwd,
                UserName = Soft.Default.ProxyAccount
            };
        }

        #region Property
        private ObservableCollection<AcgAnimeSearchResult> _SearchResult;
        public ObservableCollection<AcgAnimeSearchResult> SearchResult
        {
            get => _SearchResult;
            set => SetAndNotify(ref _SearchResult, value);
        }

        private ObservableCollection<AcgAnimeInitResult> _InitResult;
        public ObservableCollection<AcgAnimeInitResult> InitResult
        {
            get => _InitResult;
            set => SetAndNotify(ref _InitResult, value);
        }

        private ObservableCollection<AcgAnimeTagsResult> _TagResult;
        public ObservableCollection<AcgAnimeTagsResult> TagResult
        {
            get => _TagResult;
            set => SetAndNotify(ref _TagResult, value);
        }

        private ObservableCollection<AcgAnimePlayResult> _PlayResult;
        public ObservableCollection<AcgAnimePlayResult> PlayResult
        {
            get => _PlayResult;
            set => SetAndNotify(ref _PlayResult, value);
        }

        private AcgAnimeBrandsResult _BrandResult;
        public AcgAnimeBrandsResult BrandResult
        {
            get => _BrandResult;
            set => SetAndNotify(ref _BrandResult, value);
        }

        public ObservableCollection<string> _HType;
        public ObservableCollection<string> HType
        {
            get => _HType;
            set => SetAndNotify(ref _HType, value);
        }

        private int _Total;
        public int Total
        {
            get => _Total;
            set => SetAndNotify(ref _Total, value);
        }

        private int _PageIndex;
        public int PageIndex
        {
            get => _PageIndex;
            set => SetAndNotify(ref _PageIndex, value);
        }
        #endregion

        #region Override
        protected override void OnViewLoaded()
        {
            //var result = new CnadyJsWin { Visibility = Visibility.Visible }.ShowDialog();
            Init();
        }
        #endregion

        #region Command
        public void SetFilter()
        {
            CandyAcgProviderViewModel ViewModel = Container.Get<CandyAcgProviderViewModel>();
            ViewModel.HType = HType;
            ViewModel.TagResult = TagResult;
            ViewModel.BrandResult = BrandResult;
            ViewModel.Loading = Visibility.Hidden;
            BootResource.AcgView(window =>
            {
                window.DataContext = ViewModel;
            });
        }
        public void ResetFilter()
        {
            HAcgOption.Brands = null;
            HAcgOption.Tags = null;
            HAcgOption.Type = null;
        }
        public void Redirect(string input)
        {
            IsSearch = false;
            PageIndex = 1;
            CategoryWord = input;
            Category();
        }
        public void SearchAcg(string input)
        {
            IsSearch = true;
            PageIndex = 1;
            SearchWord = input;
            Search();
        }
        public void PageUpdated(FunctionEventArgs<int> input)
        {
            if (IsSearch)
            {
                PageIndex = input.Info;
                if (PageIndex <= Total)
                    Search();
            }
            else
            {
                PageIndex = input.Info;
                if (PageIndex <= Total)
                    Category();
            }
        }
        public void PreviewCommand(string input)
        {
            Play(input);
        }
        #endregion

        #region Method
        public async void Init()
        {
            try
            {
                HelpUtilty.WirteLog("初始化ACG动漫操作");
                var AcgInit = await AcgAnimeFactory.AcgAnime(opt =>
                {
                    opt.RequestParam = new AcgAnimeRequestInput
                    {
                        AcgType = AcgAnimeEnum.AcgInit,
                        CacheSpan = Soft.Default.CacheTime,
                        Proxy = this.Proxy,
                    };
                }).RunsAsync();

                HType = new ObservableCollection<string>(AcgInit.TypeResult);
                BrandResult = AcgInit.BrandResults;
                TagResult = new ObservableCollection<AcgAnimeTagsResult>(AcgInit.TagResults);
                InitResult = new ObservableCollection<AcgAnimeInitResult>(AcgInit.InitResults);
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }
        }
        public async void Category()
        {
            try
            {
                HelpUtilty.WirteLog("ACG动漫分类操作");
                var AcgCate = await AcgAnimeFactory.AcgAnime(opt =>
                {
                    opt.RequestParam = new AcgAnimeRequestInput
                    {
                        AcgType = AcgAnimeEnum.AcgCategory,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.Default.CacheTime,
                        Category = new AcgAnimeCategory
                        {
                            Page = PageIndex,
                            Route = CategoryWord
                        }
                    };
                }).RunsAsync();
                SearchResult = new ObservableCollection<AcgAnimeSearchResult>(AcgCate.SearchResults.SearchChildResult);
                Total = AcgCate.SearchResults.Total;
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }
        }
        public async void Search()
        {
            try
            {
                HelpUtilty.WirteLog("ACG动漫查询操作");
                var AcgSearch = await AcgAnimeFactory.AcgAnime(opt =>
                {
                    opt.RequestParam = new AcgAnimeRequestInput
                    {
                        AcgType = AcgAnimeEnum.AcgSearch,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.Default.CacheTime,
                        Search = new AcgAnimeSearch
                        {
                            Page = PageIndex,
                            KeyWord = SearchWord,
                            AnimeType = HAcgOption.Type,
                            Brands = HAcgOption.Brands,
                            Tags = HAcgOption.Tags
                        }
                    };
                }).RunsAsync();
                SearchResult = new ObservableCollection<AcgAnimeSearchResult>(AcgSearch.SearchResults.SearchChildResult);
                Total = AcgSearch.SearchResults.Total;
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }
        }
        public async void Play(string input)
        {
            try
            {
                HelpUtilty.WirteLog("ACG动漫播放操作");
                var AcgPlay = await AcgAnimeFactory.AcgAnime(opt =>
                {
                    opt.RequestParam = new AcgAnimeRequestInput
                    {
                        AcgType = AcgAnimeEnum.AcgPlay,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.Default.CacheTime,
                        Play = new AcgAnimePlay
                        {
                            Route = input
                        }
                    };
                }).RunsAsync();

                PlayResult = new ObservableCollection<AcgAnimePlayResult>(AcgPlay.PlayResults);

                var result = PlayResult.FirstOrDefault(t => !t.PlayRoute.IsNullOrEmpty());

                if (Soft.Default.PlayBox == 0)
                {
                    var vm = Container.Get<CandyVLCViewModel>();
                    vm.WatchResult = new AnimePlayResult
                    {
                        PlayURL = result.PlayRoute,
                        AnimeName = result.Title,
                        CollectName = result.Title
                    };
                    vm.Loading = Visibility.Hidden;
                    BootResource.AnimeVLC(window =>
                    {
                        window.DataContext = vm;
                    });
                }
                if (Soft.Default.PlayBox == 1)
                {
                    var vm = Container.Get<CandyDPlayViewModel>();
                    vm.WatchResult = new AnimePlayResult
                    {
                        PlayURL = result.PlayRoute,
                        AnimeName = result.Title,
                        CollectName = result.Title
                    };
                    vm.Loading = Visibility.Hidden;
                    BootResource.AnimeWEB(window =>
                    {
                        window.DataContext = vm;
                    });
                }
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }
        }
        #endregion
    }
}
