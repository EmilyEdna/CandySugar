using CandySugar.Controls.ControlViewModel;
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
            Header.DataContext = CandyContainer.Instance.Resolves<BasicHeaderViewModel>().Basic();
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
