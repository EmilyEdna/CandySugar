using CandySugar.Xam.Common;
using CandySugar.Xam.Common.DTO;
using CandySugar.Xam.Core.Service;
using Novel.SDK;
using Novel.SDK.ViewModel;
using Novel.SDK.ViewModel.Enums;
using Novel.SDK.ViewModel.Request;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XExten.Advance.LinqFramework;
using XF.Material.Forms.UI.Dialogs;

namespace CandySugar.App.Controls.ViewModels.NovelModel
{
    public class CandyNovelContentViewModel : ViewModelNavigatBase
    {
        private readonly NovelProxy Proxy;
        private readonly IXSLiShi Candy;
        public CandyNovelContentViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = new NovelProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
            Candy = ContainerLocator.Container.Resolve<IXSLiShi>();
            this.TextTheme = Color.Black;
            this.Theme = Color.FromHex("DDCDA1");

        }

        #region Filed
        private string Next;
        private string BookName;
        #endregion

        #region Overrive

        public override void Initialize(INavigationParameters parameters)
        {
            var ChapterURL = parameters.GetValue<string>("ChapterURL");
            BookName = parameters.GetValue<string>("BookName");

            Contents(ChapterURL);
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
        private Color _Theme;
        public Color Theme
        {
            get { return _Theme; }
            set { SetProperty(ref _Theme, value); }
        }
        private Color _TextTheme;
        public Color TextTheme
        {
            get { return _TextTheme; }
            set { SetProperty(ref _TextTheme, value); }
        }
        #endregion

        #region Command
        public ICommand ShowMoreCommand => new DelegateCommand(() =>
        {
            Contents(Next);

        });

        public ICommand ThemeCommand => new DelegateCommand<dynamic>(input =>
        {
            if (input != null)
            {
                if ((Color)input == Color.Black)
                    this.TextTheme = Color.White;
                else
                    this.TextTheme = Color.Black;
                this.Theme = (Color)input;
            }
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
                            NovelViewAddress = input
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

                await Candy.InsertOrUpdate(new CandyXSLiShiDto
                {
                    BookName = BookName,
                    ChapeterAddress = input,
                    ChapterName = ChapterName,
                });
                IsBusy = false;
            }
            catch (Exception ex)
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
