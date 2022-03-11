using CandySugar.App.Controls.ViewModels.NovelModel;
using CandySugar.App.Controls.Views.Novel;
using CandySugar.App.ViewModels;
using CandySugar.App.Views;
using CandySugar.Xam.Common.CandyUtily;
using CandySugar.Xam.Core.Service;
using CandySugar.Xam.Core.ServiceImpl;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using Syncfusion.Licensing;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using XF.Material.Forms;

namespace CandySugar.App
{
    public partial class Root : PrismApplication
    {
        public Root(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected async override void OnInitialized()
        {
            InitializeComponent();
            Material.Init(this);
            SyncfusionLicenseProvider.RegisterLicense("NTg3MTY3QDMxMzkyZTM0MmUzMFdjM01jRGZXbDIxZ1AxVGNkSlluNndGb0d4OFJLd1dzcFJpZVdYc0VQSnM9");
            await NavigationService.NavigateAsync("NavigationPage/CandyIndexView");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterForNavigation<NavigationPage>();

            containerRegistry.Register<IXSLiShi, XSLiShi>();

            #region Novel
            containerRegistry.RegisterForNavigation<CandyIndexView, CandyIndexViewModel>();
            containerRegistry.RegisterForNavigation<CandyNovelView, CandyNovelViewModel>();
            containerRegistry.RegisterForNavigation<CandyNovelDetailView, CandyNovelDetailViewModel>();
            containerRegistry.RegisterForNavigation<CandyNovelContentView, CandyNovelContentViewModel>();
            #endregion

        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            //ViewModelLocationProvider.Register<CandyNovelView>(() => Container.Resolve<CandyNovelViewModel>());
        }
    }
}
