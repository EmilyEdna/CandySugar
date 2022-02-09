﻿using CandySugar.CandyWindows.CnadyWinViewModel;
using CandySugar.Common;
using CandySugar.Properties;
using HandyControl.Controls;
using HandyControl.Data;
using LightNovel.SDK;
using LightNovel.SDK.ViewModel;
using LightNovel.SDK.ViewModel.Enums;
using LightNovel.SDK.ViewModel.Request;
using LightNovel.SDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace CandySugar.UserControlViews.LigthNovelViews
{
    public class LightNovelViewModel : Screen
    {
        public const string Account = "kilydoll365";
        public const string Password = "sion8550";
        private readonly IContainer Container;
        private readonly LightNovelProxy Proxy;
        public LightNovelViewModel(IContainer Container)
        {
            this.Container = Container;
            Proxy = new LightNovelProxy
            {
                IP = Soft.Default.ProxyIP,
                Port = Soft.Default.ProxyPort,
                PassWord = Soft.Default.ProxyPwd,
                UserName = Soft.Default.ProxyAccount
            };
        }

        #region Property
        private ObservableCollection<LightNovelCategoryResult> _LightNovelCategory;
        public ObservableCollection<LightNovelCategoryResult> LightNovelCategory
        {
            get { return _LightNovelCategory; }
            set { SetAndNotify(ref _LightNovelCategory, value); }
        }

        private ObservableCollection<LightNovelSingleCategoryResults> _LightNovelSingleCategory;
        public ObservableCollection<LightNovelSingleCategoryResults> LightNovelSingleCategory
        {
            get { return _LightNovelSingleCategory; }
            set { SetAndNotify(ref _LightNovelSingleCategory, value); }
        }

        private ObservableCollection<LightNovelViewResult> _LightNovelViews;
        public ObservableCollection<LightNovelViewResult> LightNovelViews
        {
            get { return _LightNovelViews; }
            set { SetAndNotify(ref _LightNovelViews, value); }
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

        #region Field
        private bool IsSearch = false;
        private string SearchWord;
        private string CategoryAddress;
        private LightNovelSearchEnum SearchType;
        private int Page;
        private string BookName;
        #endregion

        #region Method
        protected Dictionary<string, string> WkInfo()
        {
            if (!Soft.Default.DefaultNovel)
                return new Dictionary<string, string> { { Soft.Default.NovelAccount.IsNullOrEmpty() ? "-" : Soft.Default.NovelAccount, Soft.Default.NovelPwd.IsNullOrEmpty() ? "-" : Soft.Default.NovelPwd } };
            else
                return new Dictionary<string, string> { { Account , Password } };
        }
        protected override void OnViewLoaded()
        {
            SyncStatic.TryCatch(async () =>
            {
                //初始化
                var LightNovelInit = await LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        CacheSpan = Soft.Default.CacheTime,
                        LightNovelType = LightNovelEnum.Init,
                        Proxy = this.Proxy
                    };
                }).RunsAsync(Light =>
                {
                    Light.RefreshCookie(new LightNovelRefresh
                    {
                        UserName = WkInfo().Keys.FirstOrDefault(),
                        PassWord = WkInfo().Values.FirstOrDefault()
                    }, this.Proxy);
                });
                LightNovelCategory = new ObservableCollection<LightNovelCategoryResult>(LightNovelInit.CategoryResults);
                CategoryAddress = LightNovelInit.CategoryResults.FirstOrDefault().CategoryAddress;
                //分类
                var LightNovelCate = await LightNovelFactory.LightNovel(opt =>
                 {
                     opt.RequestParam = new LightNovelRequestInput
                     {
                         LightNovelType = LightNovelEnum.Category,
                         CacheSpan = Soft.Default.CacheTime,
                         Proxy = this.Proxy,
                         Category = new LightNovelCategory
                         {
                             CategoryAddress = CategoryAddress
                         }
                     };
                 }).RunsAsync(Light =>
                 {
                     Light.RefreshCookie(new LightNovelRefresh
                     {
                         UserName = WkInfo().Keys.FirstOrDefault(),
                         PassWord = WkInfo().Values.FirstOrDefault()
                     }, new LightNovelProxy());
                 });
                LightNovelSingleCategory = new ObservableCollection<LightNovelSingleCategoryResults>(LightNovelCate.SingleCategoryResult.Result);
                Total = LightNovelCate.SingleCategoryResult.TotalPage;

                PageIndex = 1;
                IsSearch = false;
            }, ex =>
            {
                LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.Refresh
                    };
                }).Runs();
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                return null;
            });
        }

        public void SearchBook(string args)
        {
            SearchWord = args;
            this.PageIndex = Page == 0 ? 1 : Page;
            SyncStatic.TryCatch(async () =>
            {
                //搜索
                var LightNovelSearch = await LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.Search,
                        CacheSpan = Soft.Default.CacheTime,
                        Proxy = this.Proxy,
                        Search = new LightNovelSearch
                        {
                            Page = this.PageIndex,
                            KeyWord = args,
                            SearchType = SearchType
                        }
                    };
                }).RunsAsync(Light =>
                {
                    Light.RefreshCookie(new LightNovelRefresh
                    {
                        UserName = WkInfo().Keys.FirstOrDefault(),
                        PassWord = WkInfo().Values.FirstOrDefault()
                    }, new LightNovelProxy());
                });

                if (LightNovelSearch.SearchResults.Result != null)
                {
                    LightNovelSingleCategory = new ObservableCollection<LightNovelSingleCategoryResults>(LightNovelSearch.SearchResults.Result.ToMapest<List<LightNovelSingleCategoryResults>>());
                    Total = LightNovelSearch.SearchResults.TotalPage;
                }
                IsSearch = true;
            }, ex =>
            {
                LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.Refresh
                    };
                }).Runs();
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                return null;
            });
        }

        public void Redirect(string args)
        {
            CategoryAddress = args;
            this.PageIndex = Page == 0 ? 1 : Page;
            SyncStatic.TryCatch(async () =>
            {
                var LightNovelCate = await LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.Category,
                        CacheSpan = Soft.Default.CacheTime,
                        Proxy = this.Proxy,
                        Category = new LightNovelCategory
                        {
                            Page = this.PageIndex,
                            CategoryAddress = args
                        }
                    };
                }).RunsAsync(Light =>
                {
                    Light.RefreshCookie(new LightNovelRefresh
                    {
                        UserName = WkInfo().Keys.FirstOrDefault(),
                        PassWord = WkInfo().Values.FirstOrDefault()
                    }, new LightNovelProxy());
                });
                LightNovelSingleCategory = new ObservableCollection<LightNovelSingleCategoryResults>(LightNovelCate.SingleCategoryResult.Result);
                Total = LightNovelCate.SingleCategoryResult.TotalPage;
                IsSearch = false;
            }, ex =>
            {
                LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.Refresh
                    };
                }).Runs();
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                return null;
            });
        }

        public void PageUpdated(FunctionEventArgs<int> args)
        {
            if (IsSearch)
            {
                Page = args.Info;
                SearchBook(SearchWord);
            }
            else
            {
                Page = args.Info;
                Redirect(CategoryAddress);
            }
        }

        public void GetBook(LightNovelSingleCategoryResults entity)
        {
            BookName = entity.BookName;

            SyncStatic.TryCatch(async () =>
            {
                var LightNovelDetail = await LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.Detail,
                        CacheSpan = Soft.Default.CacheTime,
                        Proxy = this.Proxy,
                        Detail = new LightNovelDetail
                        {
                            DetailAddress = entity.DetailAddress
                        }
                    };
                }).RunsAsync(Light =>
                {
                    Light.RefreshCookie(new LightNovelRefresh
                    {
                        UserName = WkInfo().Keys.FirstOrDefault(),
                        PassWord = WkInfo().Values.FirstOrDefault()
                    }, new LightNovelProxy());
                });

                var LightNovelView = await LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.View,
                        Proxy = new LightNovelProxy(),
                        CacheSpan = Soft.Default.CacheTime,
                        View = new LightNovel.SDK.ViewModel.Request.LightNovelView
                        {
                            ViewAddress = LightNovelDetail.DetailResult.Address,
                        }
                    };
                }).RunsAsync(Light =>
                {
                    Light.RefreshCookie(new LightNovelRefresh
                    {
                        UserName = WkInfo().Keys.FirstOrDefault(),
                        PassWord = WkInfo().Values.FirstOrDefault()
                    }, new LightNovelProxy());
                });

                LightNovelViews = new ObservableCollection<LightNovelViewResult>(LightNovelView.ViewResult);
            }, ex =>
            {
                LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.Refresh
                    };
                }).Runs();
                MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                return null;
            });

        }

        public void SetSearchType(ComboBoxItem control)
        {
            SearchType = (LightNovelSearchEnum)control.TabIndex;
        }

        public  void GetContent(LightNovelViewResult entity)
        {
            if (entity.IsDown)
            {

                SyncStatic.TryCatch(async () =>
                {
                    var LightNovelDown = await LightNovelFactory.LightNovel(opt =>
                    {
                        opt.RequestParam = new LightNovelRequestInput
                        {
                            LightNovelType = LightNovelEnum.Down,
                            Proxy = this.Proxy,
                            Down = new LightNovelDown
                            {
                                UId = entity.ChapterURL.AsInt(),
                                BookName = BookName
                            }
                        };
                    }).RunsAsync();
                    var dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "LoteDown", "LightNovel", $"{HelpUtilty.FileNameFilter(BookName)}"));
                    var fn = SyncStatic.CreateFile(Path.Combine(dir, $"{HelpUtilty.FileNameFilter(BookName)}.txt"));
                    SyncStatic.WriteFile(LightNovelDown.DownResult.Down, fn);
                    Process.Start("explorer.exe", dir);
                }, ex => {
                    MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                    return null;
                });
            }
            else
            {
                SyncStatic.TryCatch(async () =>
                {
                    //内容
                    var LightNovelContent =await LightNovelFactory.LightNovel(opt =>
                    {
                        opt.RequestParam = new LightNovelRequestInput
                        {
                            CacheSpan = Soft.Default.CacheTime,
                            LightNovelType = LightNovelEnum.Content,
                            Proxy = this.Proxy,
                            Content = new LightNovelContent
                            {
                                ChapterURL = entity.ChapterURL,
                            }
                        };
                    }).RunsAsync();

                    if (await DownNovel(entity.ChapterURL, LightNovelContent.ContentResult.Content) == false)
                        return;

                    var vm = Container.Get<CandyLightNovelViewModel>();
                    vm.LightNovelContent = LightNovelContent.ContentResult;
                    vm.Show = LightNovelContent.ContentResult.Image == null;
                    vm.Loading = System.Windows.Visibility.Hidden;
                    //Open
                    BootResource.LightNovel(window =>
                    {
                        window.DataContext = vm;
                    });

                }, ex => {
                    MessageBox.Info("网络有波动，请稍后再试~`(*>﹏<*)′", "提示");
                    return null;
                });
            }
        }

        private async Task<bool> DownNovel(string Url, string Check)
        {
            if (Check.Equals("因版权问题，文库不再提供该小说的阅读！"))
            {
                var result = MessageBox.Show("因版权问题，文库不再提供该小说的阅读！是否下载阅读？", "提示",
                      System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Information);

                var UId = Regex.Matches(Url, "[0-9]+/").LastOrDefault().Value.Split("/").FirstOrDefault().AsInt();

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    var LightNovelDown =await  LightNovelFactory.LightNovel(opt =>
                    {
                        opt.RequestParam = new LightNovelRequestInput
                        {
                            LightNovelType = LightNovelEnum.Down,
                            Proxy = this.Proxy,
                            Down = new LightNovelDown
                            {
                                UId = UId,
                                BookName = BookName
                            }
                        };
                    }).RunsAsync();
                    var dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "LoteDown", "LightNovel", $"{HelpUtilty.FileNameFilter(BookName)}"));
                    var fn = SyncStatic.CreateFile(Path.Combine(dir, $"{HelpUtilty.FileNameFilter(BookName)}.txt"));
                    SyncStatic.WriteFile(LightNovelDown.DownResult.Down, fn);
                    MessageBox.Info("下载完成", "提示");
                    Process.Start("explorer.exe", dir);
                    return false;
                }
                return false;
            }
            return true;
        }
        #endregion
    }
}
