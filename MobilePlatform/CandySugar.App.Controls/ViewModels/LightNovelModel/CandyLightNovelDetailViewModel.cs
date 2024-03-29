﻿using CandySugar.Xam.Common;
using CandySugar.Xam.Common.Platform;
using SDKColloction.LightNovelSDK;
using SDKColloction.LightNovelSDK.ViewModel;
using SDKColloction.LightNovelSDK.ViewModel.Enums;
using SDKColloction.LightNovelSDK.ViewModel.Request;
using SDKColloction.LightNovelSDK.ViewModel.Response;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XExten.Advance.LinqFramework;
using XF.Material.Forms.UI.Dialogs;
using Prism.Ioc;
using XExten.Advance.StaticFramework;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using XExten.Advance.InternalFramework.Securities.Common;
using CandySugar.App.Controls.Views.LightNovel;
using CandySugar.Xam.Core.Service;
using CandySugar.Xam.Common.DTO;

namespace CandySugar.App.Controls.ViewModels.LightNovelModel
{
    public class CandyLightNovelDetailViewModel : ViewModelNavigatBase
    {
        public const string Account = "kilydoll365";
        public const string Password = "sion8550";
        private readonly ILXSLiShi Candy;
        private readonly ILoger CandyLog;
        public CandyLightNovelDetailViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = new LightNovelProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
            Candy = Resolve<ILXSLiShi>();
            CandyLog = Resolve<ILoger>();
        }
        #region Field
        private readonly LightNovelProxy Proxy;
        private string Cover;
        #endregion

        #region Property
        private string _DetailAddress;
        public string DetailAddress
        {
            get => _DetailAddress;
            set => SetProperty(ref _DetailAddress, value);
        }

        private string _BookName;
        public string BookName
        {
            get => _BookName;
            set => SetProperty(ref _BookName, value);
        }

        private ObservableCollection<LightNovelViewResult> _LightNovelViews;
        public ObservableCollection<LightNovelViewResult> LightNovelViews
        {
            get => _LightNovelViews;
            set => SetProperty(ref _LightNovelViews, value);
        }

        private bool _Refresh;
        public bool Refresh
        {
            get { return _Refresh; }
            set { SetProperty(ref _Refresh, value); }
        }
        #endregion

        #region Override
        public override void Initialize(INavigationParameters parameters)
        {
            this.DetailAddress = parameters.GetValue<string>("Route");
            this.BookName = parameters.GetValue<string>("BookName");
            this.Cover = parameters.GetValue<string>("Cover");
            Book();
        }
        #endregion

        #region Method
        private async Task<string> Tip(string Method, Exception ex)
        {
            await CandyLog.Insert(new CandyGlobalLogDto
            {
                Location = $"{nameof(CandyLightNovelDetailViewModel)}_{Method}",
                ErrorMsg = ex.Message,
                ErrorStack = ex.StackTrace
            });
            return String.Format(Soft.Toast,nameof(CandyLightNovelDetailViewModel), Method);
        }
        public async void Book()
        {
            try
            {
                Refresh = true;
                await Task.Delay(500);
                var LightNovelDetail = await LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.LightDetail,
                        CacheSpan = Soft.CacheTime,
                        Proxy = this.Proxy,
                        Detail = new LightNovelDetail
                        {
                            DetailAddress = DetailAddress
                        }
                    };
                }).RunsAsync(Light =>
                {
                    Light.RefreshCookie(new LightNovelRefresh
                    {
                        UserName = Account,
                        PassWord = Password
                    }, new LightNovelProxy());
                });
                await Task.Delay(500);
                var LightNovelView = await LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.LightView,
                        Proxy = new LightNovelProxy(),
                        CacheSpan = Soft.CacheTime,
                        View = new LightNovelView
                        {
                            ViewAddress = LightNovelDetail.DetailResult.Address,
                        }
                    };
                }).RunsAsync(Light =>
                {
                    Light.RefreshCookie(new LightNovelRefresh
                    {
                        UserName = Account,
                        PassWord = Password
                    }, new LightNovelProxy());
                });
                Refresh = false;
                this.LightNovelViews = new ObservableCollection<LightNovelViewResult>(LightNovelView.ViewResult);
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Book",ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }
        public async void Content(LightNovelViewResult entity)
        {
            if (entity.IsDown)
            {
                try
                {
                    var LightNovelDown = await LightNovelFactory.LightNovel(opt =>
                    {
                        opt.RequestParam = new LightNovelRequestInput
                        {
                            LightNovelType = LightNovelEnum.LightDownLoad,
                            Proxy = this.Proxy,
                            Down = new LightNovelDown
                            {
                                UId = entity.ChapterURL.AsInt(),
                                BookName = BookName
                            }
                        };
                    }).RunsAsync();

                    var route = Resolve<IAndroidPlatform>().DownPath();

                    AuthorizeHelper.Instance.ApplyPermission(async () =>
                    {
                        var dir = SyncStatic.CreateDir(Path.Combine(route, "CandyDown", "LightNovel", $"{Extension.FileNameFilter(BookName)}"));
                        var fn = SyncStatic.CreateFile(Path.Combine(dir, $"{Extension.FileNameFilter(BookName)}.txt"));
                        var ok = SyncStatic.WriteFile(LightNovelDown.DownResult.Down, fn);
                        if (!ok.IsNullOrEmpty())
                        {
                            using (await MaterialDialog.Instance.LoadingSnackbarAsync("下载完成"))
                            {
                                await Task.Delay(1000);
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Content_Down",ex)))
                    {
                        await Task.Delay(3000);
                    }
                }
            }
            else
            {
                try
                {
                    //内容
                    var LightNovelContent = await LightNovelFactory.LightNovel(opt =>
                    {
                        opt.RequestParam = new LightNovelRequestInput
                        {
                            CacheSpan = Soft.CacheTime,
                            LightNovelType = LightNovelEnum.LightContent,
                            Proxy = this.Proxy,
                            Content = new LightNovelContent
                            {
                                ChapterURL = entity.ChapterURL,
                            }
                        };
                    }).RunsAsync();

                    if (await DownNovel(entity.ChapterURL, LightNovelContent.ContentResult.Content))
                    {
                        var State = LightNovelContent.ContentResult.Image == null;

                        //插入记录
                        await Candy.InsertOrUpdate(new CandyLXSLiShiDto
                        {
                            BookName = BookName,
                            ChapeterAddress = entity.ChapterURL,
                            ChapterName = entity.ChapterName,
                            IsBook = State,
                            Cover = Cover,
                            Content = State ? SyncStatic.Compress(LightNovelContent.ContentResult.Content, SecurityType.Base64) : string.Empty,
                            Image = State ? string.Empty : string.Join(",", LightNovelContent.ContentResult.Image)
                        });
                        Navigation(LightNovelContent.ContentResult, entity.ChapterName);
                    }
                }
                catch (Exception ex)
                {
                    using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Content_Read",ex)))
                    {
                        await Task.Delay(3000);
                    }
                }
            }
        }
        public async Task<bool> DownNovel(string Url, string Check)
        {
            if (Check.Equals("因版权问题，文库不再提供该小说的阅读！"))
            {
                var result = await MaterialDialog.Instance.ConfirmAsync("因版权问题，文库不再提供该小说的阅读！是否下载阅读?", "提示", "确认", "取消");

                var UId = Regex.Matches(Url, "[0-9]+/").LastOrDefault().Value.Split("/").FirstOrDefault().AsInt();

                if (result == true)
                {
                    var LightNovelDown = await LightNovelFactory.LightNovel(opt =>
                    {
                        opt.RequestParam = new LightNovelRequestInput
                        {
                            LightNovelType = LightNovelEnum.LightDownLoad,
                            Proxy = this.Proxy,
                            Down = new LightNovelDown
                            {
                                UId = UId,
                                BookName = BookName
                            }
                        };
                    }).RunsAsync();

                    var route = Resolve<IAndroidPlatform>().DownPath();

                    AuthorizeHelper.Instance.ApplyPermission(async () =>
                    {
                        var dir = SyncStatic.CreateDir(Path.Combine(route, "CandyDown", "LightNovel", $"{Extension.FileNameFilter(BookName)}"));
                        var fn = SyncStatic.CreateFile(Path.Combine(dir, $"{Extension.FileNameFilter(BookName)}.txt"));
                        var ok = SyncStatic.WriteFile(LightNovelDown.DownResult.Down, fn);
                        if (!ok.IsNullOrEmpty())
                        {
                            using (await MaterialDialog.Instance.LoadingSnackbarAsync("下载完成"))
                            {
                                await Task.Delay(1000);
                            }
                        }
                    });

                    return false;
                }
                return false;
            }
            return true;
        }
        public async void Navigation(LightNovelContentResult input, string name)
        {

            if (input.Image == null)
            {
                var Param = new NavigationParameters();
                Param.Add("Content", SyncStatic.Compress(input.Content, SecurityType.Base64));
                Param.Add("ChapterName", name);
                await NavigationService.NavigateAsync(new Uri(nameof(CandyLightNovelContentView), UriKind.Relative), Param);
            }
            else
            {
                var Param = new NavigationParameters();
                Param.Add("Image", input.Image);
                Param.Add("ChapterName", name);
                await NavigationService.NavigateAsync(new Uri(nameof(CandyLightNovelImageView), UriKind.Relative), Param);
            }
        }
        #endregion

        #region Command
        public ICommand SelectedCommand => new DelegateCommand<dynamic>(input =>
        {
            if (input != null)
                Content(input);
        });
        public ICommand RefreshsCommand => new DelegateCommand(async () =>
        {
            Refresh = true;
            await Task.Delay(Soft.WaitSpan);
            Refresh = false;
        });
        #endregion
    }
}
