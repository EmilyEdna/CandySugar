using CandySugar.App.Controls.ViewModels.AnimeModel;
using CandySugar.Xam.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CandySugar.App.Controls.Views.Anime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandyAnimePlayView : ContentPage
    {
        public CandyAnimePlayView()
        {
            InitializeComponent();
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            var PlayURL = (this.BindingContext as CandyAnimePlayViewModel).PlayURL;
            HtmlWebViewSource Source = new HtmlWebViewSource();
            Source.Html = Extension.ReadLocalHtml();
            Source.BaseUrl = Extension.AndroidAssetsPath;
            web.Source = Source;
            await Task.Delay(5000);
            await web.EvaluateJavaScriptAsync($"Play('{PlayURL}')");
        }
    }
}