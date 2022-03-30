using CandySugar.CandyWindows.CandyWinViewModel;
using CandySugar.Controls.ControlViewModel;
using CandySugar.Controls.UserControls;
using CandySugar.Core.CandyUtily;
using Microsoft.Web.WebView2.Core;
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

        private string[] ClassName = { "alert alert-dismissable alert-danger",
            "hd-text-icon",
            "top-nav",
            "well well-filters",
            "navbar navbar-inverse navbar-fixed-top",
            "nav nav-tabs",
            "tab-content m-b-20",
            "pull-left user-container",
            "pull-right big-views hidden-xs",
            "m-t-10 overflow-hidden",
            "col-md-4 col-sm-5",
            "footer-container",
            "col-lg-12",
            "fps60-text-icon",
            "btn btn-primary",
            "vote-box col-xs-7 col-sm-2 col-md-2",
            "pull-right m-t-15",
            "video-banner"};

        private CandyAxViewModel ViewModel;
        private void LoadEvent(object sender, RoutedEventArgs e)
        {
            ViewModel = (this.DataContext as CandyAxViewModel);
            InitializeAsync();
        }

        async void InitializeAsync()
        {
            await AxWebView.EnsureCoreWebView2Async(null);

            AxWebView.CoreWebView2.Navigate(new Uri($"{Environment.CurrentDirectory}\\AppData\\axgle.html").AbsoluteUri);

            AxWebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            AxWebView.CoreWebView2.Settings.AreDevToolsEnabled = false;
            await Task.Delay(2000);
            await AxWebView.CoreWebView2.ExecuteScriptAsync($"Init('{ViewModel.Watch}')");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in ClassName)
            {
                sb.Append($"$(document.getElementsByClassName('{item}')).remove();");
            }
            sb.Append("$(document.getElementById('ps32-container')).remove();");
            sb.Append("$(document.getElementsByTagName('iframe')).remove();");
            sb.Append("$('div[style*=\"position:absolute;left:18px;display: block;font-size:10px;\"]').remove();");
            sb.Append("$('div[style*=\"position:absolute;right:18px; display: block;font-size:10px;\"]').remove();");
            sb.Append("$('#wrapper').css('padding-bottom','0px');");
            sb.Append("$('body').css('padding-top','0px');");
            sb.Append("$('#video-player').css({'max-width':'1190px','width':'1190px','margin-left':'-30px'});");
            AxWebView.CoreWebView2.ExecuteScriptAsync(sb.ToString());
        }
    }
}
