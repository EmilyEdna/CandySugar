using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CandySugar.App.Controls.LayoutView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopPlayContentView : ContentView
    {
        private int Flag;
        public PopPlayContentView()
        {
            InitializeComponent();
            Flag = 0;
            Menu.Image = ImageSource.FromFile("repeat.png");
        }

        private void ChangedClick(object sender, EventArgs e)
        {
            if (Flag == 0)
            {
                Menu.Image = ImageSource.FromFile("repeat2.png");
                Flag = 1;
            }
            else
            {
                Menu.Image = ImageSource.FromFile("repeat.png");
                Flag = 0;
            }
        }
    }
}