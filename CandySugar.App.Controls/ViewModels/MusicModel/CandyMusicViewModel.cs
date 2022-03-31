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

namespace CandySugar.App.Controls.ViewModels.MusicModel
{
    public class CandyMusicViewModel : ViewModelNavigatBase
    {

        private readonly MusicProxy Proxy;
        private int Tap;
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
            this.PageIndex = 1;
            this.Platform = MusicPlatformEnum.NeteaseMusic;
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
        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { SetProperty(ref _PageIndex, value); }
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

        public ICommand TabChangedCommand => new DelegateCommand<dynamic>((input) => Tap = input);

        public ICommand SearchCommand => new DelegateCommand<string>((input) =>
        {
            this.SearchWord = input;
            this.PageIndex = 1;
            if (Tap == 0)
                SearchSingleSong();
            if (Tap == 1)
                SearchSheetSong();
        });

        public ICommand RefreshsCommand => new DelegateCommand(() =>
        {
            PageIndex = 1;
            if (Tap == 0 && !SearchWord.IsNullOrEmpty()) SearchSingleSong();
            if (Tap == 1 && !SearchWord.IsNullOrEmpty()) SearchSheetSong();
        });

        public ICommand ShowMoreCommand => new DelegateCommand(() =>
        {
            PageIndex += 1;
            if (PageIndex <= Total)
            {
                if (Tap == 0 && !SearchWord.IsNullOrEmpty()) SearchSingleSong(true);
                if (Tap == 1 && !SearchWord.IsNullOrEmpty()) SearchSheetSong(true);
            }
        });
        #endregion

        #region Method
        private string Tip(string Method)
        {
            return String.Format(Soft.Toast, nameof(CandyMusicViewModel), Method);
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
                               Page = PageIndex,
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
                                Page = PageIndex,
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
                                Page = PageIndex,
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
                                Page = PageIndex,
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
                                Page = PageIndex,
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
                                Page = PageIndex,
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
                                Page = PageIndex,
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
                                Page = PageIndex,
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
                                Page = PageIndex,
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
                                Page = PageIndex,
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
                                Page = PageIndex,
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
                                Page = PageIndex,
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
        #endregion
    }
}
