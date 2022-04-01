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
            SfPopupLayout Popup= new SfPopupLayout();
            Popup.PopupView.HeaderTitle = "播放列表";
            Popup.PopupView.AnimationEasing = AnimationEasing.SinIn;
            Popup.PopupView.AnimationMode = AnimationMode.SlideOnBottom;
            Popup.PopupView.ContentTemplate = new DataTemplate(() =>
            {
                return new Label()
                {
                    Text = "1231245"
                };
            });
            Popup.IsOpen = true;
        }
    }
}