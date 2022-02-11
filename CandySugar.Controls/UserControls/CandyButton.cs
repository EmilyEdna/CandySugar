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
    }
}
