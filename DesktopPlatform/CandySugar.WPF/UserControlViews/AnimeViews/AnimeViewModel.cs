﻿using SDKColloction.AnimeSDK;
using SDKColloction.AnimeSDK.ViewModel;
using SDKColloction.AnimeSDK.ViewModel.Enums;
using SDKColloction.AnimeSDK.ViewModel.Request;
using SDKColloction.AnimeSDK.ViewModel.Response;
using CandySugar.WPF.CandyWindows.CandyWinViewModel;
using CandySugar.Common;
using CandySugar.Common.DTO;
using CandySugar.Core.Service;
using CandySugar.WPF.Properties;
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
using System.Windows.Media;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace CandySugar.WPF.UserControlViews.AnimeViews
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
        protected async override void OnViewLoaded()
        {
            try
            {
                HelpUtilty.WirteLog("初始化动漫操作");

                var AnimeInit = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new AnimeRequestInput
                    {
                        AnimeType = AnimeEnum.AnimeInit,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.Default.CacheTime
                    };
                }).RunsAsync();
                this.RecommendCategory = AnimeInit.RecommendCategory;
                this.WeekDay = new ObservableCollection<AnimeWeekDayResult>(AnimeInit.WeekDays);
            }
            catch(Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }
        }
        #endregion

        #region Action
        public async void SearchAnime(string args)
        {
            try
            {
                HelpUtilty.WirteLog("查询动漫操作");
                SearchKey = args;
                CategoryKey = string.Empty;
                var AnimeSearch = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new AnimeRequestInput
                    {
                        AnimeType = AnimeEnum.AnimeSearch,
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
            }
            catch(Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }
        }

        public async void Redirect(string args)
        {
            try
            {
                HelpUtilty.WirteLog("查看动漫详情操作");
                var AnimeDetail = await AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new AnimeRequestInput
                    {
                        AnimeType = AnimeEnum.AnimeDetail,
                        CacheSpan = Soft.Default.CacheTime,
                        Proxy = this.Proxy,
                        Detail = new AnimeDetail
                        {
                            DetailAddress = args
                        }
                    };
                }).RunsAsync();
                this.Detail = new ObservableCollection<AnimeDetailResult>(AnimeDetail.DetailResults);
            }
            catch(Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }
        }

        public async void Category(string args)
        {
            SearchKey = string.Empty;
            CategoryKey = args;
            if (this.LetterCate.Contains(CategoryKey))
            {
                try
                {
                    HelpUtilty.WirteLog("查询动漫字母分类操作");
                    var AnimeCate = await AnimeFactory.Anime(opt =>
                    {
                        opt.RequestParam = new AnimeRequestInput
                        {
                            CacheSpan = Soft.Default.CacheTime,
                            AnimeType = AnimeEnum.AnimeCategory,
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
                }
                catch(Exception ex)
                {
                    HelpUtilty.WirteLog(string.Empty, ex);
                    Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
                }
            }
            else
            {
                try
                {
                    HelpUtilty.WirteLog("查询动漫分类操作");
                    var AnimeCateType = await AnimeFactory.Anime(opt =>
                    {
                        opt.RequestParam = new AnimeRequestInput
                        {
                            AnimeType = AnimeEnum.AnimeCategoryType,
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
                }
                catch(Exception ex)
                {
                    HelpUtilty.WirteLog(string.Empty, ex);
                    Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
                }
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

        public async void Play(string args)
        {
            try
            {
                HelpUtilty.WirteLog("动漫播放操作");
                var AnimeWath = await AnimeFactory.Anime(opt =>
                 {
                     opt.RequestParam = new AnimeRequestInput
                     {
                         AnimeType = AnimeEnum.AnimeWatch,
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
                    vm.Loading = System.Windows.Visibility.Hidden;
                    CandyAnimeHistoryDto DTO = AnimeWath.PlayResult.ToMapest<CandyAnimeHistoryDto>();
                    DTO.PlayMode = false;
                    BootResource.AnimeVLC(window =>
                    {
                        window.DataContext = vm;
                    });
                    Container.Get<ILiShi>().AddAnimeHistory(DTO);
                }
                if (Soft.Default.PlayBox == 1)
                {
                    var vm = Container.Get<CandyDPlayViewModel>();
                    vm.WatchResult = AnimeWath.PlayResult;
                    vm.Loading = System.Windows.Visibility.Hidden;
                    CandyAnimeHistoryDto DTO = AnimeWath.PlayResult.ToMapest<CandyAnimeHistoryDto>();
                    DTO.PlayMode = true;
                    BootResource.AnimeWEB(window =>
                    {
                        window.DataContext = vm;
                    });
                    Container.Get<ILiShi>().AddAnimeHistory(DTO);
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
