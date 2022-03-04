using CandySugar.Xam.Common;
using Novel.SDK.ViewModel.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.App.Controls.ViewModels
{
    public class NovelViewModel: ViewModelBase
    {
        private readonly NovelProxy Proxy;
        public NovelViewModel()
        {
            Proxy = new NovelProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
        }
    }
}
