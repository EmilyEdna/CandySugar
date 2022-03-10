﻿using CandySugar.Xam.Common;
using Novel.SDK;
using Novel.SDK.ViewModel;
using Novel.SDK.ViewModel.Enums;
using Novel.SDK.ViewModel.Request;
using Novel.SDK.ViewModel.Response;
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
using System.Linq;

namespace CandySugar.App.Controls.ViewModels.NovelModel
{
    public class CandyNovelContentViewModel : ViewModelNavigatBase
    {
        private readonly NovelProxy Proxy;
        public CandyNovelContentViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = new NovelProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
        }

        #region Filed
        private string Next;
        #endregion

        #region Overrive

        public override void Initialize(INavigationParameters parameters)
        {
            var route = parameters.GetValue<string>("Route");
            Contents(route);
        }
        #endregion

        #region Property
        private ObservableCollection<string> _Content;
        public ObservableCollection<string> Content
        {
            get { return _Content; }
            set { SetProperty(ref _Content, value); }
        }
        private int _FontSize;
        public int FontSize
        {
            get { return _FontSize; }
            set { SetProperty(ref _FontSize, value); }
        }
        private string _ChapterName;
        public string ChapterName
        {
            get { return _ChapterName; }
            set { SetProperty(ref _ChapterName, value); }
        }
        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { SetProperty(ref _IsBusy, value); }
        }
        #endregion

        #region Command
        public ICommand ShowMoreCommand => new DelegateCommand(() =>
        {
            Contents(Next);
        });
        #endregion

        #region Method
        public async void Contents(string input)
        {
            try
            {
                IsBusy = true;
                await Task.Delay(500);
                var NovelContent = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new NovelRequestInput
                    {
                        CacheSpan = Soft.CacheTime,
                        NovelType = NovelEnum.Watch,
                        Proxy = this.Proxy,
                        View = new NovelView
                        {
                            NovelViewAddress = input.ToString()
                        }
                    };
                }).RunsAsync();

                this.ChapterName = NovelContent.Contents.ChapterName;

                Next = NovelContent.Contents.NextPage.IsNullOrEmpty() ? NovelContent.Contents.NextChapter : NovelContent.Contents.NextPage;
                NovelContent.Contents.Content = NovelContent.Contents.Content.Replace("　", "\t");

                if (this.Content == null)
                    this.Content = new ObservableCollection<string>(new List<string>());

                NovelContent.Contents.Content.Split("\t", StringSplitOptions.RemoveEmptyEntries).ForEnumerEach(t =>
                {
                    this.Content.Add("\t\t\t\t\t" + t + "\r\n");
                });
                IsBusy = false;
            }
            catch
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("网络有波动，请稍后再试~`(*>﹏<*)′"))
                {
                    await Task.Delay(3000);
                }
            }
        }
        #endregion
    }
}
