using CandySugar.App.Controls.ViewModels.LightNovelModel;
using CandySugar.App.Controls.Views.Anime;
using CandySugar.App.Controls.Views.LightNovel;
using CandySugar.App.Controls.Views.Manga;
using CandySugar.App.Controls.Views.Novel;
using CandySugar.Xam.Common;
using CandySugar.Xam.Common.DTO;
using CandySugar.Xam.Core.Service;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using XExten.Advance.InternalFramework.Securities.Common;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;
using XF.Material.Forms.UI.Dialogs;

namespace CandySugar.App.Controls.ViewModels
{
    public class CandyContentIndexViewModel : ViewModelBase
    {
        public CandyContentIndexViewModel() : base()
        {
            XSCandy = Resolve<IXSLiShi>();
            DMCandy = Resolve<IDMLiShi>();
            LXSCandy = Resolve<ILXSLiShi>();
            MHCandy = Resolve<IMHLiShi>();
            Refresh = false;
        }

        #region Field
        private readonly IXSLiShi XSCandy;
        private readonly ILXSLiShi LXSCandy;
        private readonly IDMLiShi DMCandy;
        private readonly IMHLiShi MHCandy;
        #endregion

        #region Property
        private ObservableCollection<CandyXSLiShiDto> _XSLiShi;
        public ObservableCollection<CandyXSLiShiDto> XSLiShi
        {
            get { return _XSLiShi; }
            set { SetProperty(ref _XSLiShi, value); }
        }
        private ObservableCollection<CandyDMLiShiDto> _DMLiShi;
        public ObservableCollection<CandyDMLiShiDto> DMLiShi
        {
            get { return _DMLiShi; }
            set { SetProperty(ref _DMLiShi, value); }
        }
        private ObservableCollection<CandyLXSLiShiDto> _LXSLiShi;
        public ObservableCollection<CandyLXSLiShiDto> LXSLiShi
        {
            get { return _LXSLiShi; }
            set { SetProperty(ref _LXSLiShi, value); }
        }
        private ObservableCollection<CandyMHLiShiDto> _MHSLiShi;
        public ObservableCollection<CandyMHLiShiDto> MHSLiShi
        {
            get { return _MHSLiShi; }
            set { SetProperty(ref _MHSLiShi, value); }
        }
        public bool _Refresh;
        public bool Refresh
        {
            get => _Refresh;
            set => SetProperty(ref _Refresh, value);
        }
        #endregion

        #region Command
        public ICommand MHClickCommand => new DelegateCommand<CandyMHLiShiDto>(async input =>
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Route", input.Address);
            await Resolve<INavigationService>().NavigateAsync(new Uri(nameof(CandyMangaReaderView), UriKind.Relative), param);
        });

        public ICommand MHDeleteCommand => new DelegateCommand<CandyMHLiShiDto>(async input => {

            if (await MHCandy.Remove(input))
            {
                OnViewLaunch();
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("已从书架中移除"))
                {
                    await Task.Delay(3000);
                }
            }

        });

        public ICommand XSClickCommand => new DelegateCommand<CandyXSLiShiDto>(async input =>
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("ChapterURL", input.ChapeterAddress);
            param.Add("BookName", input.BookName);
            await Resolve<INavigationService>().NavigateAsync(new Uri(nameof(CandyNovelContentView), UriKind.Relative), param);
        });

        public ICommand XSDeleteCommand => new DelegateCommand<CandyXSLiShiDto>(async input =>
        {
            if (await XSCandy.Remove(input))
            {
                OnViewLaunch();
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("已从列表中移除"))
                {
                    await Task.Delay(3000);
                }
            }
        });

        public ICommand DMClickCommand => new DelegateCommand<CandyDMLiShiDto>(async input =>
        {

            NavigationParameters param = new NavigationParameters();
            param.Add("WatchAddress", input.PlayURL);
            await Resolve<INavigationService>().NavigateAsync(new Uri(nameof(CandyAnimePlayView), UriKind.Relative), param);
        });

        public ICommand DMDeleteCommand => new DelegateCommand<CandyDMLiShiDto>(async input =>
        {
            if (await DMCandy.Remove(input))
            {
                OnViewLaunch();
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("已从列表中移除"))
                {
                    await Task.Delay(3000);
                }
            }
        });

        public ICommand LXSClickCommand => new DelegateCommand<CandyLXSLiShiDto>(async input =>
        {
            if (input.IsBook)
            {
                NavigationParameters param = new NavigationParameters();
                param.Add("Content", SyncStatic.Compress(input.Content, SecurityType.Base64));
                param.Add("ChapterName", input.ChapterName);
                await Resolve<INavigationService>().NavigateAsync(new Uri(nameof(CandyLightNovelContentView), UriKind.Relative), param);
            }
            else
            {
                NavigationParameters param = new NavigationParameters();
                param.Add("Image", input.Image);
                param.Add("ChapterName", input.ChapterName);
                await Resolve<INavigationService>().NavigateAsync(new Uri(nameof(CandyLightNovelImageViewModel), UriKind.Relative), param);
            }

        });

        public ICommand LXSDeleteCommand => new DelegateCommand<CandyLXSLiShiDto>(async input =>
        {
            if (await LXSCandy.Remove(input))
            {
                OnViewLaunch();
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("已从列表中移除"))
                {
                    await Task.Delay(3000);
                }
            }
        });

        public ICommand RefreshNovelCommand => new DelegateCommand(async () =>
        {
            Refresh = true;
            await Task.Delay(Soft.WaitSpan);
            InitNovel();
            Refresh = false;
        });

        public ICommand RefreshLightNovelCommand => new DelegateCommand(async () =>
        {
            Refresh = true;
            await Task.Delay(Soft.WaitSpan);
            InitLightNovel();
            Refresh = false;
        });

        public ICommand RefreshAnimeCommand => new DelegateCommand(async () =>
        {
            Refresh = true;
            await Task.Delay(Soft.WaitSpan);
            InitAnime();
            Refresh = false;
        });

        public ICommand RefreshMangaCommand => new DelegateCommand(async () =>
        {
            Refresh = true;
            await Task.Delay(Soft.WaitSpan);
            InitManga();
            Refresh = false;
        });

        public ICommand TabChangedCommand => new DelegateCommand<dynamic>(input =>
        {
            if (input == 0)
                InitNovel();
            else if (input == 1)
                InitLightNovel();
            else if (input == 2)
                InitAnime();
        });
        #endregion

        #region Override
        protected override void OnViewLaunch()
        {
            InitNovel();
            InitLightNovel();
            InitAnime();
            InitManga();
        }
        #endregion

        #region Method
        public async void InitManga() 
        {
            MHSLiShi = new ObservableCollection<CandyMHLiShiDto>(await Resolve<IMHLiShi>().Query());
        }
        public async void InitNovel()
        {
            XSLiShi = new ObservableCollection<CandyXSLiShiDto>(await Resolve<IXSLiShi>().Query());
        }
        public async void InitAnime()
        {
            DMLiShi = new ObservableCollection<CandyDMLiShiDto>(await Resolve<IDMLiShi>().Query());
        }
        public async void InitLightNovel()
        {
            LXSLiShi = new ObservableCollection<CandyLXSLiShiDto>(await Resolve<ILXSLiShi>().Query());
        }
        #endregion 
    }
}
