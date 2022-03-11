using CandySugar.Xam.Common.DTO;
using CandySugar.Xam.Core.Service;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using XExten.Advance.LinqFramework;

namespace CandySugar.App.Controls.ViewModels
{
    public class CandyContentIndexViewModel : ViewModelBase
    {
        public CandyContentIndexViewModel() : base()
        {
            Candy = ContainerLocator.Container.Resolve<IXSLiShi>();
        }

        #region Field
        private readonly IXSLiShi Candy;
        #endregion

        #region Property
        private ObservableCollection<XSLiShiDto> _XSLiShi;
        public ObservableCollection<XSLiShiDto> XSLiShi
        {
            get { return _XSLiShi; }
            set { SetProperty(ref _XSLiShi, value); }
        }
        #endregion

        #region Command
        public ICommand ClickCommand => new DelegateCommand<XSLiShiDto>(async input =>
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("ChapterURL", input.ChapeterAddress);
            param.Add("BookName", input.BookName);
            await ContainerLocator.Container.Resolve<INavigationService>().NavigateAsync(new Uri("CandyNovelContentView", UriKind.Relative), param);
        });
        #endregion

        #region Override
        protected override async void OnViewLaunch()
        {
            XSLiShi = new ObservableCollection<XSLiShiDto>((await ContainerLocator.Container.Resolve<IXSLiShi>().Query()).ToMapest<List<XSLiShiDto>>());
        }
        #endregion
    }
}
