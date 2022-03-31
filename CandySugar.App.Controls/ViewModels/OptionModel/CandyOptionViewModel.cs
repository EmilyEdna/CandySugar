using CandySugar.Xam.Core.Service;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Prism.Ioc;
using CandySugar.Xam.Common.DTO;
using System.Windows.Input;
using Prism.Commands;
using CandySugar.Xam.Common;

namespace CandySugar.App.Controls.ViewModels.OptionModel
{
    public class CandyOptionViewModel : ViewModelNavigatBase
    {
        private ISetting Candy;
        public CandyOptionViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        #region Property
        private CandySettingDto _Option;
        public CandySettingDto Option
        {
            get => _Option;
            set => SetProperty(ref _Option, value);
        }
        #endregion

        #region Method
        public void Save(CandySettingDto input)
        {
            Candy.Save(input);

            Soft.AgeModule = input.AgeModule;
            Soft.CacheTime = input.CacheTime;
            Soft.WaitSpan = input.WaitSpan;
            Soft.Blur=input.Blur;
            Soft.ProxyAccount = input.ProxyAccount;
            Soft.ProxyIP = input.ProxyIP;
            Soft.ProxyPort = input.ProxyPort;
            Soft.ProxyPwd = input.ProxyPwd;
        }
        #endregion

        #region Override
        protected override async void OnViewLaunch()
        {
            Candy = ContainerLocator.Container.Resolve<ISetting>();
            Option = await Candy.Query();
        }
        #endregion
    }
}
