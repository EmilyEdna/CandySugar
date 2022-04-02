using System;
using System.Collections.Generic;
using System.Text;
using CandySugar.Xam.Core.Service;
using Prism.Ioc;

namespace CandySugar.App.Controls.LayoutView.LayoutViewModel
{
    public class PopHeaderViewModel : ViewModelBase
    {

        #region Property
        private int _Count;
        public int Count
        {
            get => _Count;
            set => SetProperty(ref _Count, value);
        }
        #endregion

        protected override async void OnViewLaunchAsync()
        {
            Count = await ContainerLocator.Container.Resolve<IYYLiShi>().PlayCount();
        }
    }
}
