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
    public class CandyButton : Button
    {
        public Geometry Shape
        {
            get { return (Geometry)GetValue(ShapeProperty); }
            set { SetValue(ShapeProperty, value); }
        }

        public static readonly DependencyProperty ShapeProperty =
            DependencyProperty.Register("Shape", typeof(Geometry), typeof(CandyButton), new PropertyMetadata(null));

        public double ShapeWidth
        {
            get { return (double)GetValue(ShapeWidthProperty); }
            set { SetValue(ShapeWidthProperty, value); }
        }

        public static readonly DependencyProperty ShapeWidthProperty =
            DependencyProperty.Register("ShapeWidth", typeof(double), typeof(CandyButton), new PropertyMetadata(14.0));

        public double ShapeHeight
        {
            get { return (double)GetValue(ShapeHeightProperty); }
            set { SetValue(ShapeHeightProperty, value); }
        }

        public static readonly DependencyProperty ShapeHeightProperty =
            DependencyProperty.Register("ShapeHeight", typeof(double), typeof(CandyButton), new PropertyMetadata(14.0));

    }
}
