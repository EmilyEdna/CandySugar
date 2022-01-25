using CandySugar.Controls.ControlViewModel;
using CandySugar.UserControlView;
using CandySugar.Core.CandyUtily;
using CandySugar.Controls.UserControls;
using System.Windows.Input;

namespace CandySugar.Views
{
    public partial class RootView : CandyWindow
    {
        public RootView()
        {
            InitializeComponent();
            Header.DataContext = CandyContainer.Instance.Resolves<HeaderViewModel>().Baisc();

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
