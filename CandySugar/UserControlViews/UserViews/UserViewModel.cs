using Anime.SDK.ViewModel.Response;
using CandySugar.CandyWindows.CandyWinViewModel;
using CandySugar.Common.DTO;
using CandySugar.Core.Service;
using Manga.SDK.ViewModel.Response;
using Novel.SDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.UserControlViews.UserViews
{
    public class UserViewModel : Screen
    {
        private readonly IContainer Container;
        public UserViewModel(IContainer Container)
        {
            this.Container = Container;
        }

        private ObservableCollection<CandyNovelHistoryDto> _NovelHistories;
        public ObservableCollection<CandyNovelHistoryDto> NovelHistories
        {
            get { return _NovelHistories; }
            set { SetAndNotify(ref _NovelHistories, value); }
        }

        private ObservableCollection<CandyMangaHistoryDto> _MangaHistories;
        public ObservableCollection<CandyMangaHistoryDto> MangaHistories
        {
            get { return _MangaHistories; }
            set { SetAndNotify(ref _MangaHistories, value); }
        }

        private ObservableCollection<CandyAnimeHistoryDto> _AnimeHistories;
        public ObservableCollection<CandyAnimeHistoryDto> AnimeHistories
        {
            get { return _AnimeHistories; }
            set { SetAndNotify(ref _AnimeHistories, value); }
        }

        protected override async void OnViewLoaded()
        {
            NovelHistories = new ObservableCollection<CandyNovelHistoryDto>(await Container.Get<ILiShi>().GetNovelHistory());
            MangaHistories = new ObservableCollection<CandyMangaHistoryDto>(await Container.Get<ILiShi>().GetMangaHistory());
            AnimeHistories = new ObservableCollection<CandyAnimeHistoryDto>(await Container.Get<ILiShi>().GetAnimeHistory());
        }

        public void Play(CandyAnimeHistoryDto input)
        {
            if (input.PlayMode)
            {
                var vm = Container.Get<CandyDPlayViewModel>();
                vm.WatchResult = input.ToMapest<AnimePlayResult>();
                vm.Loading = System.Windows.Visibility.Hidden;
                //Open
                BootResource.AnimeWEB(window =>
                {
                    window.DataContext = vm;
                });
            }
            else
            {
                var vm = Container.Get<CandyVLCViewModel>();
                vm.WatchResult = input.ToMapest<AnimePlayResult>();
                vm.Loading = System.Windows.Visibility.Hidden;
                //Open
                BootResource.AnimeVLC(window =>
                {
                    window.DataContext = vm;
                });
            }
        }

        public void Watch(CandyMangaHistoryDto input)
        {
            var vm = Container.Get<CandyMangaReaderViewModel>();
            vm.Loading = System.Windows.Visibility.Visible;
            vm.Chapters = input.Chapters.ToModel<ObservableCollection<MangaChapterResult>>();
            vm.Total = vm.Chapters.Count;
            vm.Index = input.Index;
            vm.InitCurrent();

            //Open
            BootResource.Manga(window =>
            {
                window.DataContext = vm;
            });
        }

        public void Reader(CandyNovelHistoryDto input)
        {
            var vm = Container.Get<CandyNovelViewModel>();
            vm.NovelContent = input.ToMapest<NovelContentResult>();
            vm.BookName = input.BookName;
            vm.Loading = System.Windows.Visibility.Hidden;
            //Open
            BootResource.Novel(window =>
            {
                window.DataContext = vm;
            });
        }
    }
}
