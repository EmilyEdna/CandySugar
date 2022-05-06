using CandySugar.App.Controls.ViewModels.MangaModel;
using CandySugar.Xam.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XExten.Advance.LinqFramework;

namespace CandySugar.App.Controls.Views.Manga
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandyMangaReaderView : ContentPage
    {
        public CandyMangaReaderView()
        {
            InitializeComponent();
        }

        private async void MangaReader_Appearing(object sender, EventArgs e)
        {
            var ViewModel = (CandyMangaReaderViewModel)this.BindingContext;
            var param = await ViewModel.Content();
            HtmlWebViewSource Source = new HtmlWebViewSource();
            Source.Html = Extension.ReadLocalHtml("manga");
            web.Source = Source;
            await Task.Delay(3000);
            if (!param.IsNullOrEmpty())
            {
                var result = await web.EvaluateJavaScriptAsync($"excute({param})");
                ViewModel.ContentBytes(result.ToModel<List<string>>());
            }
        }
    }
}