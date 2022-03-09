using CandySugar.Xam.Common;
using Novel.SDK.ViewModel.Request;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.App.Controls.ViewModels.NovelModel
{
    public class CandyNodelContentViewModel : ViewModelNavigatBase
    {
        private readonly NovelProxy Proxy;
        public CandyNodelContentViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = new NovelProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
        }

        #region Overrive

        public override void Initialize(INavigationParameters parameters)
        {
            parameters.GetValue<string>("Url");
        }
        #endregion
    }
}
