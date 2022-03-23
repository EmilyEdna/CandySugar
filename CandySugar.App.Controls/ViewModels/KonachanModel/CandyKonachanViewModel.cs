using CandySugar.Xam.Common;
using CandySugar.Xam.Core.Service;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wallpaper.SDK;
using Wallpaper.SDK.ViewModel;
using Wallpaper.SDK.ViewModel.Enums;
using Wallpaper.SDK.ViewModel.Request;
using Wallpaper.SDK.ViewModel.Response;
using XExten.Advance.LinqFramework;
using XF.Material.Forms.UI.Dialogs;
using Prism.Ioc;
using CandySugar.Xam.Common.DTO;

namespace CandySugar.App.Controls.ViewModels.KonachanModel
{
    public class CandyKonachanViewModel : ViewModelNavigatBase
    {
        private readonly WallpaperProxy Proxy;
        private readonly IBZLiShi Candy;
        public CandyKonachanViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = new WallpaperProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
            this.PageIndex = 1;
            Candy = ContainerLocator.Container.Resolve<IBZLiShi>();
        }

        #region Flied
        private string KeyWord;
        private int Limit = 10;
        private int PageTotal;
        private int Page = 1;
        #endregion

        #region Property
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

        private ObservableCollection<WallpaperResultDetail> _Wallpaper;
        public ObservableCollection<WallpaperResultDetail> Wallpaper
        {
            get { return _Wallpaper; }
            set { SetProperty(ref _Wallpaper, value); }
        }

        private ObservableCollection<WallpaperResultDetail> _WallpaperLike;
        public ObservableCollection<WallpaperResultDetail> WallpaperLike
        {
            get { return _WallpaperLike; }
            set { SetProperty(ref _WallpaperLike, value); }
        }
        #endregion

        #region Command

        public ICommand TabChangedCommand => new DelegateCommand<dynamic>((input) =>
        {

            if (input == 0)
            {
                if (KeyWord.IsNullOrEmpty())
                    Init();
                else
                    SearchBaisc();
            }
            else

                Query();
        });

        public ICommand RefreshsMainCommand => new DelegateCommand(() =>
        {
            PageIndex = 1;
            if (KeyWord.IsNullOrEmpty())
                Init();
            else
                SearchBaisc();
        });
        public ICommand SearchCommand => new DelegateCommand<dynamic>(input =>
        {
            PageIndex = 1;
            KeyWord = input;
            SearchBaisc();

        });
        public ICommand ShowMoreMainCommand => new DelegateCommand(() =>
        {
            PageIndex += 1;
            if (PageIndex <= Total)
            {
                if (KeyWord.IsNullOrEmpty())
                    Init(true);
                else
                    SearchBaisc(true);
            }
        });

        public ICommand InsertCommand => new DelegateCommand<dynamic>(input =>
        {
            if (input != null)
            {
                Insert(input);
            }
        });

        public ICommand RefreshsLikeCommand => new DelegateCommand(() =>
        {
            Query();
        });
        public ICommand ShowMoreLikeCommand => new DelegateCommand(() =>
        {
            Page += 1;
            if (Page <= PageTotal)
                Query(Page, true);
        });

        public ICommand RemoveCommand => new DelegateCommand<dynamic>(input =>
        {
            if (input != null)
            {
                Remove(input);
                Query(Page);
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
        public async void Init(bool IsLoadMore = false)
        {
            try
            {
                if (IsLoadMore) IsBusy = true; else Refresh = true;

                await Task.Delay(Soft.WaitSpan);
                var WallpaperInit = await WallpaperFactory.Wallpaper(async opt =>
                {
                    opt.RequestParam = new WallpaperRequestInput
                    {
                        CacheSpan = Soft.CacheTime,
                        WallpaperType = WallpaperEnum.Init,
                        Init = new WallpaperInit
                        {
                            Page = PageIndex,
                            Limit = Limit,
                            Tag = InitTag()
                        },
                        Proxy = this.Proxy
                    };
                }).RunsAsync();


                this.Total = (WallpaperInit.GlobalResult.Total + Limit - 1) / Limit;
                if (IsLoadMore)
                {
                    IsBusy = false;
                    if (this.Wallpaper == null)
                        this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(WallpaperInit.GlobalResult.Result);
                    else
                    {
                        WallpaperInit.GlobalResult.Result.ForEach(item =>
                        {
                            this.Wallpaper.Add(item);
                        });
                    }
                }
                else
                {
                    Refresh = false;
                    this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(WallpaperInit.GlobalResult.Result);
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
        public async void SearchBaisc(bool IsLoadMore = false)
        {
            try
            {
                if (IsLoadMore) IsBusy = true; else Refresh = true;
                var WallpaperSearch = await WallpaperFactory.Wallpaper(async opt =>
                {
                    opt.RequestParam = new WallpaperRequestInput
                    {
                        CacheSpan = Soft.CacheTime,
                        WallpaperType = WallpaperEnum.Search,
                        Search = new WallpaperSearch
                        {
                            Limit = Limit,
                            Page = PageIndex,
                            KeyWord = InitTag(false)
                        },
                        Proxy = this.Proxy
                    };
                }).RunsAsync();
                this.Total = (WallpaperSearch.GlobalResult.Total + Limit - 1) / Limit;

                if (IsLoadMore)
                {
                    IsBusy = false;
                    if (this.Wallpaper == null)
                        this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(WallpaperSearch.GlobalResult.Result);
                    else
                    {
                        WallpaperSearch.GlobalResult.Result.ForEach(item =>
                        {
                            this.Wallpaper.Add(item);
                        });
                    }
                }
                else
                {
                    Refresh = false;
                    this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(WallpaperSearch.GlobalResult.Result);
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
        public string InitTag(bool Type = true)
        {
            string Tag = string.Empty;
            if (Soft.AgeModule == 1)
                Tag = Soft.S12X;
            else if (Soft.AgeModule == 2)
                Tag = Soft.S15X;
            else if (Soft.AgeModule == 3)
                Tag = Soft.S18X;
            else
                Tag = string.Empty;

            return Type ? Tag : $"{Tag} {KeyWord}";
        }
        public async void Insert(WallpaperResultDetail input)
        {
            var entity = input.ToMapest<BZLiShiDto>();
            await Candy.Insert(entity);
        }
        public async void Query(int PageIndex = 1, bool IsLoadMore = false)
        {
            if (IsLoadMore) IsBusy = true; else Refresh = true;
            await Task.Delay(Soft.WaitSpan);
            var result = await Candy.Query(KeyWord, PageIndex, Limit);
            this.PageTotal = (result.Item2 + Limit - 1) / Limit;
            if (IsLoadMore)
            {
                IsBusy = false;
                if (this.WallpaperLike == null)
                    this.WallpaperLike = new ObservableCollection<WallpaperResultDetail>(result.Item1.ToMapest<List<WallpaperResultDetail>>());
                else
                {
                    result.Item1.ToMapest<List<WallpaperResultDetail>>().ForEach(item =>
                    {
                        this.WallpaperLike.Add(item);
                    });
                }
            }
            else
            {
                Refresh = false;
                this.WallpaperLike = new ObservableCollection<WallpaperResultDetail>(result.Item1.ToMapest<List<WallpaperResultDetail>>());
            }
        }
        public async void Remove(WallpaperResultDetail input)
        {
            await Candy.Remove(input.ToMapest<BZLiShiDto>());
        }
        #endregion
    }
}
