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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CandySugar.UserControlViews.MusicViews
{
    /// <summary>
    /// MusicView.xaml 的交互逻辑
    /// </summary>
    public partial class MusicView : UserControl
    {
        public MusicView()
        {
            InitializeComponent();
        }
        #region 音量
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

        private void VolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = (sender as Slider);
            VolumeShow.Content = (int)slider.Value + "%";
            //MediaPlay.Volume = slider.Value / 100;
        }
        #endregion
    }
}
