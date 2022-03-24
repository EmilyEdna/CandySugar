using CandySugar.Xam.Common.Platform;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Prism.Ioc;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;

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
            ContainerLocator.Container.Resolve<IAndroidPlatform>().Transparent();
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Landscape);
        }
    }
}
