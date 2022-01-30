using CandySugar.Common.Enum;
using Microsoft.Web.WebView2.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CandySugar.UserControlView
{
    /// <summary>
    /// AnimeHeader.xaml 的交互逻辑
    /// </summary>
    public partial class AnimeHeader : UserControl
    {
        public AnimeHeader()
        {
            InitializeComponent();
        }
        private void CandySystemClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var types = (SysFuncEnum)btn.CommandParameter;
            var win = Window.GetWindow(this);
            var webView = (win.FindName("webView") as WebView2);
            switch (types)
            {
                case SysFuncEnum.Play:
                    webView.CoreWebView2.ExecuteScriptAsync($"Play('{(win.DataContext as dynamic).WatchResult.PlayURL}')");
                    break;
                case SysFuncEnum.Download:
                    break;
                case SysFuncEnum.Setting:
                    break;
                case SysFuncEnum.MinSize:
                    Min();
                    break;
                case SysFuncEnum.Close:
                    webView.CoreWebView2.ExecuteScriptAsync($"Destory()");
                    Close();
                    break;
                default:
                    break;
            }
        }

        private void Min()
        {
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Visibility = Visibility.Collapsed;
            }
        }
        private void Close()
        {
            Window.GetWindow(this).Close();
            //Application.Current.Shutdown();
        }
    }
}
