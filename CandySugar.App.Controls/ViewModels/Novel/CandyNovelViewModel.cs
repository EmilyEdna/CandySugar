using CandySugar.Xam.Common;
using Novel.SDK;
using Novel.SDK.ViewModel;
using Novel.SDK.ViewModel.Enums;
using Novel.SDK.ViewModel.Request;
using Novel.SDK.ViewModel.Response;
using Prism.Commands;
using Syncfusion.XForms.TabView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XF.Material.Forms.UI.Dialogs;

namespace CandySugar.App.Controls.ViewModels.Novel
{
    public class CandyNovelViewModel : ViewModelBase
    {
        private readonly NovelProxy Proxy;
        public CandyNovelViewModel() : base()
        {
            Proxy = new NovelProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
        }

        

        #region Property
        private ObservableCollection<NovelCategoryResult> _NovelCategory;
        public ObservableCollection<NovelCategoryResult> NovelCategory
        {
            get { return _NovelCategory; }
            set { SetProperty(ref _NovelCategory, value); }
        }
        #endregion

        #region Command
        public ICommand ItemCommand => new DelegateCommand<string>(obj =>
        {
            Title = obj;
        });
        #endregion

        protected override async void OnViewLaunch()
        {
            try
            {
                var NovelInit = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.CacheTime,
                        NovelType = NovelEnum.Init,
                        Proxy = this.Proxy
                    };
                }).RunsAsync();
                this.NovelCategory = new ObservableCollection<NovelCategoryResult>(NovelInit.IndexCategories);
                //this.NovelRecommend = new ObservableCollection<NovelRecommendResult>(NovelInit.IndexRecommends);
            }
            catch
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("网络有波动，请稍后再试~`(*>﹏<*)′"))
                {
                    await Task.Delay(3000);
                }
            }
        }
    }
}
