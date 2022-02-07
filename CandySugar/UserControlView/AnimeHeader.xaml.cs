using CandySugar.Common.Enum;
using CandySugar.Properties;
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
        }

        private void ThemeSelected(object sender, SelectionChangedEventArgs e)
        {
            var select = (sender as ComboBox);
            var item = (ThemeFuncEnum)(select.Items[select.SelectedIndex] as ComboBoxItem).TabIndex;
            switch (item)
            {
                case ThemeFuncEnum.BaoShilv:
                    Soft.Default.Theme = "#FF2CCFA0";
                    break;
                case ThemeFuncEnum.TaoHuafen:
                    Soft.Default.Theme = "#FFFF9999";
                    break;
                case ThemeFuncEnum.XuZilan:
                    Soft.Default.Theme = "#FF10AEC2";
                    break;
                case ThemeFuncEnum.ShanChahong:
                    Soft.Default.Theme = "#FFED556A";
                    break;
                case ThemeFuncEnum.MoYuhei:
                    Soft.Default.Theme = "#FF000000";
                    break;
                case ThemeFuncEnum.ZiShuhong:
                    Soft.Default.Theme = "#FFEF4289";
                    break;
                default:
                    break;
            }
        }
    }
}
