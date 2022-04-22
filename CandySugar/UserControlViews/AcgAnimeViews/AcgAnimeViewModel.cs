using CandySugar.CandyWindows.CandyWinViewModel;
using CandySugar.Common;
using CandySugar.Common.WinDTO;
using CandySugar.Properties;
using HandyControl.Controls;
using SDKColloction.AcgAnimeSDK;
using SDKColloction.AcgAnimeSDK.ViewModel;
using SDKColloction.AcgAnimeSDK.ViewModel.Enums;
using SDKColloction.AcgAnimeSDK.ViewModel.Request;
using SDKColloction.AcgAnimeSDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CandySugar.UserControlViews.AcgAnimeViews
{
    public class AcgAnimeViewModel : Screen
    {
        private readonly IContainer Container;
        private readonly AcgAnimeProxy Proxy;
        public AcgAnimeViewModel(IContainer Container)
        {
            this.Container = Container;
            Proxy = new AcgAnimeProxy
            {
                IP = Soft.Default.ProxyIP,
                Port = Soft.Default.ProxyPort,
                PassWord = Soft.Default.ProxyPwd,
                UserName = Soft.Default.ProxyAccount
            };
        }

        #region Property
        private ObservableCollection<AcgAnimeInitResult> _InitResult;
        public ObservableCollection<AcgAnimeInitResult> InitResult
        {
            get => _InitResult;
            set => SetAndNotify(ref _InitResult, value);
        }
        private ObservableCollection<AcgAnimeTagsResult> _TagResult;
        public ObservableCollection<AcgAnimeTagsResult> TagResult
        {
            get => _TagResult;
            set => SetAndNotify(ref _TagResult, value);
        }
        private AcgAnimeBrandsResult _BrandResult;
        public AcgAnimeBrandsResult BrandResult
        {
            get => _BrandResult;
            set => SetAndNotify(ref _BrandResult, value);
        }
        public ObservableCollection<string> _HType;
        public ObservableCollection<string> HType
        {
            get => _HType;
            set => SetAndNotify(ref _HType, value);
        }
        #endregion

        #region Override
        protected override void OnViewLoaded()
        {
            Init();
        }
        #endregion

        #region Command
        public void SetFilter()
        {
            CandyAcgProviderViewModel ViewModel = Container.Get<CandyAcgProviderViewModel>();
            ViewModel.HType = HType;
            ViewModel.TagResult = TagResult;
            ViewModel.BrandResult = BrandResult;
            ViewModel.Loading = Visibility.Hidden;
            BootResource.AcgView(window =>
            {
                window.DataContext = ViewModel;
            });
        }
        public void ResetFilter()
        {
            HAcgOption.Brand = null;
            HAcgOption.Tags = null;
            HAcgOption.Type = null;
        }
        public void Redirect(string input) {
            Category(input);
        }
        #endregion

        #region Method
        public async void Init()
        {
            try
            {
                HelpUtilty.WirteLog("初始化ACG动漫操作");
                var AcgInit = await AcgAnimeFactory.AcgAnime(opt =>
                {
                    opt.RequestParam = new AcgAnimeRequestInput
                    {
                        AcgType = AcgAnimeEnum.Init,
                        CacheSpan = Soft.Default.CacheTime,
                        Proxy = this.Proxy,
                    };
                }).RunsAsync();

                HType = new ObservableCollection<string>(AcgInit.TypeResult);
                BrandResult = AcgInit.BrandResults;
                TagResult = new ObservableCollection<AcgAnimeTagsResult>(AcgInit.TagResults);
                InitResult = new ObservableCollection<AcgAnimeInitResult>(AcgInit.InitResults);
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }
        }
        public async void Category(string input)
        {
            try
            {
                HelpUtilty.WirteLog("ACG动漫分类操作");
                var AcgCate = await AcgAnimeFactory.AcgAnime(opt =>
                {
                    opt.RequestParam = new AcgAnimeRequestInput
                    {
                        AcgType = AcgAnimeEnum.Category,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.Default.CacheTime,
                        Category = new AcgAnimeCategory
                        {
                            Route = input
                        }
                    };
                }).RunsAsync();

            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                Growl.Info("网络有波动，请稍后再试~`(*>﹏<*)′");
            }
        }
        #endregion
    }
}
