using CandySugar.App.Controls.LayoutView;
using CandySugar.Xam.Common;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CandySugar.App.Controls.Views.Music
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandyMusicView : ContentPage
    {
        public CandyMusicView()
        {
            InitializeComponent();
        }

        private void PopupOpened(object sender, EventArgs e)
        {
           Test();
        }

        private void Test() {
            Pop.PopupView.PopupStyle = new PopupStyle
            {
                CornerRadius = 45,
            };
            Pop.PopupView.HeaderTemplate = new DataTemplate(() => new PopHeaderView());
            Pop.PopupView.ShowCloseButton = false;
            Pop.PopupView.ShowFooter=false;
            Pop.PopupView.MinimumWidthRequest = Soft.ScreenWidth - 40;
            Pop.PopupView.MinimumHeightRequest = Soft.ScreenHeight/2;
            Pop.PopupView.AnimationEasing = AnimationEasing.SinIn;
            Pop.PopupView.AnimationMode = AnimationMode.SlideOnBottom;
            Pop.PopupView.ContentTemplate = new DataTemplate(() => new PopContentView());
            Pop.Show(20,300);
        }
    }
}