using CandySugar.CandyWindows.CandyWinViewModel;
using CandySugar.Common;
using CandySugar.Common.DTO;
using CandySugar.Core.Service;
using CandySugar.Properties;
using GalActor.SDK;
using GalActor.SDK.ViewModel;
using GalActor.SDK.ViewModel.Eunms;
using GalActor.SDK.ViewModel.Request;
using GalActor.SDK.ViewModel.Response;
using HandyControl.Controls;
using HandyControl.Data;
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
        private readonly IAx Ax;

        public AxgleViewModel(IContainer Container)
        {
            this.Container = Container;
            this.Ax = Container.Get<IAx>();
            this.PageIndex = 1;
            this.WatchFavorite = false;
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
        private bool _WatchFavorite;
        public bool WatchFavorite
        {
            get { return _WatchFavorite; }
            set { SetAndNotify(ref _WatchFavorite, value); }
        }
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
            SearckWord = String.Empty;
            this.PageIndex = 1;
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
        public void Favorite(CalActorCategoryList input)
        {
            Ax.RemoveFavorite(input.VId);
            if (SearckWord.IsNullOrEmpty() && !KeyWord.IsNullOrEmpty())
                Category();
            else
                Search();
        }
        public void NoFavorite(CalActorCategoryList input)
        {
            Ax.AddFavorite(input.ToMapest<CandyAxFavoriteDto>());
            if (SearckWord.IsNullOrEmpty() && !KeyWord.IsNullOrEmpty())
                Category();
            else
                Search();
        }
        public void Check()
        {
            WatchFavorite = true;
            InitFavorite(string.Empty);
        }
        public void UnCheck()
        {
            WatchFavorite = false;
            PageIndex = 1;
            Category();
        }
        public void PageUpdated(FunctionEventArgs<int> args)
        {
            this.PageIndex = args.Info;
            if (!SearckWord.IsNullOrEmpty() && WatchFavorite)
                InitFavorite(SearckWord);
            else
            {
                if (SearckWord.IsNullOrEmpty() && !KeyWord.IsNullOrEmpty())
                    Category();
                else
                    Search();
            }
        }
        public void SearchAxgle(string input)
        {
            SearckWord = input;
            KeyWord = String.Empty;
            this.PageIndex = 1;
            if (WatchFavorite)
                InitFavorite(SearckWord);
            else
                Search();
        }
        #endregion
        protected async void InitFavorite(string input)
        {
            var data = await Ax.GetFavorite(input, PageIndex);
            this.Total = data.Total;
            this.CategoryList = new ObservableCollection<CalActorCategoryList>(data.Result.ToMapest<List<CalActorCategoryList>>());
        }
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
        private async void Category()
        {
            try
            {
                if (KeyWord.IsNullOrEmpty()) return;
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
                            Page = this.PageIndex,
                        }
                    };
                }).RunsAsync();

                var favoriteId = await Ax.GetAllFavorite();
                Cate.CategoryListsResults.CategaryList.ForEach(t =>
                {
                    if (favoriteId.Contains(t.VId)) t.IsFavorite = true;
                });

                this.Total = Cate.CategoryListsResults.Total;
                this.CategoryList = new ObservableCollection<CalActorCategoryList>(Cate.CategoryListsResults.CategaryList);
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
            }
        }
        private async void Search()
        {
            try
            {
                if (SearckWord.IsNullOrEmpty()) return;
                HelpUtilty.WirteLog("茶杯查询操作");
                var Seach = await GalActorFactory.GalActor(opt =>
                {
                    opt.RequestParam = new GalActorRequestInput
                    {
                        Galype = GalActorEnum.Search,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.Default.CacheTime,
                        Search = new GalActorSearch
                        {
                            Page = this.PageIndex,
                            KeyWord = SearckWord
                        }
                    };
                }).RunsAsync();

                var favoriteId = await Ax.GetAllFavorite();
                Seach.SearchResult.SearchList.ForEach(t =>
                {
                    if (favoriteId.Contains(t.VId)) t.IsFavorite = true;
                });

                this.Total = Seach.SearchResult.Total;
                this.CategoryList = new ObservableCollection<CalActorCategoryList>(Seach.SearchResult.SearchList.ToMapest<List<CalActorCategoryList>>());
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
            }
        }
    }
}
