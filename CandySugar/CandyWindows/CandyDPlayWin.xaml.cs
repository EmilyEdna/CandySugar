using CandySugar.CandyWindows.CnadyWinViewModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CandySugar.CandyWindows
{
    /// <summary>
    /// CandyDPlayWin.xaml 的交互逻辑
    /// </summary>
    public partial class CandyDPlayWin : CandyWindow
    {
        public CandyDPlayWin()
        {
            InitializeComponent();
            Header.DataContext = CandyContainer.Instance.Resolves<AnimeHeaderViewModel>().Basic();
            BeginAnime(OpenWindow);
        }


        private void WindowMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private CandyDPlayViewModel ViewModel;
        private void LoadEvent(object sender, RoutedEventArgs e)
        {
            ViewModel = (this.DataContext as CandyDPlayViewModel);
            InitializeAsync();
        }

        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);

            webView.CoreWebView2.Navigate(new Uri($"{Environment.CurrentDirectory}\\AppData\\index.html").AbsoluteUri);

            webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            webView.CoreWebView2.Settings.AreDevToolsEnabled = false;
        }
    }
}
