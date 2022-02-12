using CandySugar.CandyWindows.CnadyWinViewModel;
using CandySugar.Common.DTO;
using CandySugar.Core.Service;
using CandySugar.Properties;
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
using Wallpaper.SDK;
using Wallpaper.SDK.ViewModel;
using Wallpaper.SDK.ViewModel.Enums;
using Wallpaper.SDK.ViewModel.Request;
using Wallpaper.SDK.ViewModel.Response;
using XExten.Advance.HttpFramework.MultiCommon;
using XExten.Advance.HttpFramework.MultiFactory;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace CandySugar.UserControlViews.WallpaperViews
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

        public async void Download(long Id)
        {
            var result = Wallpaper.FirstOrDefault(t => t.Id == Id);
            try
            {
                var bytes = (await IHttpMultiClient.HttpMulti
                    .InitWebProxy(this.Proxy.ToMapest<MultiProxy>())
                    .AddNode(t =>
                    {
                        t.NodePath = !result.OriginalPng.IsNullOrEmpty() ? result.OriginalPng : result.OriginalJepg;
                        t.ReqType = MultiType.GET;
                    }).Build().RunBytesAsync()).FirstOrDefault();

                var FileName = (!result.OriginalPng.IsNullOrEmpty() ? result.OriginalPng : result.OriginalJepg).Split("/").LastOrDefault();

                var dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "CandyDown", "Wallpaper", $"{result.Author}"));
                var fn = SyncStatic.CreateFile(Path.Combine(dir, FileName));
                SyncStatic.WriteFile(bytes, fn);
                Process.Start("explorer.exe", dir);
            }
            catch
            {
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
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

            BootResource.View(window =>
            {
                window.DataContext = vm;
            });
        }
        #endregion

        #region Init
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
                var WallpaperInit = await WallpaperFactory.Wallpaper(opt =>
                {
                    opt.RequestParam = new WallpaperRequestInput
                    {
                        CacheSpan = Soft.Default.CacheTime,
                        WallpaperType = WallpaperEnum.Init,
                        Init = new WallpaperInit
                        {
                            Page=PageIndex,
                            Limit = Limit,
                            Tag=Soft.Default.SafeModule?Soft.Default.Safe:String.Empty
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
            catch
            {
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
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
                var WallpaperSearch = await WallpaperFactory.Wallpaper(opt =>
                {
                    opt.RequestParam = new WallpaperRequestInput
                    {
                        CacheSpan = Soft.Default.CacheTime,
                        WallpaperType = WallpaperEnum.Search,
                        Search = new WallpaperSearch
                        {
                            Limit = Limit,
                            Page=PageIndex,
                            KeyWord =  Soft.Default.SafeModule ? $"{Soft.Default.Safe} {KeyWord}" : KeyWord
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
            catch
            {
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
            }
        }
        #endregion
    }
}
