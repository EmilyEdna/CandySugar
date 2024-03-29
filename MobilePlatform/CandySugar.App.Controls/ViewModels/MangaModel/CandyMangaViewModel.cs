﻿using CandySugar.App.Controls.Views.Manga;
using CandySugar.Xam.Common;
using SDKColloction.MangaSDK;
using SDKColloction.MangaSDK.ViewModel;
using SDKColloction.MangaSDK.ViewModel.Emums;
using SDKColloction.MangaSDK.ViewModel.Request;
using SDKColloction.MangaSDK.ViewModel.Response;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XExten.Advance.LinqFramework;
using XF.Material.Forms.UI.Dialogs;
using Prism.Ioc;
using CandySugar.Xam.Core.Service;
using CandySugar.Xam.Common.DTO;

namespace CandySugar.App.Controls.ViewModels.MangaModel
{
    public class CandyMangaViewModel : ViewModelNavigatBase
    {
        public CandyMangaViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = Proxy = new MangaProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
            this.PageIndex = 1;
            this.IsBusy = false;
            CandyLog = Resolve<ILoger>();
        }

        #region Field
        private readonly ILoger CandyLog;
        private readonly MangaProxy Proxy;
        private string KeyWord;
        private string CateWord;
        #endregion

        #region Property
        private ObservableCollection<MangaCategoryResult> _MangaCategory;
        public ObservableCollection<MangaCategoryResult> MangaCategory
        {
            get => _MangaCategory;
            set => SetProperty(ref _MangaCategory, value);
        }

        private ObservableCollection<MangaRecommendResult> _MangaRecommend;
        public ObservableCollection<MangaRecommendResult> MangaRecommend
        {
            get { return _MangaRecommend; }
            set { SetProperty(ref _MangaRecommend, value); }
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

        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { SetProperty(ref _PageIndex, value); }
        }
        #endregion

        #region Command
        public ICommand SearchCommand => new DelegateCommand<string>(input =>
        {
            PageIndex = 1;
            KeyWord = input;
            CateWord = string.Empty;
            Search();
        });

        public ICommand ItemCommand => new DelegateCommand<string>(input =>
        {
            PageIndex = 1;
            CateWord = input;
            KeyWord = string.Empty;
            Category();
        });
        public ICommand RefreshsCommand => new DelegateCommand(() =>
        {
            PageIndex = 1;
            if (KeyWord.IsNullOrEmpty()) Category();
            else Search();

        });
        public ICommand ShowMoreCommand => new DelegateCommand(() =>
        {
            PageIndex +=1;
            if (KeyWord.IsNullOrEmpty()) Category(true);
            else Search(true);
        });

        public ICommand DetailCommand => new DelegateCommand<dynamic>(input =>
        {
            if (input != null)
                Navigation(input);
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
                Location = $"{nameof(CandyMangaViewModel)}_{Method}",
                ErrorMsg = ex.Message,
                ErrorStack = ex.StackTrace
            });
            return String.Format(Soft.Toast,nameof(CandyMangaViewModel), Method);
        }
        public async void Init()
        {
            try
            {
                Refresh = true;
                await Task.Delay(Soft.WaitSpan);
                //初始化
                var MangaInit = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new MangaRequestInput
                    {
                        MangaType = MangaEnum.MangaInit,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.CacheTime,
                    };
                }).RunsAsync();
                Refresh = false;
                MangaCategory = new ObservableCollection<MangaCategoryResult>(MangaInit.IndexCategories);
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Init",ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }

        public async void Category(bool IsLoadMore = false)
        {
            try
            {
                if (IsLoadMore) IsBusy = true; else Refresh = true;
                await Task.Delay(Soft.WaitSpan);
                var MangaCate = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new MangaRequestInput
                    {
                        MangaType = MangaEnum.MangaCategory,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.CacheTime,
                        Category = new MangaCategory
                        {
                            Address = CateWord,
                            Page = PageIndex
                        }
                    };
                }).RunsAsync();
                if (IsLoadMore)
                {
                    IsBusy = false;

                    if (MangaCate.SearchResults.Count == 0)
                        using (await MaterialDialog.Instance.LoadingSnackbarAsync("已经到底了!"))
                        {
                            await Task.Delay(1000);
                        }
                    else
                    {
                        if (MangaRecommend == null)
                            MangaRecommend = new ObservableCollection<MangaRecommendResult>(MangaCate.SearchResults.ToMapest<List<MangaRecommendResult>>());
                        else
                            MangaCate.SearchResults.ToMapest<List<MangaRecommendResult>>().ForEach(item =>
                            {
                                MangaRecommend.Add(item);
                            });
                    }
                }
                else
                {
                    Refresh = false;
                    MangaRecommend = new ObservableCollection<MangaRecommendResult>(MangaCate.SearchResults.ToMapest<List<MangaRecommendResult>>());
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

        public async void Search(bool IsLoadMore = false)
        {
            try
            {
                if (IsLoadMore) IsBusy = true; else Refresh = true;
                await Task.Delay(Soft.WaitSpan);
                var MangaSearch = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new MangaRequestInput
                    {
                        MangaType = MangaEnum.MangaSearch,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.CacheTime,
                        Search = new MangaSearch
                        {
                            KeyWord = KeyWord,
                            Page = PageIndex
                        }
                    };
                }).RunsAsync();

                if (IsLoadMore)
                {
                    if (MangaSearch.SearchResults.Count == 0) 
                        using (await MaterialDialog.Instance.LoadingSnackbarAsync("已经到底了!"))
                        {
                            await Task.Delay(1000);
                        }
                    else
                    {
                        if (MangaRecommend == null)
                            MangaRecommend = new ObservableCollection<MangaRecommendResult>(MangaSearch.SearchResults.ToMapest<List<MangaRecommendResult>>());
                        else
                            MangaSearch.SearchResults.ToMapest<List<MangaRecommendResult>>().ForEach(item =>
                            {
                                MangaRecommend.Add(item);
                            });
                    }
                }
                else
                {
                    Refresh = false;
                    MangaRecommend = new ObservableCollection<MangaRecommendResult>(MangaSearch.SearchResults.ToMapest<List<MangaRecommendResult>>());

                    IsBusy = false;
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

        public async void Navigation(MangaRecommendResult input) 
        {
            var Param = new NavigationParameters();
            Param.Add("Route", input.Address);
            Param.Add("MangaName", input.MangaName);
            Param.Add("Cover", input.Cover);
            await NavigationService.NavigateAsync(new Uri(nameof(CandyMangaChapterView), UriKind.Relative), Param);
        }
        #endregion
    }
}
