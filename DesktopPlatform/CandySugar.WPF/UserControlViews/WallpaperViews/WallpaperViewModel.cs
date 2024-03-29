﻿using CandySugar.WPF.CandyWindows.CandyWinViewModel;
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDKColloction.WallpaperSDK;
using SDKColloction.WallpaperSDK.ViewModel;
using SDKColloction.WallpaperSDK.ViewModel.Enums;
using SDKColloction.WallpaperSDK.ViewModel.Request;
using SDKColloction.WallpaperSDK.ViewModel.Response;
using XExten.Advance.HttpFramework.MultiCommon;
using XExten.Advance.HttpFramework.MultiFactory;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace CandySugar.WPF.UserControlViews.WallpaperViews
{
    public class WallpaperViewModel : Screen
    {
        private string KeyWord;
        private readonly int Limit = 16;
        private readonly IContainer Container;
        private readonly IBiZhi BiZhi;
        private readonly WallpaperProxy Proxy;
        public WallpaperViewModel(IContainer Container)
        {
            this.Container = Container;
            this.BiZhi = Container.Get<IBiZhi>();
            this.Proxy = new WallpaperProxy
            {
                IP = Soft.Default.ProxyIP,
                Port = Soft.Default.ProxyPort,
                PassWord = Soft.Default.ProxyPwd,
                UserName = Soft.Default.ProxyAccount
            };
        }

        #region Property
        private ObservableCollection<WallpaperResultDetail> _Wallpaper;
        public ObservableCollection<WallpaperResultDetail> Wallpaper
        {
            get { return _Wallpaper; }
            set { SetAndNotify(ref _Wallpaper, value); }
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

        private bool _WatchFavorite;
        public bool WatchFavorite
        {
            get { return _WatchFavorite; }
            set { SetAndNotify(ref _WatchFavorite, value); }
        }
        #endregion

        #region Command
        public void Check()
        {
            WatchFavorite = true;
            InitFavorite(string.Empty);
        }

        public async void UnCheck()
        {
            WatchFavorite = false;

            var favoriteId = await BiZhi.GetAllFavorite();

            PageIndex = 1;

            InitAll(favoriteId);
        }

        public async void Search(string input)
        {
            if (WatchFavorite)
            {
                InitFavorite(input);
            }
            else
            {
                var favoriteId = await BiZhi.GetAllFavorite();

                KeyWord = input;

                PageIndex = 1;

                SearchBasic(favoriteId);
            }
        }

        public async void PageUpdated(FunctionEventArgs<int> args)
        {
            PageIndex = args.Info;
            if (WatchFavorite)
                InitFavorite(KeyWord);
            else
            {
                var favoriteId = await BiZhi.GetAllFavorite();
                if (KeyWord.IsNullOrEmpty())
                    InitAll(favoriteId);
                else
                    SearchBasic(favoriteId);
            }
        }

        public async Task Download(long Id)
        {
            var result = Wallpaper.FirstOrDefault(t => t.Id == Id);
            try
            {
                HelpUtilty.WirteLog("壁纸下载操作");
                var WallpaperDown = await WallpaperFactory.Wallpaper(opt =>
                {
                    opt.RequestParam = new WallpaperRequestInput
                    {
                        WallpaperType = WallpaperEnum.ImgDownload,
                        CacheSpan = Soft.Default.CacheTime,
                        Download = new WallpaperDownload()
                        {
                            Route = !result.OriginalPng.IsNullOrEmpty() ? result.OriginalPng : result.OriginalJepg
                        },
                        Proxy = this.Proxy
                    };
                }).RunsAsync();

                var FileName = (!result.OriginalPng.IsNullOrEmpty() ? result.OriginalPng : result.OriginalJepg).Split("/").LastOrDefault();

                var dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "CandyDown", "Wallpaper", $"{result.Author}"));
                var fn = SyncStatic.CreateFile(Path.Combine(dir, FileName));
                SyncStatic.WriteFile(WallpaperDown.DownloadResult.Bytes, fn);
                Process.Start("explorer.exe", dir);
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }
        }

        public void NoFavorite(long Id)
        {
            var dto = this.Wallpaper.FirstOrDefault(t => t.Id == Id).ToMapest<CandyWallpaperDto>();
            BiZhi.AddFavorite(dto);

            var Temp = this.Wallpaper.ToList();

            Temp.FirstOrDefault(t => t.Id == Id).IsFavorite = true;

            this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(Temp);

        }

        public void Favorite(long Id)
        {
            BiZhi.RemoveFavorite(Id);
            var Temp = this.Wallpaper.ToList();

            Temp.FirstOrDefault(t => t.Id == Id).IsFavorite = false;

            this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(Temp);
        }

        public void Preview(long Id)
        {
            var vm = Container.Get<CandyPreviewViewModel>();
            var wallpaper = Wallpaper.FirstOrDefault(t => t.Id == Id);
            vm.FileURL = wallpaper.OriginalPng.IsNullOrEmpty() ? wallpaper.FileSizeJepg : wallpaper.OriginalPng;
            vm.Loading = System.Windows.Visibility.Hidden;
            BootResource.View(window =>
            {
                window.DataContext = vm;
            });
        }
        #endregion

        #region Init

        protected string InitTag(bool Type = true)
        {
            string Tag = string.Empty;
            if (Soft.Default.Module == 2)
                Tag = Soft.Default.S12X;
            else if (Soft.Default.Module == 3)
                Tag = Soft.Default.S15X;
            else if (Soft.Default.Module == 4)
                Tag = Soft.Default.S18X;
            else
                Tag = string.Empty;

            return Type ? Tag : $"{Tag} {KeyWord}";
        }

        protected override async void OnViewLoaded()
        {
            var favoriteId = await BiZhi.GetAllFavorite();

            PageIndex = 1;

            InitAll(favoriteId);
        }
        protected async void InitAll(List<long> favoriteId)
        {
            try
            {
                HelpUtilty.WirteLog("壁纸初始化操作");
                var WallpaperInit = await WallpaperFactory.Wallpaper(opt =>
                {
                    opt.RequestParam = new WallpaperRequestInput
                    {
                        CacheSpan = Soft.Default.CacheTime,
                        WallpaperType = WallpaperEnum.ImgInit,
                        Init = new WallpaperInit
                        {
                            Page = PageIndex,
                            Limit = Limit,
                            Tag = InitTag()
                        },
                        Proxy = this.Proxy
                    };
                }).RunsAsync();
                if (favoriteId.Count > 0)
                    WallpaperInit.GlobalResult.Result.ForEach(t =>
                    {
                        if (favoriteId.Contains(t.Id))
                            t.IsFavorite = true;
                    });

                this.Total = (WallpaperInit.GlobalResult.Total + Limit - 1) / Limit;
                this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(WallpaperInit.GlobalResult.Result);
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }
        }
        protected async void InitFavorite(string input)
        {
            var data = await BiZhi.GetFavorite(input, PageIndex);
            this.Total = data.Total;
            this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(data.Result.ToMapest<List<WallpaperResultDetail>>());
        }
        protected async void SearchBasic(List<long> favoriteId)
        {
            try
            {
                HelpUtilty.WirteLog("壁纸查询操作");
                var WallpaperSearch = await WallpaperFactory.Wallpaper(opt =>
                {
                    opt.RequestParam = new WallpaperRequestInput
                    {
                        CacheSpan = Soft.Default.CacheTime,
                        WallpaperType = WallpaperEnum.ImgSearch,
                        Search = new WallpaperSearch
                        {
                            Limit = Limit,
                            Page = PageIndex,
                            KeyWord = InitTag(false)
                        },
                        Proxy = this.Proxy
                    };
                }).RunsAsync();

                if (favoriteId.Count > 0)
                    WallpaperSearch.GlobalResult.Result.ForEach(t =>
                    {
                        if (favoriteId.Contains(t.Id))
                            t.IsFavorite = true;
                    });
                this.Total = (WallpaperSearch.GlobalResult.Total + Limit - 1) / Limit;
                this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(WallpaperSearch.GlobalResult.Result);
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
