using Anime.SDK;
using Anime.SDK.ViewModel;
using Anime.SDK.ViewModel.Enums;
using Anime.SDK.ViewModel.Request;
using Anime.SDK.ViewModel.Response;
using CandySugar.App.Controls.Views.Anime;
using CandySugar.Xam.Common;
using CandySugar.Xam.Core.Service;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XExten.Advance.InternalFramework.Securities.Common;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;
using XF.Material.Forms.UI.Dialogs;
using Prism.Ioc;
using CandySugar.Xam.Common.DTO;
using CandySugar.Xam.Common.Entity.Model;
using XF.Material.Forms.UI.Dialogs.Configurations;
using Xamarin.Forms;

namespace CandySugar.App.Controls.ViewModels.AnimeModel
{
    public class CandyAnimeViewModel : ViewModelNavigatBase
    {
        public CandyAnimeViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.Proxy = new AnimeProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };

            this.LetterCate = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(",").ToList();
            this.PageIndex = 1;
            this.Activity = false;

            Candy = ContainerLocator.Container.Resolve<IDMLiShi>();
        }

        #region Field
        private readonly IDMLiShi Candy;
        private readonly AnimeProxy Proxy;
        private string Letters;
        private string Categorys;
        private string KeyWords;
        #endregion

        #region Property
        private List<string> _LetterCate;
        public List<string> LetterCate
        {
            get { return _LetterCate; }
            set { SetProperty(ref _LetterCate, value); }
        }

        private Dictionary<string, string> _RecommendCategory;
        public Dictionary<string, string> RecommendCategory
        {
            get { return _RecommendCategory; }
            set { SetProperty(ref _RecommendCategory, value); }
        }

        private ObservableCollection<AnimeWeekDayRecommendResult> _WeekDay;
        public ObservableCollection<AnimeWeekDayRecommendResult> WeekDay
        {
            get { return _WeekDay; }
            set { SetProperty(ref _WeekDay, value); }
        }

        private int _Total;
        public int Total
        {
            get { return _Total; }
            set { SetProperty(ref _Total, value); }
        }

        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { SetProperty(ref _PageIndex, value); }
        }

        private ObservableCollection<AnimeSearchResults> _Result;
        public ObservableCollection<AnimeSearchResults> Result
        {
            get { return _Result; }
            set { SetProperty(ref _Result, value); }
        }

        private ObservableCollection<AnimeDetailResult> _Detail;
        public ObservableCollection<AnimeDetailResult> Detail
        {
            get { return _Detail; }
            set { SetProperty(ref _Detail, value); }
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

        private bool _Activity;
        public bool Activity
        {
            get { return _Activity; }
            set { SetProperty(ref _Activity, value); }
        }
        #endregion

        #region Method
        public async void Init()
        {
            try
            {
                var AnimeInit = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new AnimeRequestInput
                    {
                        AnimeType = AnimeEnum.Init,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.CacheTime
                    };
                }).RunsAsync();
                this.RecommendCategory = AnimeInit.RecommendCategory;

                this.WeekDay = new ObservableCollection<AnimeWeekDayRecommendResult>(AnimeInit.WeekDays.FirstOrDefault(t => t.DayName.Equals(GetDayName())).DayRecommends);
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("网络有波动，请稍后再试~`(*>﹏<*)′"))
                {
                    await Task.Delay(3000);
                }
            }
        }
        public async void Category(string input, bool IsLoadMore = true)
        {
            try
            {
                if (IsLoadMore)
                    this.IsBusy = true;
                else
                    this.Refresh = true;
                await Task.Delay(Soft.WaitSpan);
                var AnimeCateType = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new AnimeRequestInput
                    {
                        AnimeType = AnimeEnum.CategoryType,
                        CacheSpan = Soft.CacheTime,
                        Proxy = this.Proxy,
                        Category = new AnimeCategory
                        {
                            Page = PageIndex,
                            Address = input
                        }
                    };
                }).RunsAsync();
                this.Total = AnimeCateType.SeachResults.Page;
                if (IsLoadMore)
                {
                    this.IsBusy = false;
                    if (this.Result == null)
                        this.Result = new ObservableCollection<AnimeSearchResults>(AnimeCateType.SeachResults.Searchs);
                    else
                    {
                        AnimeCateType.SeachResults.Searchs.ForEach(item =>
                        {
                            this.Result.Add(item);
                        });
                    }
                }
                else
                {
                    this.Refresh = false;
                    this.Result = new ObservableCollection<AnimeSearchResults>(AnimeCateType.SeachResults.Searchs);
                }
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("网络有波动，请稍后再试~`(*>﹏<*)′"))
                {
                    await Task.Delay(3000);
                }
            }
        }
        public async void LetterCategory(string input, bool IsLoadMore = true)
        {
            try
            {
                if (IsLoadMore)
                    this.IsBusy = true;
                else
                    this.Refresh = true;
                await Task.Delay(Soft.WaitSpan);
                var AnimeCate = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new AnimeRequestInput
                    {
                        CacheSpan = Soft.CacheTime,
                        AnimeType = AnimeEnum.Category,
                        Proxy = this.Proxy,
                        Category = new AnimeCategory
                        {
                            Page = PageIndex,
                            AnimeLetterType = Enum.Parse<AnimeLetterEnum>(input)
                        }
                    };
                }).RunsAsync();
                this.Total = AnimeCate.SeachResults.Page;
                if (IsLoadMore)
                {
                    this.IsBusy = false;
                    if (this.Result == null)
                        this.Result = new ObservableCollection<AnimeSearchResults>(AnimeCate.SeachResults.Searchs);
                    else
                    {
                        AnimeCate.SeachResults.Searchs.ForEach(item =>
                        {
                            this.Result.Add(item);
                        });
                    }
                }
                else
                {
                    this.Refresh = false;
                    this.Result = new ObservableCollection<AnimeSearchResults>(AnimeCate.SeachResults.Searchs);
                }
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("网络有波动，请稍后再试~`(*>﹏<*)′"))
                {
                    await Task.Delay(3000);
                }
            }
        }
        public async void Search(string input, bool IsLoadMore = true)
        {
            try
            {
                if (IsLoadMore)
                    this.IsBusy = true;
                else
                    this.Refresh = true;
                await Task.Delay(Soft.WaitSpan);
                var AnimeSearch = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new AnimeRequestInput
                    {
                        AnimeType = AnimeEnum.Search,
                        CacheSpan = Soft.CacheTime,
                        Proxy = this.Proxy,
                        Search = new AnimeSearch
                        {
                            AnimeSearchKeyWord = input,
                            Page = PageIndex
                        }
                    };
                }).RunsAsync();
                this.Total = AnimeSearch.SeachResults.Page;

                if (IsLoadMore)
                {
                    this.IsBusy = false;
                    if (this.Result == null)
                        this.Result = new ObservableCollection<AnimeSearchResults>(AnimeSearch.SeachResults.Searchs);
                    else
                    {
                        AnimeSearch.SeachResults.Searchs.ForEach(item =>
                        {
                            this.Result.Add(item);
                        });
                    }
                }
                else
                {
                    this.Refresh = false;
                    this.Result = new ObservableCollection<AnimeSearchResults>(AnimeSearch.SeachResults.Searchs);
                }
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("网络有波动，请稍后再试~`(*>﹏<*)′"))
                {
                    await Task.Delay(3000);
                }
            }
        }
        public async void SearchDetail(string input)
        {
            try
            {
                Activity = true;
                await Task.Delay(Soft.WaitSpan);
                var AnimeDetail = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new AnimeRequestInput
                    {
                        AnimeType = AnimeEnum.Detail,
                        CacheSpan = Soft.CacheTime,
                        Proxy = this.Proxy,
                        Detail = new AnimeDetail
                        {
                            DetailAddress = input
                        }
                    };
                }).RunsAsync();

                this.Detail = new ObservableCollection<AnimeDetailResult>(AnimeDetail.DetailResults.Where(t => t.IsDownURL == false));
                Activity = false;

              var result =  await MaterialDialog.Instance.SelectActionAsync(this.Detail.Select(t => t.CollectName).ToList(), new MaterialSimpleDialogConfiguration
                {
                    TextColor = Color.FromRgb(255,133,133),
                    CornerRadius = 10
                }) ;

                Play(this.Detail[result]);
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("网络有波动，请稍后再试~`(*>﹏<*)′"))
                {
                    await Task.Delay(3000);
                }
            }
        }
        public async void Play(AnimeDetailResult input)
        {
            try
            {
                var AnimeWath = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new AnimeRequestInput
                    {
                        AnimeType = AnimeEnum.Watch,
                        Proxy = this.Proxy,
                        WatchPlay = new AnimeWatchPlay
                        {
                            DetailResult = input
                        }
                    };
                }).RunsAsync();

                await Candy.InsertOrUpdate(new CandyDMLiShiDto
                {
                    AnimeName = input.AnimeName,
                    CollectionName = input.CollectName,
                    Cover = input.Cover,
                    PlayURL = AnimeWath.PlayResult.PlayURL
                });

                NavigationParameters param = new NavigationParameters();
                param.Add("WatchAddress", AnimeWath.PlayResult.PlayURL);
                await NavigationService.NavigateAsync(new Uri(nameof(CandyAnimePlayView), UriKind.Relative), param);
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("网络有波动，请稍后再试~`(*>﹏<*)′"))
                {
                    await Task.Delay(3000);
                }
            }
        }
        #endregion

        #region Command
        public ICommand RefreshsCommand => new DelegateCommand(() =>
        {
            PageIndex = 1;
            if (Letters.IsNullOrEmpty() && Categorys.IsNullOrEmpty())
                Search(KeyWords, false);
            else if (Letters.IsNullOrEmpty() && KeyWords.IsNullOrEmpty())
                Category(Categorys, false);
            else
                LetterCategory(Letters, false);
        });

        public ICommand CateCommand => new DelegateCommand<dynamic>(input =>
        {
            this.PageIndex = 1;
            Letters = string.Empty;
            KeyWords = string.Empty;
            Categorys = input;
            Category(input, false);
        });

        public ICommand LettCommand => new DelegateCommand<dynamic>(input =>
        {
            this.PageIndex = 1;
            Letters = input;
            Categorys = string.Empty;
            KeyWords = string.Empty;
            LetterCategory(input, false);
        });

        public ICommand SearchCommand => new DelegateCommand<dynamic>(input =>
        {

            Letters = string.Empty;
            Categorys = string.Empty;
            KeyWords = input;
            Search(input, false);
        });

        public ICommand DetailCommand => new DelegateCommand<dynamic>(input =>
        {
            SearchDetail(input);
        });

        public ICommand ShowMoreCommand => new DelegateCommand(() =>
        {
            this.PageIndex += 1;
            if (this.PageIndex <= this.Total)
            {
                if (Letters.IsNullOrEmpty() && Categorys.IsNullOrEmpty())
                    Search(KeyWords);
                else if (Letters.IsNullOrEmpty() && KeyWords.IsNullOrEmpty())
                    Category(Categorys);
                else
                    LetterCategory(Letters);
            }
        });
    
        #endregion

        #region Override
        protected override void OnViewLaunch()
        {
            Init();
        }
        #endregion

        #region Util
        private string GetDayName()
        {
            return DateTime.Now.DayOfWeek switch
            {
                DayOfWeek.Sunday => "周日",
                DayOfWeek.Monday => "周一",
                DayOfWeek.Tuesday => "周二",
                DayOfWeek.Wednesday => "周三",
                DayOfWeek.Thursday => "周四",
                DayOfWeek.Friday => "周五",
                DayOfWeek.Saturday => "周六",
                _ => "",
            };
        }
        #endregion
    }
}
