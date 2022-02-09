using CandySugar.Properties;
using HandyControl.Controls;
using Music.SDK;
using Music.SDK.ViewModel;
using Music.SDK.ViewModel.Enums;
using Music.SDK.ViewModel.Request;
using Music.SDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;
using TabControl = HandyControl.Controls.TabControl;
using TabItem = HandyControl.Controls.TabItem;

namespace CandySugar.UserControlViews.MusicViews
{
    public class MusicViewModel : Screen
    {
        private readonly IContainer Container;
        private readonly MusicProxy Proxy;
        public MusicViewModel(IContainer Container)
        {
            this.Container = Container;
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
                SearchBasic(1);
            }
            if (Type == 1)
            {
                PageIndex = GeDan_Page;
                SearchBasic(2);
            }
        }

        public void SeleteSheet(MusicSongSheetItem entity)
        {
            if (entity == null)
                return;
            var SheetDetail =  SyncStatic.TryCatch(() =>
              {
                  var SheetDetail = MusicFactory.Music(opt =>
                 {
                     opt.RequestParam = new MusicRequestInput
                     {
                         Proxy = this.Proxy,
                         MusicPlatformType = Platform,
                         MusicType = MusicTypeEnum.SheetDetail,
                         SheetSearch = new MusicSheetSearch
                         {
                             Page = PageIndex,
                             Id = entity.SongSheetId.AsString()
                         }
                     };
                 }).Runs();
                  return SheetDetail;
              }, ex => { MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"); return null; });
            if (SheetDetail == null)
            {
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                return;
            }

            Tab.SelectedIndex = 2;

            this.SheetDetail = SheetDetail.SongSheetDetailResult;
        }

        public void ShowAlbum(string input) 
        {
            var SongAlbum = SyncStatic.TryCatch(() =>
            {
                var SongAlbum = MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new MusicRequestInput
                    {
                        Proxy = this.Proxy,
                        MusicPlatformType = Platform,
                        MusicType = MusicTypeEnum.AlbumDetail,
                        AlbumSearch = new MusicAlbumSearch
                        {
                            AlbumId = input
                        }
                    };
                }).Runs();
                return SongAlbum;
            }, ex => { MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"); return null; });
            if (SongAlbum == null)
            {
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                return;
            }

            Tab.SelectedIndex = 3;

            this.AlbumDetail = SongAlbum.SongAlbumDetailResult;
        }
        #endregion

        #region Function
        private async Task SearchBasic(int type = 1)
        {
            ShowType = type;
            switch (Platform)
            {
                case MusicPlatformEnum.QQMusic:
                    {
                        if (type == 1)
                        {
                            var SongItem = await SyncStatic.TryCatch(async () =>
                              {
                                  //单曲
                                  var SongItem = await MusicFactory.Music(opt =>
                                    {
                                        opt.RequestParam = new MusicRequestInput
                                        {
                                            Proxy = this.Proxy,
                                            MusicPlatformType = MusicPlatformEnum.QQMusic,
                                            MusicType = MusicTypeEnum.SongItem,
                                            Search = new MusicSearch
                                            {
                                                Page = PageIndex,
                                                KeyWord = KeyWord
                                            }
                                        };
                                    }).RunsAsync();
                                  return SongItem;
                              }, ex => { MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"); return null; });

                            if (SongItem == null)
                            {
                                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                                return;
                            }

                            this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                            this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                            this.Total = SongItem.SongItemResult.Total.Value;
                        }
                        else
                        {
                            var SongSheet = await SyncStatic.TryCatch(async () =>
                            {
                                //歌单
                                var SongSheet = await MusicFactory.Music(opt =>
                                 {
                                     opt.RequestParam = new MusicRequestInput
                                     {
                                         Proxy = this.Proxy,
                                         MusicPlatformType = MusicPlatformEnum.QQMusic,
                                         MusicType = MusicTypeEnum.SongSheet,
                                         Search = new MusicSearch
                                         {
                                             Page = PageIndex,
                                             KeyWord = KeyWord
                                         }
                                     };
                                 }).RunsAsync();
                                return SongSheet;
                            }, ex => { MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"); return null; });

                            if (SongSheet == null)
                            {
                                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                                return;
                            }

                            this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                            this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                            this.Total = SongSheet.SongSheetResult.Total.Value;
                        }
                        break;
                    }
                case MusicPlatformEnum.NeteaseMusic:
                    {
                        if (type == 1)
                        {
                            var SongItem = await SyncStatic.TryCatch(async () =>
                            {
                                //单曲
                                var SongItem = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.NeteaseMusic,
                                        MusicType = MusicTypeEnum.SongItem,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();
                                return SongItem;
                            }, ex =>
                            {
                                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                                return null;
                            });

                            if (SongItem == null)
                            {
                                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                                return;
                            }

                            this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                            this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                            this.Total = SongItem.SongItemResult.Total.Value;
                        }
                        else
                        {
                            var SongSheet = await SyncStatic.TryCatch(async () =>
                            {
                                //歌单
                                var SongSheet = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.NeteaseMusic,
                                        MusicType = MusicTypeEnum.SongSheet,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();
                                return SongSheet;
                            }, ex => { MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"); return null; });

                            if (SongSheet == null)
                            {
                                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                                return;
                            }

                            this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                            this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                            this.Total = SongSheet.SongSheetResult.Total.Value;
                        }
                        break;
                    }
                case MusicPlatformEnum.KuGouMusic:
                    {
                        if (type == 1)
                        {
                            var SongItem = await SyncStatic.TryCatch(async () =>
                            {
                                //单曲
                                var SongItem = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.KuGouMusic,
                                        MusicType = MusicTypeEnum.SongItem,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();
                                return SongItem;
                            }, ex =>
                            {
                                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                                return null;
                            });

                            if (SongItem == null)
                            {
                                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                                return;
                            }

                            this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                            this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                            this.Total = SongItem.SongItemResult.Total.Value;
                        }
                        else
                        {
                            var SongSheet = await SyncStatic.TryCatch(async () =>
                            {
                                //歌单
                                var SongSheet = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.KuGouMusic,
                                        MusicType = MusicTypeEnum.SongSheet,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();
                                return SongSheet;
                            }, ex => { MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"); return null; });

                            if (SongSheet == null)
                            {
                                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                                return;
                            }

                            this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                            this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                            this.Total = SongSheet.SongSheetResult.Total.Value;
                        }
                        break;
                    }
                case MusicPlatformEnum.KuWoMusic:
                    {
                        if (type == 1)
                        {
                            var SongItem = await SyncStatic.TryCatch(async () =>
                            {
                                //单曲
                                var SongItem = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.KuWoMusic,
                                        MusicType = MusicTypeEnum.SongItem,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();
                                return SongItem;
                            }, ex => { MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"); return null; });

                            if (SongItem == null)
                            {
                                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                                return;
                            }

                            this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                            this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                            this.Total = SongItem.SongItemResult.Total.Value;
                        }
                        else
                        {
                            var SongSheet = await SyncStatic.TryCatch(async () =>
                            {
                                //歌单
                                var SongSheet = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.KuWoMusic,
                                        MusicType = MusicTypeEnum.SongSheet,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();
                                return SongSheet;
                            }, ex => { 
                                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"); 
                                return null; 
                            });

                            if (SongSheet == null)
                            {
                                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                                return;
                            }

                            this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                            this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                            this.Total = SongSheet.SongSheetResult.Total.Value;
                        }
                        break;
                    }
                case MusicPlatformEnum.BiliBiliMusic:
                    {
                        if (type == 1)
                        {
                            var SongItem = await SyncStatic.TryCatch(async () =>
                            {
                                //单曲
                                var SongItem = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.BiliBiliMusic,
                                        MusicType = MusicTypeEnum.SongItem,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();
                                return SongItem;
                            }, ex => { MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"); return null; });

                            if (SongItem == null)
                            {
                                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                                return;
                            }

                            this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                            this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                            this.Total = SongItem.SongItemResult.Total.Value;
                        }
                        else
                        {
                            var SongSheet = await SyncStatic.TryCatch(async () =>
                            {
                                //歌单
                                var SongSheet = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.BiliBiliMusic,
                                        MusicType = MusicTypeEnum.SongSheet,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();
                                return SongSheet;
                            }, ex => { MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"); return null; });

                            if (SongSheet == null)
                            {
                                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                                return;
                            }

                            this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                            this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                            this.Total = SongSheet.SongSheetResult.Total.Value;
                        }
                        break;
                    }
                case MusicPlatformEnum.MiGuMusic:
                    {
                        if (type == 1)
                        {
                            var SongItem = await SyncStatic.TryCatch(async () =>
                            {
                                //单曲
                                var SongItem = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.MiGuMusic,
                                        MusicType = MusicTypeEnum.SongItem,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();
                                return SongItem;
                            }, ex => { MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"); return null; });

                            if (SongItem == null)
                            {
                                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                                return;
                            }

                            this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                            this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                            this.Total = SongItem.SongItemResult.Total.Value;
                        }
                        else
                        {
                            var SongSheet = await SyncStatic.TryCatch(async () =>
                            {
                                //歌单
                                var SongSheet = await MusicFactory.Music(opt =>
                                {
                                    opt.RequestParam = new MusicRequestInput
                                    {
                                        Proxy = this.Proxy,
                                        MusicPlatformType = MusicPlatformEnum.MiGuMusic,
                                        MusicType = MusicTypeEnum.SongSheet,
                                        Search = new MusicSearch
                                        {
                                            Page = PageIndex,
                                            KeyWord = KeyWord
                                        }
                                    };
                                }).RunsAsync();
                                return SongSheet;
                            }, ex => { MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示"); return null; });

                            if (SongSheet == null)
                            {
                                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                                return;
                            }

                            this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                            this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                            this.Total = SongSheet.SongSheetResult.Total.Value;
                        }
                        break;
                    }
                default:
                    throw new NullReferenceException();
            }
        }
        #endregion
    }
}
