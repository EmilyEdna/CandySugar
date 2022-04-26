using SDKColloction.AcgAnimeSDK.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF = Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CandySugar.Xam.Common;

namespace CandySugar.App.Controls.LayoutView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopTagContentView : ContentView
    {
        public PopTagContentView()
        {
            InitializeComponent();
        }

        private void TypeCheck(object sender, CheckedChangedEventArgs e)
        {
            var rb = (RadioButton)sender;
            Extension.AcgType = rb.Value.ToString();
        }

        private void TagCheck(object sender, SF.StateChangedEventArgs e)
        {
            var ck = (SF.SfCheckBox)sender;
            if (Extension.AcgTags.Contains(ck.Text))
                Extension.AcgTags.Remove(ck.Text);
            else
                Extension.AcgTags.Add(ck.Text);
        }

        private void BrandCheck(object sender, SF.StateChangedEventArgs e)
        {
            var ck = (SF.SfCheckBox)sender;
            if (Extension.AcgBrands.Contains(ck.Text))
                Extension.AcgBrands.Remove(ck.Text);
            else
                Extension.AcgBrands.Add(ck.Text);
        }
    }
}