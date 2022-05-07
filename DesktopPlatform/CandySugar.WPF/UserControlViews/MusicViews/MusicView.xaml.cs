using CandySugar.WPF.CandyWindows;
using CandySugar.WPF.CandyWindows.CandyWinViewModel;
using CandySugar.Common.DTO;
using CandySugar.Common.Enum;
using SDKColloction.MusicSDK.ViewModel.Response;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CandySugar.WPF.UserControlViews.MusicViews
{
    /// <summary>
    /// MusicView.xaml 的交互逻辑
    /// </summary>
    public partial class MusicView : UserControl
    {
        private MusicLyricResult LyricResult;
        private CandyLyricWin CandyLyric;
        /// <summary>
        /// 歌词状态
        /// </summary>
        private bool LyricStutas = false;
        /// <summary>
        /// 当前播放歌曲
        /// </summary>
        private Dictionary<string, CandyPlayListDto> CurrentPlay = null;

        public MusicView()
        {
            InitializeComponent();
            BootResource.Wave = new WaveOutEvent();
            CurrentPlay = new Dictionary<string, CandyPlayListDto>();

            BootResource.Timer = new Timer
            {
                AutoReset = true,
                Interval = 100,
                Enabled = true
            };
            BootResource.Timer.Elapsed += Timer_Elapsed;

            BootResource.LyricTimer = new Timer
            {
                AutoReset = true,
                Interval = 100,
                Enabled = true
            };

            BootResource.LyricTimer.Elapsed += LyricTimer_Elapsed;

            BootResource.Wave.PlaybackStopped += Wave_PlaybackStopped;

            InitMedia();
        }



        #region 初始化
        protected void InitMedia()
        {
            BootResource.Wave.Stop();

            BootResource.Timer.Close();
            BootResource.LyricTimer.Close();

            this.暂停.Visibility = Visibility.Collapsed;
            this.播放.Visibility = Visibility.Visible;

            this.当前播放.Content = "请选择要播放的歌曲...";
            this.时常.Content = "00:00:00";

            this.播放条.Value = 0;
            this.播放条.IsEnabled = false;
        }
        private void LyricTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var Seconds = TimeSpan.Parse(BootResource.Reader.CurrentTime.ToString().Split(".").FirstOrDefault());
                if (LyricResult.Lyrics != null)
                {
                    foreach (var item in LyricResult.Lyrics)
                    {
                        var Target = TimeSpan.Parse("00:" + item.Time.Split(".").FirstOrDefault());
                        if (Target == Seconds)
                        {
                            (CandyLyric.DataContext as CandyLyricViewModel).Lyric = item.Lyric;
                        }
                    }
                }
            });
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (this.播放条.Value == this.播放条.Maximum)
                {
                    InitMedia();
                    PlayConditions();
                    LyricHandles();
                }
                else
                {
                    if (BootResource.Wave.PlaybackState == PlaybackState.Playing)
                    {
                        this.时常.Content = BootResource.Reader.CurrentTime.ToString().Split(".").FirstOrDefault();
                        this.播放条.Value = BootResource.Reader.CurrentTime.TotalSeconds;
                    }
                }
            });
        }

        private void Wave_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            BootResource.Wave.Stop();
        }

        public async Task LyricData()
        {
            MusicLyricResult result = await this.ViewModel.LoadLyric(CurrentPlay.Values.FirstOrDefault());
            if (result == null)
                return;
            LyricResult = result;
        }

        private MusicViewModel ViewModel => this.DataContext as MusicViewModel;
        #endregion

        #region 功能按钮组
        /// <summary>
        /// 音量设置
        /// </summary>
        private int VolumeAnime = 0;
        private void VolumeClick(object sender, MouseButtonEventArgs e)
        {
            if (VolumeAnime == 0)
            {
                BeginStoryboard((Storyboard)FindResource("Open"));
                VolumeAnime = 1;
            }
            else
            {
                BeginStoryboard((Storyboard)FindResource("Close"));
                VolumeAnime = 0;
            }
        }

        private void VolumeSetting(object sender, MouseEventArgs e)
        {
            BeginStoryboard((Storyboard)FindResource("Close"));
            VolumeAnime = 0;
        }

        private void VolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = (sender as Slider);
            VolumeShow.Content = (int)slider.Value + "%";
            if (BootResource.Wave != null)
                BootResource.Wave.Volume = (float)(slider.Value / 100);
        }
        /// <summary>
        /// 播放列表
        /// </summary>
        private int PlayListAnime = 0;
        private void PlayListClick(object sender, MouseButtonEventArgs e)
        {
            if (PlayListAnime == 0)
            {
                BeginStoryboard((Storyboard)FindResource("PlayOpen"));
                PlayListAnime = 1;
            }
            else
            {
                BeginStoryboard((Storyboard)FindResource("PlayClose"));
                PlayListAnime = 0;
            }
        }
        #endregion

        #region 播放条件
        protected void PlayConditions()
        {
            if (播放列表.Items.Count <= 0)
                return;
            //没有选中默认选列表循环
            if (播放条件.SelectedIndex == -1) { 播放条件.SelectedIndex = 0; }
            //初始化播放器
            InitMedia();
            //列表循环
            if (播放条件.SelectedIndex == 0)
            {
                //是否到底部
                if (播放列表.SelectedIndex + 1 == 播放列表.Items.Count)
                {
                    var PlayList = ViewModel.PlayLists.FirstOrDefault();
                    Plays(PlayList);
                    播放列表.SelectedIndex = 0;
                }
                else
                {
                    Plays(ViewModel.PlayLists[播放列表.SelectedIndex + 1]);
                    播放列表.SelectedIndex += 1;//定位
                }
            }
            //单曲循环
            if (播放条件.SelectedIndex == 1)
            {
                var index = 播放列表.SelectedIndex == -1 ? 0 : 播放列表.SelectedIndex;
                Plays(ViewModel.PlayLists[index]);
                播放列表.SelectedIndex = index;

            }
            //随机播放
            if (播放条件.SelectedIndex == 2)
            {
                if (播放列表.Items.Count == 1)
                {
                    Plays(ViewModel.PlayLists.FirstOrDefault());
                    播放列表.SelectedIndex = 0;//定位

                }
                else
                {
                    int stc = 0;//循环条件
                    while (stc == 0)//循环
                    {
                        int i = new Random().Next(播放列表.Items.Count - 1);//随机范围在列表最大-1
                        if (播放列表.SelectedIndex != i)//排除当前正在播放的
                        {
                            Plays(ViewModel.PlayLists[i]);//播放歌曲
                            播放列表.SelectedIndex = i;//定位
                            stc = 1;//条件排除

                        }
                    }
                }
            }
            //顺序播放
            if (播放条件.SelectedIndex == 3)
            {
                if (播放列表.SelectedIndex + 1 != 播放列表.Items.Count)
                {
                    Plays(ViewModel.PlayLists[播放列表.SelectedIndex + 1]);
                    播放列表.SelectedIndex += 1;

                }
            }
        }
        protected void Plays(CandyPlayListDto input)
        {
            CurrentPlay.Clear();
            CurrentPlay.Add(nameof(CandyPlayListDto), input);
            this.当前播放.Content = input.SongName;
            NPlay(input);
            this.播放条.IsEnabled = true;
            播放.Visibility = Visibility.Collapsed;
            暂停.Visibility = Visibility.Visible;
            BootResource.Timer.Start();
        }

        /// <summary>
        /// 暂时没用
        /// </summary>
        protected void SetSelect()
        {
            for (int index = 0; index < this.播放列表.Items.Count; index++)
            {
                var item = (this.播放列表.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem);

                if (index == this.播放列表.SelectedIndex)
                {
                    item.IsSelected = true;
                    item.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#33C18D32"));
                }
                else
                {
                    item.IsSelected = false;
                    item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF"));
                }

            }
        }
        #endregion

        #region 播放
        private void PlayHandle(object sender, RoutedEventArgs e)
        {
            var btn = (sender as Button);
            var Enum = (MusicPlayFuncEnum)btn.CommandParameter;
            switch (Enum)
            {
                case MusicPlayFuncEnum.SkipPrevious:
                    {
                        InitMedia();
                        if (播放条件.SelectedIndex != -1 && ViewModel.PlayLists.Count > 0)
                        {
                            if (播放条件.SelectedIndex == 0 || 播放条件.SelectedIndex == 1 || 播放条件.SelectedIndex == 3)
                            {
                                if (播放列表.SelectedIndex == 0)//从最底部播放
                                {
                                    Plays(ViewModel.PlayLists[播放列表.Items.Count - 1]);
                                    播放列表.SelectedIndex = 播放列表.Items.Count - 1;

                                }
                                else//正常上一曲
                                {
                                    if (播放列表.SelectedIndex == -1)
                                    {
                                        Plays(ViewModel.PlayLists[播放列表.Items.Count - 1]);
                                        播放列表.SelectedIndex = 播放列表.Items.Count - 1;

                                    }
                                    else
                                    {
                                        Plays(ViewModel.PlayLists[播放列表.SelectedIndex - 1]);
                                        播放列表.SelectedIndex -= 1;

                                    }
                                }
                            }
                            else if (播放条件.SelectedIndex == 2) { PlayConditions(); }
                        }
                        else
                        {
                            if (播放列表.Items.Count != 0)
                            {
                                播放条件.SelectedIndex = 0;
                                PlayHandle(btn, e);
                            }
                            else
                                HandyControl.Controls.Growl.Info("木有任何歌曲（；´д｀）ゞ");
                        }
                        LyricHandles();
                        break;
                    }
                case MusicPlayFuncEnum.Play:
                    {
                        if (BootResource.Wave.PlaybackState == PlaybackState.Paused)
                        {
                            BootResource.Wave.Play();
                            this.暂停.Visibility = Visibility.Visible;
                            this.播放.Visibility = Visibility.Collapsed;
                            BootResource.Timer.Start();
                            BootResource.LyricTimer.Start();
                        }
                        else
                            PlayConditions();
                        break;
                    }
                case MusicPlayFuncEnum.Pause:
                    {
                        BootResource.Wave.Pause();
                        this.暂停.Visibility = Visibility.Collapsed;
                        this.播放.Visibility = Visibility.Visible;
                        BootResource.Timer.Stop();
                        BootResource.LyricTimer.Stop();
                        break;
                    }
                case MusicPlayFuncEnum.SkipNext:
                    {
                        InitMedia();
                        if (播放条件.SelectedIndex != -1 && ViewModel.PlayLists.Count > 0)
                        {
                            if (播放条件.SelectedIndex == 0 || 播放条件.SelectedIndex == 1 || 播放条件.SelectedIndex == 3)
                            {
                                if (播放列表.SelectedIndex == 0)//从最底部播放
                                {
                                    Plays(ViewModel.PlayLists[播放列表.Items.Count - 1]);
                                    播放列表.SelectedIndex = 播放列表.Items.Count - 1;

                                }
                                else//正常上一曲
                                {
                                    if (播放列表.SelectedIndex == -1)
                                    {
                                        Plays(ViewModel.PlayLists[播放列表.Items.Count - 1]);
                                        播放列表.SelectedIndex = 播放列表.Items.Count - 1;

                                    }
                                    else
                                    {
                                        Plays(ViewModel.PlayLists[播放列表.SelectedIndex - 1]);
                                        播放列表.SelectedIndex -= 1;

                                    }
                                }
                            }
                            else
                            {
                                PlayConditions();
                            }
                        }
                        else
                        {
                            if (播放列表.Items.Count != 0)
                            {
                                播放条件.SelectedIndex = 0;
                                PlayHandle(btn, e);
                            }
                            else
                                HandyControl.Controls.Growl.Info("木有任何歌曲（；´д｀）ゞ");
                        }
                        LyricHandles();
                        break;
                    }
                default:
                    break;
            }
        }
        private async void LyricHandle(object sender, MouseButtonEventArgs e)
        {
            await LyricData();
            LyricStutas = BootResource.Lyric(window =>
            {
                window.DataContext = ViewModel.GetContainer<CandyLyricViewModel>();
                if (BootResource.Wave.PlaybackState == PlaybackState.Playing) BootResource.LyricTimer.Start();
                else BootResource.LyricTimer.Close();

                CandyLyric = window;
            }) >= 1;
        }
        private async void LyricHandles()
        {
            if (LyricStutas)
            {
                await LyricData();

                LyricStutas = BootResource.Lyric(window =>
                {
                    window.DataContext = ViewModel.GetContainer<CandyLyricViewModel>();
                    if (BootResource.Wave.PlaybackState == PlaybackState.Playing) BootResource.LyricTimer.Start();
                    else BootResource.LyricTimer.Close();
                    CandyLyric = window;
                }, 3) >= 1;
            }
        }
        #endregion

        #region NAudio
        protected void NPlay(CandyPlayListDto input)
        {
            try
            {
                BootResource.Wave.Dispose();
                BootResource.Wave = new WaveOutEvent();
                BootResource.Reader = new MediaFoundationReader(input.CacheAddress);
                时常.Content = BootResource.Reader.TotalTime.ToString().Split(".").FirstOrDefault();
                this.播放条.Maximum = BootResource.Reader.TotalTime.TotalSeconds;
                this.播放条.Value = 0;
                BootResource.Wave.Init(BootResource.Reader);
                BootResource.Wave.Play();
            }
            catch
            {
                HandyControl.Controls.Growl.Info("播放失败，未检测到音频设备，请检查设备驱动");
            }

        }
        #endregion
    }
}
