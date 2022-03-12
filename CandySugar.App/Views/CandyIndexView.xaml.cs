using CandySugar.App.Controls.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandySugar.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CandySugar.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandyIndexView : ContentPage
    {
        public CandyIndexView()
        {
            InitializeComponent();  
        }

        private void OpenSlider(object sender, EventArgs e)
        {
            drawer.ToggleDrawer();
        }
    }
}