﻿using CandySugar.CandyWindows.CnadyWinViewModel;
using CandySugar.Properties;
using HandyControl.Controls;
using HandyControl.Data;
using Novel.SDK;
using Novel.SDK.ViewModel;
using Novel.SDK.ViewModel.Enums;
using Novel.SDK.ViewModel.Request;
using Novel.SDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.StaticFramework;
using SDKRequest = Novel.SDK.ViewModel.Request;

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
                var NovelDetail = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.Default.CacheTime,
                        NovelType = NovelEnum.Detail,
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
            catch
            {
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
            }
        }

        public async void ShowContent(string args)
        {
            if (string.IsNullOrEmpty(args))
                return;
            try
            {
                var NovelContent = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.Default.CacheTime,
                        NovelType = NovelEnum.Watch,
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
            }
            catch
            {
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
            }

            //LoteNovelHistoryDTO DTO = NovelContent.Contents.ToMapest<LoteNovelHistoryDTO>();
            //DTO.BookName = this.NovelDetail.BookName;
            //container.Get<IHistoryService>().AddNovelHistory(DTO);
        }
        #endregion
    }
}