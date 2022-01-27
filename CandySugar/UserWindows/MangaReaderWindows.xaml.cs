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

namespace CandySugar.UserWindows
{
    /// <summary>
    /// MangaReaderWindows.xaml 的交互逻辑
    /// </summary>
    public partial class MangaReaderWindows : CandyWindow
    {
        public MangaReaderWindows()
        {
            InitializeComponent();
            Header.DataContext = CandyContainer.Instance.Resolves<MangaHeaderViewModel>().Basic();
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
