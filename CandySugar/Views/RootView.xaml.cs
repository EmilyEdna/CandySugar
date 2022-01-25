using CandySugar.Common.Enum;
using CandySugar.Controls.ControlViewModel;
using CandySugar.Controls.UIElementHelper;
using CandySugar.Controls.UserControls;
using CandySugar.Controls.UserControlView;
using CandySugar.Core.CandyUtily;
using StyletIoC;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
