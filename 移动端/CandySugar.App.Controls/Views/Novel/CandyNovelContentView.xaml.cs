using CandySugar.Xam.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CandySugar.App.Controls.Views.Novel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandyNovelContentView : ContentPage
    {
        public CandyNovelContentView()
        {
            InitializeComponent();

            AbsoluteLayout.SetLayoutBounds(Ctrls, new Rectangle(Soft.ScreenWidth / 4, Soft.ScreenHeight-70, 1, 0.2));
        }

        private void Tapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            if(!Ctrls.IsVisible)
                Ctrls.IsVisible=true;
            else
                Ctrls.IsVisible=false;
        }
    }
}