using CandySugar.App.Controls.ViewModels.MusicModel;
using CandySugar.Xam.Common;
using Music.SDK;
using Music.SDK.ViewModel;
using Music.SDK.ViewModel.Enums;
using Music.SDK.ViewModel.Request;
using Music.SDK.ViewModel.Response;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XF.Material.Forms.UI.Dialogs;
using Prism.Ioc;

namespace CandySugar.App.Controls.LayoutView.LayoutViewModel
{
    public class PopAlbumContentViewModel : ViewModelBase
    {
        private readonly MusicProxy Proxy;
        public PopAlbumContentViewModel()
        {
            this.Proxy = new MusicProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
        }

        #region Property
        private ObservableCollection<MusicSongItem> _AlbumDetail;
        public ObservableCollection<MusicSongItem> AlbumDetail
        {
            get { return _AlbumDetail; }
            set { SetProperty(ref _AlbumDetail, value); }
        }
        #endregion

        #region Method
        private string Tip(string Method)
        {
            return String.Format(Soft.Toast, nameof(PopAlbumContentViewModel), Method);
        }
        public async void SearchAlbum(MusicSongItem input, MusicPlatformEnum platform)
        {
            try
            {
                var SongAlbum = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new MusicRequestInput
                    {
                        Proxy = this.Proxy,
                        MusicPlatformType = platform,
                        MusicType = MusicTypeEnum.AlbumDetail,
                        AlbumSearch = new MusicAlbumSearch
                        {
                            AlbumId = input.SongAlbumId
                        }
                    };
                }).RunsAsync();

                this.AlbumDetail = new ObservableCollection<MusicSongItem>(SongAlbum.SongAlbumDetailResult.SongItems);
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(Tip("SearchAlbum")))
                {
                    await Task.Delay(3000);
                }
            }
        }
        #endregion

        #region Commod
        public ICommand AddPlayCommand => new DelegateCommand<MusicSongItem>(input =>
        {
            ContainerLocator.Container.Resolve<CandyMusicViewModel>().LoadMusic(input);
        });
        #endregion
    }
}
