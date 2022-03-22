using CandySugar.Xam.Common;
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

namespace CandySugar.App.Controls.ViewModels.KonachanModel
{
    public class CandyKonachanViewModel : ViewModelNavigatBase
    {
        private readonly WallpaperProxy Proxy;
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
        }

        #region Flied
        private string KeyWord;
        private int Limit = 10;
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
        #endregion

        #region Command
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
                var WallpaperInit = await WallpaperFactory.Wallpaper(opt =>
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
                else {
                    Refresh = false;
                    this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(WallpaperInit.GlobalResult.Result);
                }
            }
            catch
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
                var WallpaperSearch = await WallpaperFactory.Wallpaper(opt =>
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
            catch
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
        #endregion
    }
}
