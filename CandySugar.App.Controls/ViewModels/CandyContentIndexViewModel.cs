using CandySugar.App.Controls.Views.Anime;
using CandySugar.App.Controls.Views.Novel;
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
            XSCandy = ContainerLocator.Container.Resolve<IXSLiShi>();
            DMCandy = ContainerLocator.Container.Resolve<IDMLiShi>();
        }

        #region Field
        private readonly IXSLiShi XSCandy;
        private readonly IDMLiShi DMCandy;
        #endregion

        #region Property
        private ObservableCollection<XSLiShiDto> _XSLiShi;
        public ObservableCollection<XSLiShiDto> XSLiShi
        {
            get { return _XSLiShi; }
            set { SetProperty(ref _XSLiShi, value); }
        }
        private ObservableCollection<DMLiShiDto> _DMLiShi;
        public ObservableCollection<DMLiShiDto> DMLiShi
        {
            get { return _DMLiShi; }
            set { SetProperty(ref _DMLiShi, value); }
        }
        #endregion

        #region Command
        public ICommand XSClickCommand => new DelegateCommand<XSLiShiDto>(async input =>
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("ChapterURL", input.ChapeterAddress);
            param.Add("BookName", input.BookName);
            await ContainerLocator.Container.Resolve<INavigationService>().NavigateAsync(new Uri(nameof(CandyNovelContentView), UriKind.Relative), param);
        });

        public ICommand XSDeleteCommand => new DelegateCommand<XSLiShiDto>(async input =>
        {
            if (await XSCandy.Remove(input.ToMapest<XS_LiShi>()))
            {
                OnViewLaunch();
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("已从书架中移除"))
                {
                    await Task.Delay(3000);
                }
            }
        });

        public ICommand DMClickCommand => new DelegateCommand<DMLiShiDto>(async input => {

            NavigationParameters param = new NavigationParameters();
            param.Add("WatchAddress", input.PlayURL);
            await ContainerLocator.Container.Resolve<INavigationService>().NavigateAsync(new Uri(nameof(CandyAnimePlayView), UriKind.Relative), param);
        });

        public ICommand DMDeleteCommand => new DelegateCommand<DMLiShiDto>(async input =>
        {
            if (await DMCandy.Remove(input.ToMapest<DM_LiShi>()))
            {
                OnViewLaunch();
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("已从列表中移除"))
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
            DMLiShi = new ObservableCollection<DMLiShiDto>((await ContainerLocator.Container.Resolve<IDMLiShi>().Query()).ToMapest <List<DMLiShiDto>>());
        }
        #endregion
    }
}
