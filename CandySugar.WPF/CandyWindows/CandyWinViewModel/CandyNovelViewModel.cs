using CandySugar.Common;
using CandySugar.Common.DTO;
using CandySugar.Controls.Commands;
using CandySugar.Core.Service;
using CandySugar.WPF.Properties;
using HandyControl.Controls;
using SDKColloction.NovelSDK;
using SDKColloction.NovelSDK.ViewModel;
using SDKColloction.NovelSDK.ViewModel.Enums;
using SDKColloction.NovelSDK.ViewModel.Request;
using SDKColloction.NovelSDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Windows;
using System.Windows.Input;
using XExten.Advance.LinqFramework;

namespace CandySugar.WPF.CandyWindows.CandyWinViewModel
{
    public class CandyNovelViewModel : Screen
    {
        private readonly IContainer Container;
        private readonly NovelProxy Proxy;
        public CandyNovelViewModel(IContainer Container)
        {
            this.Container = Container;
            this.Proxy = new NovelProxy
            {
                IP = Soft.Default.ProxyIP,
                Port = Soft.Default.ProxyPort,
                PassWord = Soft.Default.ProxyPwd,
                UserName = Soft.Default.ProxyAccount
            };
            this.FontSize = 22;
        }

        #region Property
        private NovelContentResult _NovelContent;
        public NovelContentResult NovelContent
        {
            get { return _NovelContent; }
            set { SetAndNotify(ref _NovelContent, value); }
        }
        private int _FontSize;
        public int FontSize
        {
            get { return _FontSize; }
            set { SetAndNotify(ref _FontSize, value); }
        }
        private string _BookName;
        public string BookName
        {
            get { return _BookName; }
            set { SetAndNotify(ref _BookName, value); }
        }
        private Visibility _Loading;
        public Visibility Loading
        {
            get { return _Loading; }
            set { SetAndNotify(ref _Loading, value); }
        }
        #endregion

        #region Method
        public ICommand SliderChange => new CandyCommand(input =>
        {
            FontSize = (int)input;
        }, null);

        public ICommand ShowContent => new CandyCommand(async input =>
        {
            if (string.IsNullOrEmpty(input.ToString()))
                return;

           try
            {
                HelpUtilty.WirteLog("显示小说内容操作");

                var NovelContent = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.Default.CacheTime,
                        NovelType = NovelEnum.NovelWatch,
                        Proxy = this.Proxy,
                        View = new NovelView
                        {
                            NovelViewAddress = input.ToString()
                        }
                    };
                }).RunsAsync();
                this.NovelContent = NovelContent.Contents;

                CandyNovelHistoryDto DTO = NovelContent.Contents.ToMapest<CandyNovelHistoryDto>();
                DTO.BookName = this.BookName;
                Container.Get<ILiShi>().AddNovelHistory(DTO);
            }
            catch(Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }

        }, null);
        #endregion
    }
}
