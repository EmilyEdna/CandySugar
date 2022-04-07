using CandySugar.Xam.Common;
using CandySugar.Xam.Common.AppDTO;
using Music.SDK;
using Music.SDK.ViewModel;
using Music.SDK.ViewModel.Enums;
using Music.SDK.ViewModel.Request;
using Music.SDK.ViewModel.Response;
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
using CandySugar.Xam.Common.Platform;
using XExten.Advance.StaticFramework;
using System.IO;
using XExten.Advance.HttpFramework.MultiFactory;
using System.Linq;
using MediaManager.Library;

namespace CandySugar.App.Controls.ViewModels.MusicModel
{
    public class CandyMusicViewModel : ViewModelNavigatBase
    {

        private readonly MusicProxy Proxy;
        private int Tap;
        private readonly IYYLiShi Candy;
        public CandyMusicViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.Proxy = new MusicProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
            this.Tap = 0;
            this.PageSheetIndex = 1;
            this.PageSheetIndex = 1;
            this.Platform = MusicPlatformEnum.NeteaseMusic;
            Candy = ContainerLocator.Container.Resolve<IYYLiShi>();
        }

        #region Property
        private ObservableCollection<MusicSongSheetItem> _SongSheets;
        public ObservableCollection<MusicSongSheetItem> SongSheets
        {
            get { return _SongSheets; }
            set { SetProperty(ref _SongSheets, value); }
        }

        private ObservableCollection<MusicSongItem> _SongItems;
        public ObservableCollection<MusicSongItem> SongItems
        {
            get { return _SongItems; }
            set { SetProperty(ref _SongItems, value); }
        }

        private ObservableCollection<GalComboDto> _Combo;
        public ObservableCollection<GalComboDto> Combo
        {
            get => _Combo;
            set => SetProperty(ref _Combo, value);
        }
        private MusicPlatformEnum _Platform;
        public MusicPlatformEnum Platform
        {
            get { return _Platform; }
            set { SetProperty(ref _Platform, value); }
        }
        private string _SearchWord;
        public string SearchWord
        {
            get { return _SearchWord; }
            set { SetProperty(ref _SearchWord, value); }
        }
        private int _PageSingleIndex;
        public int PageSingleIndex
        {
            get { return _PageSingleIndex; }
            set { SetProperty(ref _PageSingleIndex, value); }
        }
        private int _PageSheetIndex;
        public int PageSheetIndex
        {
            get { return _PageSheetIndex; }
            set { SetProperty(ref _PageSheetIndex, value); }
        }
        private int _Total;
        public int Total
        {
            get => _Total;
            set => SetProperty(ref _Total, value);
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

        #region Override
        protected override void OnViewLaunch()
        {
            Combo = new ObservableCollection<GalComboDto>
            {
                new GalComboDto
                {
                    Description = 0,
                    Name = "QQ"
                },
                new GalComboDto
                {
                    Description = 1,
                    Name = "网易"
                },
                new GalComboDto
                {
                    Description = 2,
                    Name = "酷狗"
                },
                new GalComboDto
                {
                    Description = 3,
                    Name = "酷我"
                },
                new GalComboDto
                {
                    Description = 4,
                    Name = "B站"
                },
                new GalComboDto{
                   Description = 5,
                   Name="咪咕"
                }
            };
        }
        #endregion

        #region Command
        public ICommand ComboSelectCommand => new DelegateCommand<dynamic>(input =>
        {
            var box = (GalComboDto)input;
            if (box != null)
            {
                this.Platform = (MusicPlatformEnum)box.Description;
            }
        });

        public ICommand TabChangedCommand => new DelegateCommand<dynamic>((input) =>
        {
            Tap = input;
            if (Tap == 0 && SongItems == null)
            {
                this.PageSingleIndex = 1;
                if (!SearchWord.IsNullOrEmpty())
                    SearchSingleSong();
            }
            if (Tap == 1 && SongSheets == null)
            {
                this.PageSheetIndex = 1;
                if (!SearchWord.IsNullOrEmpty())
                    SearchSheetSong();
            }
        });

        public ICommand SearchCommand => new DelegateCommand<string>((input) =>
        {
            this.SearchWord = input;
            this.PageSingleIndex = 1;
            this.PageSheetIndex = 1;
            if (Tap == 0)
                SearchSingleSong();
            if (Tap == 1)
                SearchSheetSong();
        });

        public ICommand RefreshsCommand => new DelegateCommand(() =>
        {
            this.PageSingleIndex = 1;
            this.PageSheetIndex = 1;
            if (Tap == 0 && !SearchWord.IsNullOrEmpty()) SearchSingleSong();
            if (Tap == 1 && !SearchWord.IsNullOrEmpty()) SearchSheetSong();
        });

        public ICommand ShowMoreCommand => new DelegateCommand(() =>
        {
            if (Tap == 0 && !SearchWord.IsNullOrEmpty())
            {
                this.PageSingleIndex += 1;
                if (PageSingleIndex <= Total)
                    SearchSingleSong(true);
            }
            if (Tap == 1 && !SearchWord.IsNullOrEmpty())
            {
                this.PageSheetIndex += 1;
                if (PageSheetIndex <= Total)
                    SearchSheetSong(true);
            }
        });

        public ICommand AddPlayListCommand => new DelegateCommand<MusicSongItem>(input =>
        {
            if (input != null)
                LoadMusic(input, this.Platform);
        });
        #endregion

        #region Method
        private string Tip(string Method)
        {
            return String.Format(Soft.Toast, nameof(CandyMusicViewModel), Method);
        }
        private  void Cache(MusicSongItem input, MusicSongPlayAddressResult Song)
        {
            var SongArtist = string.Join(",", input.SongArtistName);
            var SongFile = $"{input.SongName}({input.SongAlbumName})-{SongArtist}_{Song.MusicPlatformType}.mp3";
            var route = ContainerLocator.Container.Resolve<IAndroidPlatform>().DownPath();
            AuthorizeHelper.Instance.ApplyPermission(() =>
            {
                var dir = SyncStatic.CreateDir(Path.Combine(route, "CandyDown", "Music", $"{SongArtist}"));
                var fn = SyncStatic.CreateFile(Path.Combine(dir, SongFile));

                if (this.Platform == MusicPlatformEnum.BiliBiliMusic)
                {
                    var CacheAddress = SyncStatic.WriteFile(Song.BilibiliFileBytes, fn);
                    AddMusic(input, Song, CacheAddress);
                }
                else
                {
                    var filebytes = IHttpMultiClient.HttpMulti.AddNode(opt => opt.NodePath = Song.SongURL).Build().RunBytesFirst();
                    var CacheAddress = SyncStatic.WriteFile(filebytes, fn);
                    AddMusic(input, Song, CacheAddress);
                }
            });
        }
        private async void AddMusic(MusicSongItem input, MusicSongPlayAddressResult Song, string path)
        {
            await Candy.AddPlayList(new CandyYYLiShiDto
            {
                Address = Song.SongURL,
                SongAlbum = input.SongAlbumName,
                SongName = input.SongName,
                SongArtist = string.Join(",", input.SongArtistName),
                SongId = input.SongId,
                CacheAddress = path,
                Platform = (int)this.Platform,
                IsPlayed = false
            });
        }
        /// <summary>
        /// 查单曲
        /// </summary>
        /// <param name="IsLoadMore"></param>
        public async void SearchSingleSong(bool IsLoadMore = false)
        {
            try
            {
                MusicResponseOutput SongItem = null;

                if (IsLoadMore) IsBusy = true; else Refresh = true;
                if (Platform == MusicPlatformEnum.QQMusic)
                {
                    //单曲
                    SongItem = await MusicFactory.Music(opt =>
                   {
                       opt.RequestParam = new MusicRequestInput
                       {
                           Proxy = this.Proxy,
                           MusicPlatformType = MusicPlatformEnum.QQMusic,
                           MusicType = MusicTypeEnum.SongItem,
                           Search = new MusicSearch
                           {
                               Page = PageSingleIndex,
                               KeyWord = SearchWord
                           }
                       };
                   }).RunsAsync();
                }
                else if (Platform == MusicPlatformEnum.NeteaseMusic)
                {
                    //单曲
                    SongItem = await MusicFactory.Music(opt =>
                    {
                        opt.RequestParam = new MusicRequestInput
                        {
                            Proxy = this.Proxy,
                            MusicPlatformType = MusicPlatformEnum.NeteaseMusic,
                            MusicType = MusicTypeEnum.SongItem,
                            Search = new MusicSearch
                            {
                                Page = PageSingleIndex,
                                KeyWord = SearchWord
                            }
                        };
                    }).RunsAsync();
                }
                else if (Platform == MusicPlatformEnum.KuGouMusic)
                {
                    //单曲
                    SongItem = await MusicFactory.Music(opt =>
                    {
                        opt.RequestParam = new MusicRequestInput
                        {
                            Proxy = this.Proxy,
                            MusicPlatformType = MusicPlatformEnum.KuGouMusic,
                            MusicType = MusicTypeEnum.SongItem,
                            Search = new MusicSearch
                            {
                                Page = PageSingleIndex,
                                KeyWord = SearchWord
                            }
                        };
                    }).RunsAsync();
                }
                else if (Platform == MusicPlatformEnum.KuWoMusic)
                {
                    //单曲
                    SongItem = await MusicFactory.Music(opt =>
                    {
                        opt.RequestParam = new MusicRequestInput
                        {
                            Proxy = this.Proxy,
                            MusicPlatformType = MusicPlatformEnum.KuWoMusic,
                            MusicType = MusicTypeEnum.SongItem,
                            Search = new MusicSearch
                            {
                                Page = PageSingleIndex,
                                KeyWord = SearchWord
                            }
                        };
                    }).RunsAsync();
                }
                else if (Platform == MusicPlatformEnum.BiliBiliMusic)
                {
                    //单曲
                    SongItem = await MusicFactory.Music(opt =>
                    {
                        opt.RequestParam = new MusicRequestInput
                        {
                            Proxy = this.Proxy,
                            MusicPlatformType = MusicPlatformEnum.BiliBiliMusic,
                            MusicType = MusicTypeEnum.SongItem,
                            Search = new MusicSearch
                            {
                                Page = PageSingleIndex,
                                KeyWord = SearchWord
                            }
                        };
                    }).RunsAsync();
                }
                else
                {
                    //单曲
                    SongItem = await MusicFactory.Music(opt =>
                    {
                        opt.RequestParam = new MusicRequestInput
                        {
                            Proxy = this.Proxy,
                            MusicPlatformType = MusicPlatformEnum.MiGuMusic,
                            MusicType = MusicTypeEnum.SongItem,
                            Search = new MusicSearch
                            {
                                Page = PageSingleIndex,
                                KeyWord = SearchWord
                            }
                        };
                    }).RunsAsync();
                }

                this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                this.Total = SongItem.SongItemResult.Total.Value;

                if (IsLoadMore)
                {
                    IsBusy = false;
                    if (this.SongItems == null)
                        this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                    else
                        SongItem.SongItemResult.SongItems.ForEach(item =>
                        {
                            this.SongItems.Add(item);
                        });
                }
                else
                {
                    Refresh = false;
                    this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                }
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(Tip("SearchSingleSong")))
                {
                    await Task.Delay(3000);
                }
            }
        }
        /// <summary>
        /// 查歌单
        /// </summary>
        /// <param name="IsLoadMore"></param>
        public async void SearchSheetSong(bool IsLoadMore = false)
        {
            try
            {
                MusicResponseOutput SongSheet = null;

                if (IsLoadMore) IsBusy = true; else Refresh = true;
                if (Platform == MusicPlatformEnum.QQMusic)
                {
                    //单曲
                    SongSheet = await MusicFactory.Music(opt =>
                    {
                        opt.RequestParam = new MusicRequestInput
                        {
                            Proxy = this.Proxy,
                            MusicPlatformType = MusicPlatformEnum.QQMusic,
                            MusicType = MusicTypeEnum.SongSheet,
                            Search = new MusicSearch
                            {
                                Page = PageSheetIndex,
                                KeyWord = SearchWord
                            }
                        };
                    }).RunsAsync();
                }
                else if (Platform == MusicPlatformEnum.NeteaseMusic)
                {
                    //单曲
                    SongSheet = await MusicFactory.Music(opt =>
                    {
                        opt.RequestParam = new MusicRequestInput
                        {
                            Proxy = this.Proxy,
                            MusicPlatformType = MusicPlatformEnum.NeteaseMusic,
                            MusicType = MusicTypeEnum.SongSheet,
                            Search = new MusicSearch
                            {
                                Page = PageSheetIndex,
                                KeyWord = SearchWord
                            }
                        };
                    }).RunsAsync();
                }
                else if (Platform == MusicPlatformEnum.KuGouMusic)
                {
                    //单曲
                    SongSheet = await MusicFactory.Music(opt =>
                    {
                        opt.RequestParam = new MusicRequestInput
                        {
                            Proxy = this.Proxy,
                            MusicPlatformType = MusicPlatformEnum.KuGouMusic,
                            MusicType = MusicTypeEnum.SongSheet,
                            Search = new MusicSearch
                            {
                                Page = PageSheetIndex,
                                KeyWord = SearchWord
                            }
                        };
                    }).RunsAsync();
                }
                else if (Platform == MusicPlatformEnum.KuWoMusic)
                {
                    //单曲
                    SongSheet = await MusicFactory.Music(opt =>
                    {
                        opt.RequestParam = new MusicRequestInput
                        {
                            Proxy = this.Proxy,
                            MusicPlatformType = MusicPlatformEnum.KuWoMusic,
                            MusicType = MusicTypeEnum.SongSheet,
                            Search = new MusicSearch
                            {
                                Page = PageSheetIndex,
                                KeyWord = SearchWord
                            }
                        };
                    }).RunsAsync();
                }
                else if (Platform == MusicPlatformEnum.BiliBiliMusic)
                {
                    //单曲
                    SongSheet = await MusicFactory.Music(opt =>
                    {
                        opt.RequestParam = new MusicRequestInput
                        {
                            Proxy = this.Proxy,
                            MusicPlatformType = MusicPlatformEnum.BiliBiliMusic,
                            MusicType = MusicTypeEnum.SongSheet,
                            Search = new MusicSearch
                            {
                                Page = PageSheetIndex,
                                KeyWord = SearchWord
                            }
                        };
                    }).RunsAsync();
                }
                else
                {
                    //单曲
                    SongSheet = await MusicFactory.Music(opt =>
                    {
                        opt.RequestParam = new MusicRequestInput
                        {
                            Proxy = this.Proxy,
                            MusicPlatformType = MusicPlatformEnum.MiGuMusic,
                            MusicType = MusicTypeEnum.SongSheet,
                            Search = new MusicSearch
                            {
                                Page = PageSheetIndex,
                                KeyWord = SearchWord
                            }
                        };
                    }).RunsAsync();
                }

                this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                this.Total = SongSheet.SongSheetResult.Total.Value;

                if (IsLoadMore)
                {
                    IsBusy = false;
                    if (this.SongSheets == null)
                        this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                    else
                        SongSheet.SongSheetResult.SongSheetItems.ForEach(item =>
                        {
                            this.SongSheets.Add(item);
                        });
                }
                else
                {
                    Refresh = false;
                    this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                }
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(Tip("SearchSingleSong")))
                {
                    await Task.Delay(3000);
                }
            }

        }
        /// <summary>
        /// 加载歌曲
        /// </summary>
        /// <param name="input"></param>
        /// <param name="PlatformType"></param>
        public async void LoadMusic(MusicSongItem input, MusicPlatformEnum PlatformType)
        {
            try
            {
                var SongURL = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new MusicRequestInput
                    {
                        Proxy = this.Proxy,
                        MusicPlatformType = PlatformType,
                        MusicType = MusicTypeEnum.PlayAddress,
                        AddressSearch = new MusicPlaySearch
                        {
                            Dynamic = input.SongId,
                            KuGouAlbumId = input.SongAlbumId,
                        }
                    };
                }).RunsAsync();

                if (SongURL.SongPlayAddressResult.CanPlay == false)
                {
                    using (await MaterialDialog.Instance.LoadingSnackbarAsync("当前歌曲已下架，请切换到其他其他平台搜索"))
                    {
                        await Task.Delay(3000);
                    }
                    return;
                }

                Cache(input, SongURL.SongPlayAddressResult);
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(Tip("SearchSingleSong")))
                {
                    await Task.Delay(3000);
                }
            }
        }
        #endregion
    }
}
