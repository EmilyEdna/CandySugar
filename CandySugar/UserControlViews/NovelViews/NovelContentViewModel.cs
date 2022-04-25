using CandySugar.CandyWindows.CandyWinViewModel;
using CandySugar.Common;
using CandySugar.Common.DTO;
using CandySugar.Core.Service;
using CandySugar.Properties;
using HandyControl.Controls;
using HandyControl.Data;
using SDKColloction.NovelSDK;
using SDKColloction.NovelSDK.ViewModel;
using SDKColloction.NovelSDK.ViewModel.Enums;
using SDKColloction.NovelSDK.ViewModel.Request;
using SDKColloction.NovelSDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;
using SDKRequest = SDKColloction.NovelSDK.ViewModel.Request;

namespace CandySugar.UserControlViews.NovelViews
{
    public class NovelContentViewModel:Screen
    {
        private readonly IContainer Container;
        private readonly NovelProxy Proxy;
        public NovelContentViewModel(IContainer Container)
        {
            this.Container = Container;
            Proxy = new NovelProxy
            {
                IP = Soft.Default.ProxyIP,
                Port = Soft.Default.ProxyPort,
                PassWord = Soft.Default.ProxyPwd,
                UserName = Soft.Default.ProxyAccount
            };
        }

        #region Property
        private NovelDetailResult _NovelDetail;
        public NovelDetailResult NovelDetail
        {
            get { return _NovelDetail; }
            set { SetAndNotify(ref _NovelDetail, value); }
        }

        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { SetAndNotify(ref _PageIndex, value); }
        }

        private string _Addr;
        public string Addr
        {
            get { return _Addr; }
            set { SetAndNotify(ref _Addr, value); }
        }
        #endregion

        #region Method
        public async void PageUpdated(FunctionEventArgs<int> args)
        {
            try
            {
                PageIndex = args.Info;
                HelpUtilty.WirteLog("小说章节列表操作");
                var NovelDetail = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.Default.CacheTime,
                        NovelType = NovelEnum.NovelDetail,
                        Proxy = this.Proxy,
                        Detail = new NovelDetail
                        {
                            Page = PageIndex,
                            NovelDetailAddress = Addr
                        }
                    };
                }).RunsAsync();
                this.NovelDetail = NovelDetail.Details;
            }
            catch(Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }
        }

        public async void ShowContent(string args)
        {
            if (string.IsNullOrEmpty(args))
                return;
            try
            {
                HelpUtilty.WirteLog("小说内容操作");
                var NovelContent = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.Default.CacheTime,
                        NovelType = NovelEnum.NovelWatch,
                        Proxy = this.Proxy,
                        View = new SDKRequest.NovelView
                        {
                            NovelViewAddress = args
                        }
                    };
                }).RunsAsync();

                var vm = Container.Get<CandyNovelViewModel>();
                vm.NovelContent = NovelContent.Contents;
                vm.BookName = this.NovelDetail.BookName;
                vm.Loading = System.Windows.Visibility.Hidden;
                BootResource.Novel(window =>
                {
                    window.DataContext = vm;
                });
                CandyNovelHistoryDto DTO = NovelContent.Contents.ToMapest<CandyNovelHistoryDto>();
                DTO.BookName = this.NovelDetail.BookName;
                Container.Get<ILiShi>().AddNovelHistory(DTO);
            }
            catch(Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }

          
        }
        #endregion
    }
}
