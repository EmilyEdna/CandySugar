﻿using CandySugar.Xam.Common;
using SDKColloction.NovelSDK;
using SDKColloction.NovelSDK.ViewModel;
using SDKColloction.NovelSDK.ViewModel.Enums;
using SDKColloction.NovelSDK.ViewModel.Request;
using SDKColloction.NovelSDK.ViewModel.Response;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;
using XF.Material.Forms.UI.Dialogs;
using Prism.Ioc;
using CandySugar.App.Controls.Views.Novel;
using CandySugar.Xam.Core.Service;
using CandySugar.Xam.Common.DTO;

namespace CandySugar.App.Controls.ViewModels.NovelModel
{
    public class CandyNovelViewModel : ViewModelNavigatBase
    {
        private readonly NovelProxy Proxy;
        private readonly ILoger CandyLog;
        public CandyNovelViewModel(INavigationService navigationService) : base(navigationService)
        {
            CandyLog = Resolve<ILoger>();
            Proxy = new NovelProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
        }

        #region Field
        private string CategoryType;
        #endregion

        #region Override
        protected override void OnViewLaunch()
        {
            Init();
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

        public ICommand DetailCommand => new DelegateCommand<dynamic>(input =>
        {
            Navigation(input);
        });
        #endregion

        #region Method
        private async Task<string> Tip(string Method, Exception ex)
        {
            await CandyLog.Insert(new CandyGlobalLogDto
            {
                Location = $"{nameof(CandyNovelViewModel)}_{Method}",
                ErrorMsg = ex.Message,
                ErrorStack = ex.StackTrace
            });
            return String.Format(Soft.Toast,nameof(CandyNovelViewModel), Method);
        }
        private async void Init()
        {
            try
            {
                this.Refresh = true;
                await Task.Delay(Soft.WaitSpan);
                var NovelInit = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.CacheTime,
                        NovelType = NovelEnum.NovelInit,
                        Proxy = this.Proxy
                    };
                }).RunsAsync();
                this.Refresh = false;
                this.NovelCategory = new ObservableCollection<NovelCategoryResult>(NovelInit.IndexCategories);
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Init",ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }
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
                        NovelType = NovelEnum.NovelCategory,
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
            catch (Exception ex)
            {
                this.IsBusy = false;
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Category",ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }
        private async void Refreshs(string input)
        {
            if (input.IsNullOrEmpty())
            {
                this.Refresh = false;
                return;
            }
            CategoryType = input;
            this.Refresh = true;
            try
            {
                await Task.Delay(Soft.WaitSpan);
                var NovelCate = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.CacheTime,
                        NovelType = NovelEnum.NovelCategory,
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
            catch (Exception ex)
            {
                this.Refresh = false;
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Refreshs",ex)))
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
                NovelSearchResult inputKey = null;
                if (input is NovelSearchResult)
                {
                     inputKey = (input as NovelSearchResult);
                }
                this.Refresh = true;
                await Task.Delay(Soft.WaitSpan);

                var NovelSearch = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.CacheTime,
                        NovelType = NovelEnum.NovelSearch,
                        Proxy = this.Proxy,
                        Search = new NovelSearch
                        {
                            NovelSearchKeyWord = inputKey==null?input:inputKey?.Author
                        }
                    };
                }).RunsAsync();
                this.NovelSearch = new ObservableCollection<NovelSearchResult>(NovelSearch.SearchResults);

                this.PageIndex = 1;
                this.Total = 0;
                this.Refresh = false;
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Search",ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }
        private async void Navigation(dynamic input)
        {
            if (input == null)
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("还未选择浏览的书籍~"))
                {
                    await Task.Delay(3000);
                }

            NovelSearchResult? inputKey = (input as NovelSearchResult);

            var Param = new NavigationParameters();
            Param.Add(nameof(NovelSearchResult), inputKey);
            await NavigationService.NavigateAsync(new Uri(nameof(CandyNovelDetailView), UriKind.Relative), Param);
        }
        #endregion

    }
}
