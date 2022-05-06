using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CandySugar.Controls.UserControls
{
    public class CandyImgButton: Button
    {
        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(CandyImgButton), new PropertyMetadata(null));

        public string Total
        {
            get { return (string)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }

        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("Total", typeof(string), typeof(CandyImgButton), new PropertyMetadata(TotalChanged));

        private static void TotalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CandyImgButton)d).Total= (string)e.NewValue;
        }
    }
}
