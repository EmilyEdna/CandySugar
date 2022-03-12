using CandySugar.Xam.Common.DTO;
using CandySugar.Xam.Common.Entity.Model;
using CandySugar.Xam.Core.Service;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XExten.Advance.LinqFramework;
using XF.Material.Forms.UI.Dialogs;

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

        public ICommand DeleteCommand => new DelegateCommand<XSLiShiDto>(async input =>
        {

            if (await Candy.Remove(input.ToMapest<XS_LiShi>()))
            {
                OnViewLaunch();
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("已从书架中移除"))
                {
                    await Task.Delay(3000);
                }
               
            }
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
