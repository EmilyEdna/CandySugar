using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CandySugar.Xam.Core.Service;
using Prism.Ioc;

namespace CandySugar.App.Controls.LayoutView.LayoutViewModel
{
    public class PopPlayHeaderViewModel : ViewModelBase
    {

        #region Property
        private int _Count;
        public int Count
        {
            get => _Count;
            set => SetProperty(ref _Count, value);
        }
        #endregion

        protected override void OnViewLaunch()
        {
            Refresh();
        }

        public async void Refresh()
        {
            Count = await ContainerLocator.Container.Resolve<IYYLiShi>().PlayCount();
        }
    }
}
