using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CandySugar.Controls.UserControls
{
    public class CandyWindow : Window
    {

        public string OpenWindow = "OpenWindow";
        public string CloseWindow = "CloseWindow";
        public string GiftClose = "GiftClose";
        public string GiftOpen = "GiftOpen";

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(CandyWindow), new PropertyMetadata(null));

        public bool Loading
        {
            get { return (bool)GetValue(LoadingProperty); }
            set { SetValue(LoadingProperty, value); }
        }

        public static readonly DependencyProperty LoadingProperty =
            DependencyProperty.Register("Loading", typeof(bool), typeof(CandyWindow), new PropertyMetadata(false));

        public void BeginAnime(string Key)
        {
            BeginStoryboard((Storyboard)FindResource(Key));
        }
    }
}
