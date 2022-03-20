using CandySugar.CandyWindows.CnadyWinViewModel;
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
using System.Windows.Controls;
using XExten.Advance.LinqFramework;

namespace CandySugar.UserControlViews.AxgleViews
{
    public class AxgleViewModel : Screen
    {
        private readonly IContainer Container;
        private readonly GalActorProxy Proxy;
      
        public AxgleViewModel(IContainer Container)
        {
            this.Container = Container;
            this.PageIndex = 1;
            Proxy = new GalActorProxy
            {
                IP = Soft.Default.ProxyIP,
                Port = Soft.Default.ProxyPort,
                PassWord = Soft.Default.ProxyPwd,
                UserName = Soft.Default.ProxyAccount
            };
            Desc = GalActorDescEnum.Latest;
        }

        #region Field
        private GalActorDescEnum Desc;
        private string KeyWord;
        private string SearckWord;
        #endregion

        #region Property
        private ObservableCollection<GalActorCategory> _Categories;
        public ObservableCollection<GalActorCategory> Categories
        {
            get { return _Categories; }
            set { SetAndNotify(ref _Categories, value); }
        }
        private ObservableCollection<CalActorCategoryList> _CategoryList;
        public ObservableCollection<CalActorCategoryList> CategoryList
        {
            get { return _CategoryList; }
            set { SetAndNotify(ref _CategoryList, value); }
        }
        private int _Total;
        public int Total
        {
            get { return _Total; }
            set { SetAndNotify(ref _Total, value); }
        }
        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { SetAndNotify(ref _PageIndex, value); }
        }
        #endregion

        #region Command
        public void CategoryCommand(string input)
        {
            KeyWord = input;
            Category();
        }
        public void DescCommand(ComboBoxItem input) 
        {
            Desc = (GalActorDescEnum)input.TabIndex;
            this.PageIndex = 1;
            Category();
        }

        public void PreviewCommand(string input)
        {
            var vm = Container.Get<CandyAxViewModel>();
            vm.Watch = input;
            vm.Loading = System.Windows.Visibility.Hidden;
            BootResource.AxgleWEB(window =>
            {
                window.DataContext = vm;
            });
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
                        Proxy = this.Proxy,
                        GalInit = new GalActorInit(),
                        CacheSpan = Soft.Default.CacheTime
                    };
                }).RunsAsync();

                Categories = new ObservableCollection<GalActorCategory>(Init.CategoryResults);
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
            }

        }
        private async void Category() {
            try
            {
                HelpUtilty.WirteLog("茶杯分类操作");
                var Cate = await GalActorFactory.GalActor(opt =>
                {
                    opt.RequestParam = new GalActorRequestInput
                    {
                        Galype = GalActorEnum.Category,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.Default.CacheTime,
                        Category = new CalActorCategory
                        {
                            CId = KeyWord.AsInt(),
                            Desc = Desc,
                            Page=this.PageIndex,
                        }
                    };
                }).RunsAsync();
                this.Total = Cate.CategoryListsResults.Total;
                this.CategoryList = new ObservableCollection<CalActorCategoryList>(Cate.CategoryListsResults.CategaryList);
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
            }
        }
    }
}
