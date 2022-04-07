using CandySugar.App.Controls.LayoutView;
using CandySugar.App.Controls.LayoutView.LayoutViewModel;
using CandySugar.App.Controls.ViewModels.MusicModel;
using CandySugar.Xam.Common;
using CandySugar.Xam.Common.Enum;
using CandySugar.Xam.Core.Service;
using MediaManager;
using MediaManager.Library;
using MediaManager.Playback;
using MediaManager.Player;
using Music.SDK.ViewModel.Response;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XExten.Advance.LinqFramework;
using XF.Material.Forms.UI.Dialogs;
using Prism.Ioc;

namespace CandySugar.App.Controls.Views.Music
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandyMusicView : ContentPage
    {
        public static CandyMusicView MusicViewProperty { get; set; }
        public CandyMusicView()
        {
            InitializeComponent();
            Flag = 0;
            Play = 0;
            Menu.Source = ImageSource.FromFile("repeat.png");
            PlayHandler.Source = ImageSource.FromFile("play.png");
            Candy = ContainerLocator.Container.Resolve<IYYLiShi>();
            CrossMediaManager.Current.RepeatMode = RepeatMode.All;
            MusicViewProperty = this;
        }
        private int Flag;
        private int Play;
        private IYYLiShi Candy;
        #region Event
        private void ChangedClick(object sender, EventArgs e)
        {
            if (Flag == 0)
            {
                Menu.Source = ImageSource.FromFile("repeat2.png");
                Flag = 1;
                CrossMediaManager.Current.RepeatMode = RepeatMode.All;
            }
            else
            {
                Menu.Source = ImageSource.FromFile("repeat.png");
                Flag = 0;
                CrossMediaManager.Current.RepeatMode = RepeatMode.One;
            }
        }
        private void PopupOpened(object sender, EventArgs e)
        {
            PopPlayList();
        }
        private void PopupSheetOpened(object sender, EventArgs e)
        {
            PopSheet((MusicSongSheetItem)(sender as Button).CommandParameter);
        }
        private void PopupAlbumOpened(object sender, EventArgs e)
        {
            PopAlbum((MusicSongItem)(sender as Button).CommandParameter);
        }
        private void HandleClick(object sender, EventArgs e)
        {
            PlayMusic();
        }
        private void MusicView_Disappearing(object sender, EventArgs e)
        {
            if (CrossMediaManager.Current.State == MediaPlayerState.Paused || CrossMediaManager.Current.State == MediaPlayerState.Playing)
            {
                CrossMediaManager.Current.Stop();
                CrossMediaManager.Current.Dispose();
            }
        }
        #endregion
        #region Method
        private void PopPlayList()
        {
            var HeaderView = new PopPlayHeaderView();
            var ContentView = new PopPlayContentView();
            (ContentView.BindingContext as PopPlayContentViewModel).PopPlayHeaderView = HeaderView;
            PopCommon(HeaderView, ContentView);
            Pop.Show(20, 300);
        }
        private void PopSheet(MusicSongSheetItem input)
        {
            var HeaderView = new PopSheetHeaderView();
            ((PopSheetHeaderViewModel)HeaderView.BindingContext).Title = input.SongSheetName;
            var ContentView = new PopSheetContentView();
            ((PopSheetContentViewModel)ContentView.BindingContext).SearchSheet(input, ((CandyMusicViewModel)BindingContext).Platform);
            PopCommon(HeaderView, ContentView);
            Pop.Show(20, 300);
        }
        private void PopAlbum(MusicSongItem input)
        {
            var HeaderView = new PopAlbumHeaderView();
            ((PopAlbumHeaderViewModel)HeaderView.BindingContext).Title = input.SongAlbumName;
            var ContentView = new PopAlbumContentView();
            ((PopAlbumContentViewModel)ContentView.BindingContext).SearchAlbum(input, ((CandyMusicViewModel)BindingContext).Platform);
            PopCommon(HeaderView, ContentView);
            Pop.Show(20, 300);
        }
        private void PopCommon(ContentView HeaderView, ContentView ContentView)
        {
            Pop.PopupView.PopupStyle = new PopupStyle
            {
                CornerRadius = 45,
            };
            Pop.PopupView.HeaderTemplate = new DataTemplate(() => HeaderView);
            Pop.PopupView.ShowCloseButton = false;
            Pop.PopupView.ShowFooter = false;
            Pop.PopupView.MinimumWidthRequest = Soft.ScreenWidth - 40;
            Pop.PopupView.MinimumHeightRequest = Soft.ScreenHeight / 2;
            Pop.PopupView.AnimationEasing = AnimationEasing.SinIn;
            Pop.PopupView.AnimationMode = AnimationMode.SlideOnBottom;
            Pop.PopupView.ContentTemplate = new DataTemplate(() => ContentView);
        }
        public async Task<List<MediaItem>> Query()
        {
            var data = await Candy.GetPlayList();
            if (data.Count(t => t.IsPlayed) <= 0)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("还未选择歌曲！"))
                {
                    await Task.Delay(3000);
                }
                return null;
            }
            List<MediaItem> medias = new List<MediaItem>();

            data.ForEach(item =>
            {
                MediaItem media = new MediaItem
                {
                    Album = item.SongAlbum,
                    Artist = item.SongArtist,
                    Date = new DateTime(item.Span),
                    FileExtension = ".mp3",
                    Title = item.SongName,
                    FileName = item.SongName,
                    MediaType = MediaType.Audio,
                    MediaLocation = MediaLocation.FileSystem,
                    IsMetadataExtracted = true,
                    DisplayTitle = item.SongName,
                    MediaUri = "file:///" + item.CacheAddress,
                    Id = item.PId.ToString()
                };
                medias.Add(media);
            });
            return medias;
        }
        public async void PlayMusic()
        {
            if (Play == 0)
            {
                PlayHandler.Source = ImageSource.FromFile("pause.png");
                Play = 1;

                var data = await Query();

                if (data != null && data.Count > 0)
                {
                    SliderPosition.IsEnabled = true;
                    if (CrossMediaManager.Current.State == MediaPlayerState.Paused)
                        await CrossMediaManager.Current.PlayPause();
                    else
                    {
                        await CrossMediaManager.Current.Play(data);
                        CrossMediaManager.Current.PositionChanged += Current_PositionChanged;
                        CrossMediaManager.Current.MediaItemFinished += Current_MediaItemFinished;
                        CrossMediaManager.Current.MediaItemChanged += Current_MediaItemChanged;
                    }
                }
            }
            else
            {
                PlayHandler.Source = ImageSource.FromFile("play.png");
                Play = 0;
                if (CrossMediaManager.Current.IsPlaying())
                    await CrossMediaManager.Current.Pause();
            }
        }

        #endregion
        #region PlayEvent
        private void Current_MediaItemFinished(object sender, MediaManager.Media.MediaItemEventArgs e)
        {
            Candy.UpdatePlayState(Guid.Parse(e.MediaItem.Id), false);
        }
        private void Current_MediaItemChanged(object sender, MediaManager.Media.MediaItemEventArgs e)
        {
            SongName.Text = e.MediaItem.Title;
            Candy.UpdatePlayState(Guid.Parse(e.MediaItem.Id), true);
        }
        private void Current_PositionChanged(object sender, MediaManager.Playback.PositionChangedEventArgs e)
        {
            if (SliderPosition.Maximum <= 0.01)
                SliderPosition.Maximum = ((IMediaManager)sender).Duration.TotalSeconds;
            if (EndText.Text.IsNullOrEmpty())
                EndText.Dispatcher.BeginInvokeOnMainThread(() =>
                {
                    EndText.Text ="/"+((IMediaManager)sender).Duration.ToString().Split(".").FirstOrDefault();
                });
            SliderPosition.Dispatcher.BeginInvokeOnMainThread(() =>
            {
                SliderPosition.Value = e.Position.TotalSeconds;
            });
            StartText.Dispatcher.BeginInvokeOnMainThread(() =>
            {
                StartText.Text= e.Position.ToString().Split(".").FirstOrDefault();
            });
        }
        #endregion
    }
}