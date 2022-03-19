using CandySugar.Common;
using CandySugar.Properties;
using GalActor.SDK;
using GalActor.SDK.ViewModel;
using GalActor.SDK.ViewModel.Eunms;
using GalActor.SDK.ViewModel.Request;
using GalActor.SDK.ViewModel.Response;
using HandyControl.Controls;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.UserControlViews.AxgleViews
{
    public class AxgleViewModel: Screen
    {
        private readonly IContainer Container;
        private readonly GalActorProxy Proxy;
        public AxgleViewModel(IContainer Container)
        {
            this.Container = Container;
            Proxy = new GalActorProxy
            {
                IP = Soft.Default.ProxyIP,
                Port = Soft.Default.ProxyPort,
                PassWord = Soft.Default.ProxyPwd,
                UserName = Soft.Default.ProxyAccount
            };
        }

        #region Property
        private ObservableCollection<GalActorCategory> _Categories;
        public ObservableCollection<GalActorCategory> Categories
        {
            get { return _Categories; }
            set { SetAndNotify(ref _Categories, value); }
        }
        #endregion


        protected override async void OnViewLoaded()
        {
            try
            {
                HelpUtilty.WirteLog("初始茶杯操作");
                var Init = await GalActorFactory.GalActor(opt =>
                {
                    opt.RequestParam = new GalActorRequestInput
                    {
                        Galype = GalActorEnum.Init,
                        Proxy = new GalActorProxy(),
                        GalInit = new GalActorInit(),
                        CacheSpan = Soft.Default.CacheTime
                    };
                }).RunsAsync();

                Categories = new ObservableCollection<GalActorCategory>(Init.CategoryResults);
            }
            catch(Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
            }
         
        }
    }
}
