using CandySugar.Properties;
using CandySugar.ViewModels;
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace CandySugar.UserControlViews.NovelViews
{
    public class NovelViewModel : Screen
    {
        private readonly IContainer Container;
        private readonly NovelProxy Proxy;
        public NovelViewModel(IContainer Container)
        {
            this.Container = Container;
            Proxy = new NovelProxy
            {
                IP = Soft.Default.ProxyIP,
                Port = Soft.Default.ProxyPort,
                PassWord = Soft.Default.ProxyPwd,
                UserName = Soft.Default.ProxyAccount
            };
            this.Themes = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Soft.Default.Theme));
        }

        #region Property
        private ObservableCollection<NovelRecommendResult> _NovelRecommend;
        public ObservableCollection<NovelRecommendResult> NovelRecommend
        {
            get { return _NovelRecommend; }
            set { SetAndNotify(ref _NovelRecommend, value); }
        }

        private ObservableCollection<NovelCategoryResult> _NovelCategory;
        public ObservableCollection<NovelCategoryResult> NovelCategory
        {
            get { return _NovelCategory; }
            set { SetAndNotify(ref _NovelCategory, value); }
        }

        private ObservableCollection<NovelSearchResult> _NovelSearch;
        public ObservableCollection<NovelSearchResult> NovelSearch
        {
            get { return _NovelSearch; }
            set { SetAndNotify(ref _NovelSearch, value); }
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

        private SolidColorBrush _Themes;
        public SolidColorBrush Themes
        {
            get { return _Themes; }
            set { SetAndNotify(ref _Themes, value); }
        }
        #endregion

        #region Field
        private string DetailAddress;
        private int Page;
        #endregion

        #region Method

        protected override void OnViewLoaded()
        {
            SyncStatic.TryCatch(async () =>
            {
                var NovelInit = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.Default.CacheTime,
                        NovelType = NovelEnum.Init,
                        Proxy = this.Proxy
                    };
                }).RunsAsync();
                this.NovelCategory = new ObservableCollection<NovelCategoryResult>(NovelInit.IndexCategories);
                this.NovelRecommend = new ObservableCollection<NovelRecommendResult>(NovelInit.IndexRecommends);
            }, ex =>
             {
                 MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                 return null;
             });
        }

        public void SearchBook(string args)
        {
            SyncStatic.TryCatch(async () =>
            {
                var NovelSearch = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan =Soft.Default.CacheTime,
                        NovelType = NovelEnum.Search,
                        Proxy = this.Proxy,
                        Search = new NovelSearch
                        {
                            NovelSearchKeyWord = args
                        }
                    };
                }).RunsAsync();
                this.NovelSearch = new ObservableCollection<NovelSearchResult>(NovelSearch.SearchResults);
            }, ex =>
            {
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                return null;
            });

            this.Total = 0;
            this.Page = 1;
        }

        public void Redirect(string args)
        {
            this.DetailAddress = args;
            this.PageIndex = Page == 0 ? 1 : Page;

            SyncStatic.TryCatch(async () =>
            {
                var NovelCate = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.Default.CacheTime,
                        NovelType = NovelEnum.Category,
                        Proxy = this.Proxy,
                        Category = new NovelCategory
                        {
                            Page = this.PageIndex,
                            NovelCategoryAddress = args
                        }
                    };
                }).RunsAsync();
                this.Total = NovelCate.SingleCategories.TotalPage;
                this.NovelSearch = new ObservableCollection<NovelSearchResult>(NovelCate.SingleCategories.NovelSingles.ToMapest<List<NovelSearchResult>>());
            }, ex =>
            {
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                return null;
            });
        }

        public void PageUpdated(FunctionEventArgs<int> args)
        {
            Page = args.Info;
            Redirect(this.DetailAddress);
        }

        public void GetBook(dynamic entity)
        {
            SyncStatic.TryCatch(async () =>
            {
                var NovelDetail = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.Default.CacheTime,
                        NovelType = NovelEnum.Detail,
                        Proxy = this.Proxy,
                        Detail = new NovelDetail
                        {
                            NovelDetailAddress = entity.DetailAddress
                        }
                    };
                }).RunsAsync();
               
                var vm = Container.Get<NovelContentViewModel>();
                vm.NovelDetail = NovelDetail.Details;
                vm.PageIndex = 1;
                vm.Addr = entity.DetailAddress;
                Container.Get<RootViewModel>().ActivateItem(vm);
            }, ex =>
            {
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                return null;
            });
           
        }
        #endregion
    }
}
