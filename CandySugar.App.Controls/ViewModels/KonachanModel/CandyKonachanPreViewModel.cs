using CandySugar.Xam.Common.Platform;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.App.Controls.ViewModels.KonachanModel
{
    public class CandyKonachanPreViewModel : ViewModelNavigatBase
    {
        public CandyKonachanPreViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        private string _Route;
        public string Route
        {
            get => _Route;
            set => SetProperty(ref _Route, value);
        }
        public override void Initialize(INavigationParameters parameters)
        {
            Route = parameters.GetValue<string>("Route");
        }
    }
}
