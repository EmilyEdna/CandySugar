using CandySugar.App.Controls.LayoutView;
using CandySugar.App.Controls.LayoutView.LayoutViewModel;
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
            
        }

        private void PopPlayList()
        {
            var HeaderView = new PopPlayHeaderView();
            var ContentView = new PopPlayContentView();
            (ContentView.BindingContext as PopPlayContentViewModel).PopPlayHeaderView = HeaderView;
            PopCommon(HeaderView, ContentView);
            Pop.Show(20, 300);
        }
        private void PopSheet() { 
        
        }

        private void PopCommon(ContentView HeaderView, ContentView ContentView) 
        {
            Pop.PopupView.PopupStyle = new PopupStyle
            {
                CornerRadius = 45,
            };
            Pop.PopupView.HeaderTemplate = new DataTemplate(() => HeaderView);
            Pop.PopupView.ShowCloseButton = false;
            Pop.PopupView.ShowFooter = false;
            Pop.PopupView.MinimumWidthRequest = Soft.ScreenWidth - 40;
            Pop.PopupView.MinimumHeightRequest = Soft.ScreenHeight / 2;
            Pop.PopupView.AnimationEasing = AnimationEasing.SinIn;
            Pop.PopupView.AnimationMode = AnimationMode.SlideOnBottom;
            Pop.PopupView.ContentTemplate = new DataTemplate(() => ContentView);
        }
    }
}