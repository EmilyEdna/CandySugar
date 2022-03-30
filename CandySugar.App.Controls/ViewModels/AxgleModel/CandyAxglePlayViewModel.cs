using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.App.Controls.ViewModels.AxgleModel
{
    public class CandyAxglePlayViewModel : ViewModelNavigatBase
    {
        public CandyAxglePlayViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region Property
        private string _PlayURL;
        public string PlayURL
        {
            get { return _PlayURL; }
            set { SetProperty(ref _PlayURL, value); }
        }
        #endregion

        #region Override
        public override void Initialize(INavigationParameters parameters)
        {
            PlayURL = parameters.GetValue<string>("Route");
        }
        #endregion
    }
}
