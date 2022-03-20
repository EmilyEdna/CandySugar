using CandySugar.CandyWindows.CandyWinViewModel;
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
    /// CandyAxgleWin.xaml 的交互逻辑
    /// </summary>
    public partial class CandyAxgleWin : CandyWindow
    {
        public CandyAxgleWin()
        {
            InitializeComponent();
            Header.DataContext = CandyContainer.Instance.Resolves<AxgleHeaderViewModel>().Basic();
            BeginAnime(OpenWindow);
        }

        private void WindowMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private CandyAxViewModel ViewModel;
        private void LoadEvent(object sender, RoutedEventArgs e)
        {
            ViewModel = (this.DataContext as CandyAxViewModel);
            InitializeAsync();
        }

        async void InitializeAsync()
        {
            await AxWebView.EnsureCoreWebView2Async(null);

            AxWebView.CoreWebView2.Navigate(ViewModel.Watch);

            AxWebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            AxWebView.CoreWebView2.Settings.AreDevToolsEnabled = false;
        }
    }
}
