using CandySugar.Controls.ControlViewModel;
using CandySugar.Controls.UserControls;
using CandySugar.Core.CandyUtily;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CandySugar.CandyWindows
{
    /// <summary>
    /// CandyLightNovelWin.xaml 的交互逻辑
    /// </summary>
    public partial class CandyLightNovelWin : CandyWindow
    {
        public CandyLightNovelWin()
        {
            InitializeComponent();
            Header.DataContext = CandyContainer.Instance.Resolves<NormalHeaderViewModel>().Basic();
        }
        private void WindowMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void WindowColor(object sender, RoutedEventArgs e)
        {
            this.Word.Foreground = (sender as Button).Background;
        }
    }
}
