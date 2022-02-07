using CandySugar.CandyWindows.CnadyWinViewModel;
using CandySugar.Common.Enum;
using CandySugar.Controls.ControlViewModel;
using CandySugar.Controls.UserControls;
using CandySugar.Core.CandyUtily;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CandySugar.CandyWindows
{
    /// <summary>
    /// CandyVLCWin.xaml 的交互逻辑
    /// </summary>
    public partial class CandyVLCWin : CandyWindow
    {
        #region Field
        public DispatcherTimer Timer;
        public LibVLC LibVlcs;
        public LibVLCSharp.Shared.MediaPlayer MediaPlayers;

        private Color color;
        private Media Medias;
        private CandyVLCViewModel ViewModel;
        #endregion

        public CandyVLCWin()
        {
            InitializeComponent();
            Header.DataContext = CandyContainer.Instance.Resolves<NormalHeaderViewModel>().Basic();

            LibVLCSharp.Shared.Core.Initialize(Environment.CurrentDirectory + @"\Plugins\VLC_X64\");
            this.LibVlcs = new LibVLC();
            this.MediaPlayers = new LibVLCSharp.Shared.MediaPlayer(this.LibVlcs);
            this.Videos.MediaPlayer = this.MediaPlayers;
            this.window.MaxWidth = SystemParameters.PrimaryScreenWidth;
            this.window.MaxHeight = SystemParameters.PrimaryScreenHeight;
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10)
            };
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }


        #region Private

        private void Play()
        {
            if (!this.Videos.MediaPlayer.IsPlaying)
            {
                using (Medias = new Media(LibVlcs, new Uri(ViewModel.WatchResult.PlayURL)))
                    this.Videos.MediaPlayer.Play(Medias);

                this.Videos.MediaPlayer.Playing += MediaPlayer_Playing;
                this.Videos.MediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
            }
            else
            {
                this.Videos.MediaPlayer.Play();
            }
        }

        private void Pause()
        {
            this.Videos.MediaPlayer.Pause();
        }

        private void Stop()
        {
            this.RatePlay.Text = "00:00:00";
            this.RateTotal.Text = "00:00:00";
            this.Videos.MediaPlayer.Stop();
        }
        #endregion

        #region Event
        private void VoiceChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.Videos.MediaPlayer == null) return;
            this.Videos.MediaPlayer.Volume = Convert.ToInt32(e.NewValue) * 10;
        }

        private void RateDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if (this.Videos.MediaPlayer == null) return;
            var position = (float)(Rate.Value / Rate.Maximum);
            if (position == 1)
            {
                position = 0.99f;
            }
            this.Videos.MediaPlayer.Position = position;
        }

        private void BtnClick(object sender, RoutedEventArgs e)
        {
            var btn = (sender as Button);
            var VlcFunc = (VLCFuncEnum)btn.CommandParameter;
            switch (VlcFunc)
            {
                case VLCFuncEnum.Pause:
                    Pause();
                    break;
                case VLCFuncEnum.Play:
                    Play();
                    break;
                case VLCFuncEnum.Stop:
                    Stop();
                    break;
                default:
                    break;
            }

        }

        private void MediaPlayer_PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var play = this.Videos.MediaPlayer;
                Rate.Value = play.Time / 1000;
                RatePlay.Text = TimeSpan.FromSeconds(play.Time / 1000).ToString();
                if (Rate.Value % 60 == 0)
                {

                }
            });
        }

        private void MediaPlayer_Playing(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var play = this.Videos.MediaPlayer;
                Rate.Maximum = play.Length / 1000;
                RateTotal.Text = "/" + TimeSpan.FromSeconds(play.Length / 1000).ToString();
            });
        }

        private void WindowMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void WindowMouseEnter(object sender, MouseEventArgs e)
        {
            VideoHandle.Visibility = Visibility.Visible;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.Videos != null && this.Videos.MediaPlayer != null && this.Videos.MediaPlayer.IsPlaying && VideoHandle.Visibility == Visibility.Visible)
            {
                VideoHandle.Visibility = Visibility.Hidden;
            }
        }
        private void LoadEvent(object sender, RoutedEventArgs e)
        {
            ViewModel = (this.DataContext as CandyVLCViewModel);
        }
        #endregion
    }
}
