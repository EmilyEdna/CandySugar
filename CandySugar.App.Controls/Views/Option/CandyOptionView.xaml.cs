using CandySugar.App.Controls.ViewModels.OptionModel;
using CandySugar.Xam.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XExten.Advance.LinqFramework;

namespace CandySugar.App.Controls.Views.Option
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandyOptionView : ContentPage
    {
        public CandyOptionView()
        {
            InitializeComponent();
        }

        private int Age = 0;
        private void MaterialButton_Clicked(object sender, EventArgs e)
        {
            var viewmodel = (this.BindingContext as CandyOptionViewModel);
            if (viewmodel.Option == null)
                viewmodel.Option = new CandySettingDto();
            viewmodel.Option.ProxyPort = this.ProxyPort.Text == "0" ? -1 : Convert.ToInt32(this.ProxyPort.Text);
            viewmodel.Option.ProxyIP = this.ProxyIP.Text;
            viewmodel.Option.ProxyAccount = this.ProxyAccount.Text;
            viewmodel.Option.ProxyPwd = this.ProxyPwd.Text;
            viewmodel.Option.WaitSpan = this.WaitSpan.Text == "0" ? 500 : Convert.ToInt32(this.WaitSpan.Text);
            viewmodel.Option.CacheTime = this.CacheTime.Text == "0" ? 60 : Convert.ToInt32(this.CacheTime.Text);
            viewmodel.Option.AgeModule = Age;
            viewmodel.Save(viewmodel.Option);
        }

        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var radio = (sender as RadioButton);
            Age = radio.Value.AsString().AsInt();
        }
    }
}