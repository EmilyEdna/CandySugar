using CandySugar.CandyWindows.CandyWinViewModel;
using CandySugar.Common;
using CandySugar.Common.DTO;
using CandySugar.Core.Service;
using CandySugar.Properties;
using HandyControl.Controls;
using Manga.SDK;
using Manga.SDK.ViewModel;
using Manga.SDK.ViewModel.Emums;
using Manga.SDK.ViewModel.Request;
using Manga.SDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace CandySugar.UserControlViews.MangaViews
{
    public class MangaViewModel : Screen
    {
        private readonly IContainer Container;
        private readonly MangaProxy Proxy;
        public MangaViewModel(IContainer Container)
        {
            this.Container = Container;
            Proxy = new MangaProxy
            {
                IP = Soft.Default.ProxyIP,
                Port = Soft.Default.ProxyPort,
                PassWord = Soft.Default.ProxyPwd,
                UserName = Soft.Default.ProxyAccount
            };
        }

        #region Property
        private ObservableCollection<MangaRecommendResult> _MangaRecommend;
        public ObservableCollection<MangaRecommendResult> MangaRecommend
        {
            get { return _MangaRecommend; }
            set { SetAndNotify(ref _MangaRecommend, value); }
        }

        private ObservableCollection<MangaCategoryResult> _MangaCategory;
        public ObservableCollection<MangaCategoryResult> MangaCategory
        {
            get { return _MangaCategory; }
            set { SetAndNotify(ref _MangaCategory, value); }
        }

        private ObservableCollection<MangaChapterResult> _Chapters;
        public ObservableCollection<MangaChapterResult> Chapters
        {
            get { return _Chapters; }
            set { SetAndNotify(ref _Chapters, value); }
        }

        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { SetAndNotify(ref _PageIndex, value); }
        }
        #endregion

        #region Field
        public int Type { get; set; }
        private string Keyword = string.Empty;
        #endregion

        #region Action
        public void Search(string input)
        {
            this.Type = 1;
            this.PageIndex = 1;
            Keyword = input;
            Handle();
            Chapters = new ObservableCollection<MangaChapterResult>();
        }

        public void Redirect(string input)
        {
            this.Type = 2;
            this.PageIndex = 1;
            Keyword = input;
            Handle();
            Chapters = new ObservableCollection<MangaChapterResult>();
        }
        public void MangaDetailSort(string input)
        {
            if (input == null)
                return;
            DetailList(input);
        }

        public void MangaDetail(MangaRecommendResult input)
        {
            if (input == null)
                return;
            DetailList(input.Address);
        }

        public void MangaContent(MangaChapterResult input)
        {
            if (Chapters.Count != 0)
            {
                if (Chapters.FirstOrDefault(t => t.TagKey == input.TagKey) != null)
                {
                    CandyMangaReaderViewModel vm = Container.Get<CandyMangaReaderViewModel>();
                    vm.Chapters = Chapters;
                    vm.Total = Chapters.Count;
                    vm.Index = Chapters.IndexOf(input);
                    vm.Loading = System.Windows.Visibility.Visible;
                    vm.InitCurrent();

                    BootResource.Manga(window =>
                    {
                        window.DataContext = vm;
                    });

                    CandyMangaHistoryDto DTO = input.ToMapest<CandyMangaHistoryDto>();
                    DTO.Chapters = Chapters.ToJson();
                    DTO.Index = Chapters.IndexOf(input);
                    Container.Get<ILiShi>().AddMangaHistory(DTO);
                }
            }
        }

        public void ScrollChanged(Dictionary<string, int> input)
        {
            if (Type == 0 || Keyword.IsNullOrEmpty())
                return;
            PageIndex += input.Values.FirstOrDefault();
            if (PageIndex < 0) return;
            Handle();
        }
        #endregion

        #region Internal
        protected async void Handle()
        {
            if (Type == 1)
            {
                try
                {
                    HelpUtilty.WirteLog("漫画查询操作");
                    var MangaSearch = await MangaFactory.Manga(opt =>
                    {
                        opt.RequestParam = new MangaRequestInput
                        {
                            MangaType = MangaEnum.Search,
                            Proxy = this.Proxy,
                            CacheSpan = Soft.Default.CacheTime,
                            Search = new MangaSearch
                            {
                                KeyWord = Keyword,
                                Page = PageIndex
                            }
                        };
                    }).RunsAsync();

                    if (MangaSearch.SearchResults.Count == 0)
                        MessageBox.Info("数据已到底~`(*>﹏<*)′", "提示");
                    else
                        MangaRecommend = new ObservableCollection<MangaRecommendResult>(MangaSearch.SearchResults.ToMapest<List<MangaRecommendResult>>());
                }
                catch(Exception ex)
                {
                    HelpUtilty.WirteLog(string.Empty, ex);
                    MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                }
            }
            else
            {
                try
                {
                    HelpUtilty.WirteLog("漫画分类操作");
                    //分类
                    var MangaCate = await MangaFactory.Manga(opt =>
                    {
                        opt.RequestParam = new MangaRequestInput
                        {
                            MangaType = MangaEnum.Category,
                            Proxy = this.Proxy,
                            CacheSpan = Soft.Default.CacheTime,
                            Category = new MangaCategory
                            {
                                Address = Keyword,
                                Page = PageIndex
                            }
                        };
                    }).RunsAsync();
                    if (MangaCate.SearchResults.Count == 0)
                        MessageBox.Info("数据已到底~`(*>﹏<*)′", "提示");
                    else
                        MangaRecommend = new ObservableCollection<MangaRecommendResult>(MangaCate.SearchResults.ToMapest<List<MangaRecommendResult>>());
                }
                catch (Exception ex)
                {
                    HelpUtilty.WirteLog(string.Empty, ex);
                    MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                }
            }
        }
        protected async override void OnViewLoaded()
        {

            try
            {
                PageIndex = 1;
                HelpUtilty.WirteLog("漫画初始化操作");
                //初始化
                var MangaInit = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new MangaRequestInput
                    {
                        MangaType = MangaEnum.Init,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.Default.CacheTime,
                    };
                }).RunsAsync();

                MangaRecommend = new ObservableCollection<MangaRecommendResult>(MangaInit.IndexRecommends);
                MangaCategory = new ObservableCollection<MangaCategoryResult>(MangaInit.IndexCategories);
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
            }
        }
        private async void DetailList(string input)
        {
            try
            {
                HelpUtilty.WirteLog("漫画详情操作");
                //详情
                var MangaDetail = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new MangaRequestInput
                    {
                        MangaType = MangaEnum.Detail,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.Default.CacheTime,
                        Detail = new MangaDetail
                        {
                            Address = input
                        }
                    };
                }).RunsAsync();
                Chapters = new ObservableCollection<MangaChapterResult>(MangaDetail.ChapterResults);
            }
            catch (Exception ex)
            {
                HelpUtilty.WirteLog(string.Empty, ex);
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
            }
        }
        #endregion
    }
}
