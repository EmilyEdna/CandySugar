using CandySugar.CandyWindows;
using CandySugar.CandyWindows.CnadyWinViewModel;
using CandySugar.Common.DTO;
using CandySugar.Common.Enum;
using Music.SDK.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Timer = System.Timers.Timer;

namespace CandySugar.UserControlViews.MusicViews
{
    /// <summary>
    /// MusicView.xaml 的交互逻辑
    /// </summary>
    public partial class MusicView : UserControl
    {
        /// <summary>
        /// 播放定时器
        /// </summary>
        public Timer Timer;
        /// <summary>
        /// 歌词定时器
        /// </summary>
        public Timer LyricTimer;
        /// <summary>
        /// 播放状态1表示播放中0表示为播放
        /// </summary>
        private bool PlayState = false;
        /// <summary>
        /// 歌词状态1表示播放中0表示为播放
        /// </summary>
        private bool LyricState = false;
        /// <summary>
        /// 当前播放歌曲
        /// </summary>
        private Dictionary<string, CandyPlayListDto> CurrentPlay = null;
        private Dictionary<string, CandyLyricWin> LryicWin = null;

        public MusicView()
        {
            InitializeComponent();

            CurrentPlay = new Dictionary<string, CandyPlayListDto>();
            LryicWin = new Dictionary<string, CandyLyricWin>();

            Timer = new Timer
            {
                AutoReset = true,
                Interval = 100,
                Enabled = true
            };
            Timer.Elapsed += Timer_Elapsed;

            LyricTimer = new Timer
            {
                AutoReset = true,
                Interval = 100,
                Enabled = true
            };

            InitMedia();
        }

        #region 初始化
        protected void InitMedia()
        {
            MediaPlay.Source = null;
            MediaPlay.Close();

            Timer.Close();
            LyricTimer.Close();

            this.暂停.Visibility = Visibility.Collapsed;
            this.播放.Visibility = Visibility.Visible;

            this.当前播放.Content = "请选择要播放的歌曲...";
            this.时常.Content = "00:00:00";

            this.播放条.Value = 0;
            this.播放条.IsEnabled = false;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (this.播放条.Value == this.播放条.Maximum)
                {
                    InitMedia();
                    PlayConditions();
                }
                else
                {
                    if (this.MediaPlay.Source != null && PlayState)
                    {
                        this.时常.Content = this.MediaPlay.Position.ToString().Split(".").FirstOrDefault();
                        this.播放条.Value = this.MediaPlay.Position.TotalSeconds;
                    }
                }
            });
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
            if (MediaPlay != null)
                MediaPlay.Volume = slider.Value / 100;
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
            MediaPlay.Close();
            MediaPlay.Source = new Uri(input.CacheAddress, UriKind.Absolute);
            MediaPlay.Play();
            LoadTime();
            播放.Visibility = Visibility.Collapsed;
            暂停.Visibility = Visibility.Visible;
            PlayState = true;
            Timer.Start();
        }
        /// <summary>
        /// 加载音频的时常
        /// </summary>
        private void LoadTime()
        {
            Dispatcher.Invoke(() =>
            {
                Thread.Sleep(200);
                var time = !MediaPlay.NaturalDuration.HasTimeSpan ? "" : MediaPlay.NaturalDuration.TimeSpan.ToString()?.Split(".")?.FirstOrDefault();
                if (string.IsNullOrEmpty(time))
                    LoadTime();
                else
                {
                    时常.Content = time;
                    this.播放条.IsEnabled = true;
                    this.播放条.Maximum = int.Parse(time.Substring(3, 2)) * 60 + int.Parse(time.Substring(6, 2));
                    this.播放条.Value = 0;
                    return;
                }
            });
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
                                HandyControl.Controls.MessageBox.Info("木有任何歌曲（；´д｀）ゞ", "提示");
                        }
                        LyricState = false;
                        LyricHandle(null, null);
                        break;
                    }
                case MusicPlayFuncEnum.Play:
                    {
                        if (MediaPlay.NaturalDuration.HasTimeSpan)
                        {
                            MediaPlay.Play();
                            this.暂停.Visibility = Visibility.Visible;
                            this.播放.Visibility = Visibility.Collapsed;
                            this.Timer.Start();
                            this.LyricTimer.Start();
                            PlayState = true;
                        }
                        else
                            PlayConditions();
                        break;
                    }
                case MusicPlayFuncEnum.Pause:
                    {
                        this.MediaPlay.Pause();
                        this.暂停.Visibility = Visibility.Collapsed;
                        this.播放.Visibility = Visibility.Visible;
                        this.Timer.Stop();
                        this.LyricTimer.Stop();
                        PlayState = false;
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
                                HandyControl.Controls.MessageBox.Info("木有任何歌曲（；´д｀）ゞ", "提示");
                        }
                        LyricState = false;
                        LyricHandle(null, null);
                        break;
                    }
                default:
                    break;
            }
        }
        private async void LyricHandle(object sender, MouseButtonEventArgs e)
        {
            if (this.MediaPlay.Source != null && LyricState == false)
            {
                MusicLyricResult result = await this.ViewModel.LoadLyric(CurrentPlay.Values.FirstOrDefault());
                if (result == null && result.Lyrics == null)
                    return;
                LyricState = true;
                CandyLyricWin win = new();
                win.DataContext = ViewModel.GetContainer<CandyLyricViewModel>();

                LyricTimer.Elapsed += (s, e) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        var Seconds = this.MediaPlay.Position.ToString().Split(".").FirstOrDefault();
                        if (result.Lyrics != null)
                        {
                            foreach (var item in result.Lyrics)
                            {
                                var Target = "00:" + item.Time.Split(".").FirstOrDefault();
                                if (Target == Seconds)
                                {
                                    (win.DataContext as CandyLyricViewModel).Lyric = item.Lyric;
                                }
                            }
                        }

                    });
                };

                win.WindowStartupLocation = WindowStartupLocation.Manual;
                win.Top = (SystemParameters.PrimaryScreenHeight / 10) * 7.5;
                win.Left = (SystemParameters.PrimaryScreenWidth / 10) * 1.9;
                win.Topmost = true;
                win.Show();
                if (LryicWin.Count != 0)
                {
                    LryicWin.Values.FirstOrDefault().Close();
                    LryicWin.Clear();
                }
                LryicWin.Add(nameof(CandyLyricWin), win);
                LyricTimer.Start();
                return;
            }
            if (this.MediaPlay.Source == null || LyricState == true)
            {
                var win = LryicWin.Values.FirstOrDefault();
                if (win == null)
                    return;
                (win.DataContext as CandyLyricViewModel).Lyric = String.Empty;
                win.Close();
                LyricTimer.Close();
                LyricState = false;
            }
        }
        #endregion

    }
}
