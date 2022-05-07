using CandySugar.Common;
using CandySugar.Common.DTO;
using CandySugar.Core.Service;
using CandySugar.WPF.Properties;
using HandyControl.Controls;
using SDKColloction.MusicSDK;
using SDKColloction.MusicSDK.ViewModel;
using SDKColloction.MusicSDK.ViewModel.Enums;
using SDKColloction.MusicSDK.ViewModel.Request;
using SDKColloction.MusicSDK.ViewModel.Response;
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
using System.Windows.Controls;
using XExten.Advance.HttpFramework.MultiFactory;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;
using TabControl = HandyControl.Controls.TabControl;
using TabItem = HandyControl.Controls.TabItem;

namespace CandySugar.WPF.UserControlViews.MusicViews
{
    public class MusicViewModel : Screen
    {
        private readonly IContainer Container;
        private readonly IYinYue YinYue;
        private readonly MusicProxy Proxy;
        public MusicViewModel(IContainer Container)
        {
            this.Container = Container;
            this.YinYue = Container.Get<IYinYue>();
            this.Proxy = new MusicProxy
            {
                IP = Soft.Default.ProxyIP,
                Port = Soft.Default.ProxyPort,
                PassWord = Soft.Default.ProxyPwd,
                UserName = Soft.Default.ProxyAccount
            };
            this.PageIndex = this.DanQu_Page = this.GeDan_Page = 1;
            this.Platform = MusicPlatformEnum.NeteaseMusic;
        }

        #region Field
        /// <summary>
        /// 关键字
        /// </summary>
        private string KeyWord;
        /// <summary>
        /// 查询类型 1 表示单曲 2 表示歌单
        /// </summary>
        private int ShowType;

        private int DanQu_Page;
        private int GeDan_Page;
        private TabControl Tab;
        #endregion

        #region Property
        private ObservableCollection<MusicSongItem> _SongItems;
        public ObservableCollection<MusicSongItem> SongItems
        {
            get { return _SongItems; }
            set { SetAndNotify(ref _SongItems, value); }
        }

        private ObservableCollection<MusicSongSheetItem> _SongSheets;
        public ObservableCollection<MusicSongSheetItem> SongSheets
        {
            get { return _SongSheets; }
            set { SetAndNotify(ref _SongSheets, value); }
        }

        private MusicSongSheetDetailResult _SheetDetail;
        public MusicSongSheetDetailResult SheetDetail
        {
            get { return _SheetDetail; }
            set { SetAndNotify(ref _SheetDetail, value); }
        }

        private MusicSongAlbumDetailResult _AlbumDetail;
        public MusicSongAlbumDetailResult AlbumDetail
        {
            get { return _AlbumDetail; }
            set { SetAndNotify(ref _AlbumDetail, value); }
        }

        private ObservableCollection<CandyPlayListDto> _PlayLists;
        public ObservableCollection<CandyPlayListDto> PlayLists
        {
            get { return _PlayLists; }
            set { SetAndNotify(ref _PlayLists, value); }
        }

        private int _Total;
        public int Total
        {
            get { return _Total; }
            set { SetAndNotify(ref _Total, value); }
        }

        private int _Count;
        public int Count
        {
            get { return _Count; }
            set { SetAndNotify(ref _Count, value); }
        }

        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { SetAndNotify(ref _PageIndex, value); }
        }

        private MusicPlatformEnum _Platform;
        public MusicPlatformEnum Platform
        {
            get { return _Platform; }
            set { SetAndNotify(ref _Platform, value); }
        }
        #endregion

        #region Command
        public void SearchType(ComboBoxItem control)
        {
            this.Platform = (MusicPlatformEnum)control.TabIndex;
            SheetDetail = null;
            AlbumDetail = null;
        }

        public void SearchMusic(string input)
        {
            KeyWord = input;
            SearchBasic();
        }

        public void ScrollChanged(Dictionary<string, int> input)
        {
            if (input.Keys.FirstOrDefault().Equals("单曲"))
            {
                PageIndex = (DanQu_Page += input.Values.FirstOrDefault());
                if (PageIndex <= 0)
                    return;
                if (PageIndex < Total)
                    SearchBasic(ShowType);
            }
            if (input.Keys.FirstOrDefault().Equals("歌单"))
            {
                PageIndex = (GeDan_Page += input.Values.FirstOrDefault());
                if (PageIndex <= 0)
                    return;
                if (PageIndex < Total)
                    SearchBasic(ShowType);
            }
        }

        public void TabChanged(TabItem input)
        {

            Tab = (TabControl)input.Parent;
            var Type = Tab.SelectedIndex;
            if (Type == 0)
            {
                PageIndex = DanQu_Page;
                if (KeyWord != null)
                    SearchBasic(1);

            }
            if (Type == 1)
            {
                PageIndex = GeDan_Page;
                if (KeyWord != null)
                    SearchBasic(2);
            }
        }

        public async void SeleteSheet(MusicSongSheetItem entity)
        {
            if (entity == null)
                return;
            try
            {
                Tab.SelectedIndex = 2;

                HelpUtilty.WirteLog("歌单详情操作");
                var SheetDetail = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new MusicRequestInput
                    {
                        Proxy = this.Proxy,
                        MusicPlatformType = Platform,
                        MusicType = MusicTypeEnum.MusicSheetDetail,
                        SheetSearch = new MusicSheetSearch
                        {
                            Page = PageIndex,
                            Id = entity.SongSheetId.AsString()
                        }
                    };
                }).RunsAsync();

                this.SheetDetail = SheetDetail.SongSheetDetailResult;
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }
        }

        public async void ShowAlbum(string input)
        {
            try
            {
                HelpUtilty.WirteLog("专辑详情操作");
                var SongAlbum = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new MusicRequestInput
                    {
                        Proxy = this.Proxy,
                        MusicPlatformType = Platform,
                        MusicType = MusicTypeEnum.MusicAlbumDetail,
                        AlbumSearch = new MusicAlbumSearch
                        {
                            AlbumId = input
                        }
                    };
                }).RunsAsync();

                Tab.SelectedIndex = 3;

                this.AlbumDetail = SongAlbum.SongAlbumDetailResult;
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }
        }

        public async void AddPlayList(MusicSongItem input)
        {
            try
            {
                HelpUtilty.WirteLog("音乐播放操作");

                var SongURL = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new MusicRequestInput
                    {
                        Proxy = this.Proxy,
                        MusicPlatformType = this.Platform,
                        MusicType = MusicTypeEnum.MusicPlayAddress,
                        AddressSearch = new MusicPlaySearch
                        {
                            Dynamic = input.SongId,
                            KuGouAlbumId = input.SongAlbumId,
                        }
                    };
                }).RunsAsync();

                if (SongURL.SongPlayAddressResult.CanPlay == false)
                {
                    Growl.Info("当前歌曲已下架，请切换到其他其他平台搜索");
                    return;
                }

                var tuple = MusicCache(input, SongURL.SongPlayAddressResult);

                await this.YinYue.AddPlayList(new CandyPlayListDto
                {
                    Address = SongURL.SongPlayAddressResult.SongURL,
                    SongAlbum = input.SongAlbumName,
                    SongName = input.SongName,
                    SongArtist = tuple.Item1,
                    SongId = input.SongId,
                    CacheAddress = tuple.Item2,
                    Platform = (int)this.Platform
                });

                Launch();
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }
        }

        public void DownMusic(CandyPlayListDto input)
        {
            var index = input.CacheAddress.LastIndexOf("\\");
            var path = input.CacheAddress.Substring(0, index);
            Process.Start("explorer.exe", path);
        }

        public async void DeleteMusic(CandyPlayListDto input)
        {
            await this.YinYue.RemovePlayList(input.PId);
            SyncStatic.DeleteFile(input.CacheAddress);
            Launch();
        }

        public async void ClearMusic()
        {
            await this.YinYue.ClearPlayList();
            SyncStatic.DeleteFolder(SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "CandyDown", "Music")));
            Launch();
        }
        #endregion

        #region Method
        protected override void OnViewLoaded()
        {
            Launch();
        }
        public async Task<MusicLyricResult> LoadLyric(CandyPlayListDto input)
        {
            if (input == null)
            {
                Growl.Info("当前没有歌曲在播放中~~~"); return null;
            }
            var data = await this.YinYue.GetLyrics(input.SongId, input.Platform);
            if (data != null)
            {
                return new MusicLyricResult
                {
                    Lyrics = data.ToMapest<List<MusicLyricItemResult>>()
                };
            }
            else
            {
                var SongLyric = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new MusicRequestInput
                    {

                        MusicPlatformType = (MusicPlatformEnum)input.Platform,
                        Proxy = this.Proxy,
                        MusicType = MusicTypeEnum.MusicLyric,
                        LyricSearch = new MusicLyricSearch
                        {
                            Dynamic = input.SongId
                        }
                    };
                }).RunsAsync();
                if (SongLyric.SongLyricResult.Lyrics != null)
                {
                    var lyric = string.Join("_", SongLyric.SongLyricResult.Lyrics.Select(t => $"{t.Time}|{t.Lyric}"));
                    await this.YinYue.AddLyric(input.SongId, input.Platform, lyric);
                }
                return SongLyric.SongLyricResult;
            }
        }
        public T GetContainer<T>()
        {
            return Container.Get<T>();
        }
        #endregion

        #region Function
        private async void SearchBasic(int type = 1)
        {
            ShowType = type;
            HelpUtilty.WirteLog("音乐查询操作");
            switch (Platform)
            {
                case MusicPlatformEnum.QQMusic:
                    {
                        if (type == 1)
                        {
                            try
                            {
                                //单曲
                                var SongItem = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.QQMusic,
                                        MusicType = MusicTypeEnum.MusicSongItem,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();

                                this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                                this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                                this.Total = SongItem.SongItemResult.Total.Value;
                            }
                            catch (Exception ex)
                            {
                                HelpUtilty.WirteLog(string.Empty, ex);
                                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
                            }
                        }
                        else
                        {
                            try
                            {
                                //歌单
                                var SongSheet = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.QQMusic,
                                        MusicType = MusicTypeEnum.MusicSongSheet,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();

                                this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                                this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                                this.Total = SongSheet.SongSheetResult.Total.Value;
                            }
                            catch (Exception ex)
                            {
                                HelpUtilty.WirteLog(string.Empty, ex);
                                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
                            }
                        }
                        break;
                    }
                case MusicPlatformEnum.NeteaseMusic:
                    {
                        if (type == 1)
                        {
                            try
                            {
                                //单曲
                                var SongItem = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.NeteaseMusic,
                                        MusicType = MusicTypeEnum.MusicSongItem,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();

                                this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                                this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                                this.Total = SongItem.SongItemResult.Total.Value;
                            }
                            catch (Exception ex)
                            {
                                HelpUtilty.WirteLog(string.Empty, ex);
                                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
                            }
                        }
                        else
                        {
                            try
                            {
                                //歌单
                                var SongSheet = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.NeteaseMusic,
                                        MusicType = MusicTypeEnum.MusicSongSheet,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();

                                this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                                this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                                this.Total = SongSheet.SongSheetResult.Total.Value;
                            }
                            catch (Exception ex)
                            {
                                HelpUtilty.WirteLog(string.Empty, ex);
                                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
                            }
                        }
                        break;
                    }
                case MusicPlatformEnum.KuGouMusic:
                    {
                        if (type == 1)
                        {
                            try
                            {
                                //单曲
                                var SongItem = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.KuGouMusic,
                                        MusicType = MusicTypeEnum.MusicSongItem,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();

                                this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                                this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                                this.Total = SongItem.SongItemResult.Total.Value;
                            }
                            catch (Exception ex)
                            {
                                HelpUtilty.WirteLog(string.Empty, ex);
                                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
                            }
                        }
                        else
                        {
                            try
                            {
                                //歌单
                                var SongSheet = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.KuGouMusic,
                                        MusicType = MusicTypeEnum.MusicSongSheet,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();

                                this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                                this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                                this.Total = SongSheet.SongSheetResult.Total.Value;
                            }
                            catch (Exception ex)
                            {
                                HelpUtilty.WirteLog(string.Empty, ex);
                                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
                            }
                        }
                        break;
                    }
                case MusicPlatformEnum.KuWoMusic:
                    {
                        if (type == 1)
                        {
                            try
                            {
                                //单曲
                                var SongItem = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.KuWoMusic,
                                        MusicType = MusicTypeEnum.MusicSongItem,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();

                                this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                                this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                                this.Total = SongItem.SongItemResult.Total.Value;
                            }
                            catch (Exception ex)
                            {
                                HelpUtilty.WirteLog(string.Empty, ex);
                                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
                            }
                        }
                        else
                        {
                            try
                            {
                                //歌单
                                var SongSheet = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.KuWoMusic,
                                        MusicType = MusicTypeEnum.MusicSongSheet,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();

                                this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                                this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                                this.Total = SongSheet.SongSheetResult.Total.Value;
                            }
                            catch (Exception ex)
                            {
                                HelpUtilty.WirteLog(string.Empty, ex);
                                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
                            }
                        }
                        break;
                    }
                case MusicPlatformEnum.BiliBiliMusic:
                    {
                        if (type == 1)
                        {
                            try
                            {
                                //单曲
                                var SongItem = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.BiliBiliMusic,
                                        MusicType = MusicTypeEnum.MusicSongItem,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();

                                this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                                this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                                this.Total = SongItem.SongItemResult.Total.Value;
                            }
                            catch (Exception ex)
                            {
                                HelpUtilty.WirteLog(string.Empty, ex);
                                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
                            }
                        }
                        else
                        {
                            try
                            {
                                //歌单
                                var SongSheet = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.BiliBiliMusic,
                                        MusicType = MusicTypeEnum.MusicSongSheet,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();

                                this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                                this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                                this.Total = SongSheet.SongSheetResult.Total.Value;
                            }
                            catch (Exception ex)
                            {
                                HelpUtilty.WirteLog(string.Empty, ex);
                                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
                            }
                        }
                        break;
                    }
                case MusicPlatformEnum.MiGuMusic:
                    {
                        if (type == 1)
                        {
                            try
                            {
                                //单曲
                                var SongItem = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.MiGuMusic,
                                        MusicType = MusicTypeEnum.MusicSongItem,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();

                                this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                                this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                                this.Total = SongItem.SongItemResult.Total.Value;
                            }
                            catch (Exception ex)
                            {
                                HelpUtilty.WirteLog(string.Empty, ex);
                                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
                            }
                        }
                        else
                        {
                            try
                            {
                                //歌单
                                var SongSheet = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.MiGuMusic,
                                        MusicType = MusicTypeEnum.MusicSongSheet,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();

                                this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                                this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                                this.Total = SongSheet.SongSheetResult.Total.Value;
                            }
                            catch (Exception ex)
                            {
                                HelpUtilty.WirteLog(string.Empty, ex);
                                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
                            }
                        }
                        break;
                    }
                default:
                    throw new NullReferenceException();
            }
        }
        private Tuple<string, string> MusicCache(MusicSongItem input, MusicSongPlayAddressResult song)
        {
            string CacheAddress = string.Empty;
            var SongArtist = string.Join(",", input.SongArtistName);
            var dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "CandyDown", "Music", $"{SongArtist}"));
            var SongFile = $"{input.SongName}({input.SongAlbumName})-{SongArtist}_{Platform}.mp3";

            if (this.Platform == MusicPlatformEnum.BiliBiliMusic)
            {
                var file = SyncStatic.CreateFile(Path.Combine(dir, SongFile));
                CacheAddress = SyncStatic.WriteFile(song.BilibiliFileBytes, file);
            }
            else
            {
                var file = SyncStatic.CreateFile(Path.Combine(dir, SongFile));
                var filebytes = IHttpMultiClient.HttpMulti.AddNode(opt => opt.NodePath = song.SongURL).Build().RunBytes().FirstOrDefault();
                CacheAddress = SyncStatic.WriteFile(filebytes, file);
            }
            return new Tuple<string, string>(SongArtist, CacheAddress);
        }
        private async void Launch()
        {
            var Result = await this.YinYue.GetPlayList();
            this.Count = Result.Count;
            this.PlayLists = new ObservableCollection<CandyPlayListDto>(Result);
        }
        #endregion
    }
}
