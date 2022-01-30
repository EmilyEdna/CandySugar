using Anime.SDK;
using Anime.SDK.ViewModel;
using Anime.SDK.ViewModel.Enums;
using Anime.SDK.ViewModel.Request;
using Anime.SDK.ViewModel.Response;
using CandySugar.CandyWindows.CnadyWinViewModel;
using CandySugar.Properties;
using HandyControl.Controls;
using HandyControl.Data;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace CandySugar.UserControlViews.AnimeViews
{
    public class AnimeViewModel : Screen
    {
        private readonly IContainer Container;
        private readonly AnimeProxy Proxy;
        public AnimeViewModel(IContainer Container)
        {
            this.Container = Container;
            Proxy = new AnimeProxy
            {
                IP = Soft.Default.ProxyIP,
                Port = Soft.Default.ProxyPort,
                PassWord = Soft.Default.ProxyPwd,
                UserName = Soft.Default.ProxyAccount
            };
            LetterCate = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(",").ToList();
            PageIndex = 1;
        }

        #region Property
        private List<string> _LetterCate;
        public List<string> LetterCate
        {
            get { return _LetterCate; }
            set { SetAndNotify(ref _LetterCate, value); }
        }

        private Dictionary<string, string> _RecommendCategory;
        public Dictionary<string, string> RecommendCategory
        {
            get { return _RecommendCategory; }
            set { SetAndNotify(ref _RecommendCategory, value); }
        }

        private ObservableCollection<AnimeWeekDayResult> _WeekDay;
        public ObservableCollection<AnimeWeekDayResult> WeekDay
        {
            get { return _WeekDay; }
            set { SetAndNotify(ref _WeekDay, value); }
        }

        private int _Total;
        public int Total
        {
            get { return _Total; }
            set { SetAndNotify(ref _Total, value); }
        }

        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { SetAndNotify(ref _PageIndex, value); }
        }

        private ObservableCollection<AnimeSearchResults> _Result;
        public ObservableCollection<AnimeSearchResults> Result
        {
            get { return _Result; }
            set { SetAndNotify(ref _Result, value); }
        }

        private ObservableCollection<AnimeDetailResult> _Detail;
        public ObservableCollection<AnimeDetailResult> Detail
        {
            get { return _Detail; }
            set { SetAndNotify(ref _Detail, value); }
        }

        #endregion

        #region Field
        private string SearchKey;
        private string CategoryKey;
        #endregion

        #region Internal
        protected override void OnViewLoaded()
        {
            SyncStatic.TryCatch(async () =>
            {
                var AnimeInit = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new AnimeRequestInput
                    {
                        AnimeType = AnimeEnum.Init,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.Default.CacheTime
                    };
                }).RunsAsync();
                this.RecommendCategory = AnimeInit.RecommendCategory;
                this.WeekDay = new ObservableCollection<AnimeWeekDayResult>(AnimeInit.WeekDays);
            }, ex => MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"));
        }
        #endregion

        #region Action
        public void SearchAnime(string args)
        {
            SearchKey = args;
            CategoryKey = string.Empty;
            SyncStatic.TryCatch(async () =>
            {
                var AnimeSearch = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new AnimeRequestInput
                    {
                        AnimeType = AnimeEnum.Search,
                        CacheSpan = Soft.Default.CacheTime,
                        Proxy = this.Proxy,
                        Search = new AnimeSearch
                        {
                            AnimeSearchKeyWord = SearchKey,
                            Page = PageIndex
                        }
                    };
                }).RunsAsync();
                this.Total = AnimeSearch.SeachResults.Page;
                this.Result = new ObservableCollection<AnimeSearchResults>(AnimeSearch.SeachResults.Searchs);
            }, ex => MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"));
        }

        public void Redirect(string args)
        {
            SyncStatic.TryCatch(() =>
            {
                var AnimeDetail = AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new AnimeRequestInput
                    {
                        AnimeType = AnimeEnum.Detail,
                        CacheSpan = Soft.Default.CacheTime,
                        Proxy = this.Proxy,
                        Detail = new AnimeDetail
                        {
                            DetailAddress = args
                        }
                    };
                }).Runs();
                this.Detail = new ObservableCollection<AnimeDetailResult>(AnimeDetail.DetailResults);
            }, ex => MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"));
        }

        public void Category(string args)
        {
            SearchKey = string.Empty;
            CategoryKey = args;
            if (this.LetterCate.Contains(CategoryKey))
            {
                SyncStatic.TryCatch(async () =>
                {
                    var AnimeCate = await AnimeFactory.Anime(opt =>
                    {
                        opt.RequestParam = new AnimeRequestInput
                        {
                            CacheSpan = Soft.Default.CacheTime,
                            AnimeType = AnimeEnum.Category,
                            Proxy = this.Proxy,
                            Category = new AnimeCategory
                            {
                                Page = PageIndex,
                                AnimeLetterType = Enum.Parse<AnimeLetterEnum>(CategoryKey)
                            }
                        };
                    }).RunsAsync();
                    this.Total = AnimeCate.SeachResults.Page;
                    this.Result = new ObservableCollection<AnimeSearchResults>(AnimeCate.SeachResults.Searchs);
                }, ex => MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"));
            }
            else
            {
                SyncStatic.TryCatch(async () =>
                {
                    var AnimeCateType = await AnimeFactory.Anime(opt =>
                    {
                        opt.RequestParam = new AnimeRequestInput
                        {
                            AnimeType = AnimeEnum.CategoryType,
                            CacheSpan = Soft.Default.CacheTime,
                            Proxy = this.Proxy,
                            Category = new AnimeCategory
                            {
                                Page = PageIndex,
                                Address = CategoryKey
                            }
                        };
                    }).RunsAsync();
                    this.Total = AnimeCateType.SeachResults.Page;
                    this.Result = new ObservableCollection<AnimeSearchResults>(AnimeCateType.SeachResults.Searchs);
                }, ex => MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"));
            }
        }

        public void PageUpdated(FunctionEventArgs<int> args)
        {
            PageIndex = args.Info;
            if (CategoryKey.IsNullOrEmpty())
                SearchAnime(SearchKey);
            else
                Category(CategoryKey);
        }

        public void Play(string args)
        {
            SyncStatic.TryCatch(async () =>
            {
                var AnimeWath = await AnimeFactory.Anime(opt =>
                 {
                     opt.RequestParam = new AnimeRequestInput
                     {
                         AnimeType = AnimeEnum.Watch,
                         Proxy = new AnimeProxy(),
                         WatchPlay = new AnimeWatchPlay
                         {
                             DetailResult = Detail.Where(t => t.WatchAddress.Equals(args)).FirstOrDefault()
                         }
                     };
                 }).RunsAsync();

                if (Soft.Default.PlayBox == 0)
                {
                    var vm = Container.Get<CandyVLCViewModel>();
                    vm.WatchResult = AnimeWath.PlayResult;
                    //LoteAnimeHistoryDTO DTO = AnimeWath.PlayResult.ToMapest<LoteAnimeHistoryDTO>();
                    //DTO.PlayMode = false;
                    //Open
                    BootResource.AnimeVLC(window =>
                    {
                        window.DataContext = vm;
                    });
                    //container.Get<IHistoryService>().AddAnimeHistory(DTO);
                }
                if (Soft.Default.PlayBox == 1)
                {
                    var vm = Container.Get<CandyDPlayViewModel>();
                    vm.WatchResult = AnimeWath.PlayResult;
                    //LoteAnimeHistoryDTO DTO = AnimeWath.PlayResult.ToMapest<LoteAnimeHistoryDTO>();
                    //DTO.PlayMode = true;
                    //Open
                    BootResource.AnimeWEB(window =>
                    {
                        window.DataContext = vm;
                    });
                    //container.Get<IHistoryService>().AddAnimeHistory(DTO);
                }
            }, ex => MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"));
        }
        #endregion
    }
}
