using CandySugar.App.ViewModels;
using CandySugar.App.Views;
using Prism;
using Prism.Ioc;
using Prism.Mvvm;
using Syncfusion.Licensing;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace CandySugar.App
{
    public partial class Root
    {
        public Root(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override void OnInitialized()
        {
            InitializeComponent();
            SyncfusionLicenseProvider.RegisterLicense("NTg3MTY3QDMxMzkyZTM0MmUzMFdjM01jRGZXbDIxZ1AxVGNkSlluNndGb0d4OFJLd1dzcFJpZVdYc0VQSnM9");
            //await NavigationService.NavigateAsync("NavigationPage/Index");
            MainPage = new Index();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            //containerRegistry.RegisterForNavigation<NavigationPage>();
            ViewModelLocationProvider.Register<Index, IndexViewModel>();
        }
    }
}
