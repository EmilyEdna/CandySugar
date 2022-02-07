using CandySugar.Controls.ControlViewModel;
using CandySugar.UserControlView;
using CandySugar.Core.CandyUtily;
using CandySugar.Controls.UserControls;
using System.Windows.Input;
using CandySugar.CandyWindows;

namespace CandySugar.Views
{
    public partial class RootView : CandyWindow
    {
        public RootView()
        {
            InitializeComponent();
            Header.DataContext = CandyContainer.Instance.Resolves<BasicHeaderViewModel>().Basic();
            CandyVLCWin w = new CandyVLCWin();
            w.Show();
        }

        private void WindowMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ProcessClick(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
