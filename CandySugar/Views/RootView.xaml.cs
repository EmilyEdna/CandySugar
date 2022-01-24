using CandySugar.Controls.UserControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CandySugar.Views
{
    public partial class RootView : CandyWindow
    {
        public RootView()
        {
            InitializeComponent();
        }

        private void WindowMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
