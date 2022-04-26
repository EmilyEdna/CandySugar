using CandySugar.App.Controls.LayoutView;
using CandySugar.App.Controls.LayoutView.LayoutViewModel;
using CandySugar.App.Controls.ViewModels.AcgAnimeModel;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CandySugar.App.Controls.Views.AcgAnime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandyAcgAnimeView : ContentPage
    {
        public CandyAcgAnimeView()
        {
            InitializeComponent();
        }

        private void ClickCommand(object sender, EventArgs e)
        {
            var vm = (CandyAcgAnimeViewModel)this.BindingContext;
            Pop.PopupView.PopupStyle = new PopupStyle
            {
                CornerRadius = 45,
            };
            Pop.PopupView.ShowHeader = false;
            Pop.PopupView.ShowCloseButton = false;
            Pop.PopupView.ShowFooter = false;
            Pop.PopupView.AnimationEasing = AnimationEasing.SinIn;
            Pop.PopupView.AnimationMode = AnimationMode.SlideOnBottom;
            Pop.PopupView.ContentTemplate = new DataTemplate(() => new PopTagContentView
            {
                BindingContext = new PopTagContentViewModel
                {
                     BrandResult = vm.BrandResult,
                     HType = vm.HType,
                     TagResult = vm.TagResult,
                }
            });
            Pop.Show(true);
        }
    }
}