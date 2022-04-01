using CandySugar.Xam.Common.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using CandySugar.App.Controls.ViewModels.AxgleModel;
using CandySugar.Xam.Common;
using XExten.Advance.LinqFramework;

namespace CandySugar.App.Controls.Views.Axgle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandyAxglePlayView : ContentPage
    {
        public CandyAxglePlayView()
        {
            InitializeComponent();
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

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            ContainerLocator.Container.Resolve<IAndroidPlatform>().ShowStatusBar();
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        }

        private void BtnClicked(object sender, EventArgs e)
        {
            /*          
           var PlayURL = (this.BindingContext as CandyAxglePlayViewModel).PlayURL;
           HtmlWebViewSource Source = new HtmlWebViewSource();
           Source.Html = Extension.ReadLocalHtml("axgle");
           web.Source = Source;
           await Task.Delay(3000);
           await web.EvaluateJavaScriptAsync($"Init('{PlayURL}')");
          */

            var param = (sender as Button).CommandParameter.ToString().AsInt();

            if (param == 0)
            {
                ContainerLocator.Container.Resolve<IAndroidPlatform>().HiddenStatusBar();
                CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Landscape);
            }
            else if (param == 1)
            {
                ContainerLocator.Container.Resolve<IAndroidPlatform>().ShowStatusBar();
                CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
            }
            else if (param == 2)
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
                //sb.Append("$('#video-player').css({'max-width':'1190px','width':'1190px','margin-left':'-30px'});");
                web.EvaluateJavaScriptAsync(sb.ToString());
            }
            else {
                web.Reload();
            }
        }
    }
}