using CandySugar.Xam.Common;
using CandySugar.Xam.Common.AppDTO;
using CandySugar.Xam.Core.Service;
using SDKColloction.MangaSDK;
using SDKColloction.MangaSDK.ViewModel;
using SDKColloction.MangaSDK.ViewModel.Emums;
using SDKColloction.MangaSDK.ViewModel.Request;
using SDKColloction.MangaSDK.ViewModel.Response;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Ioc;
using XF.Material.Forms.UI.Dialogs;
using CandySugar.Xam.Common.DTO;

namespace CandySugar.App.Controls.ViewModels.MangaModel
{
    public class CandyMangaReaderViewModel : ViewModelNavigatBase
    {
        private readonly MangaProxy Proxy;
        private readonly ILoger CandyLog;
        public CandyMangaReaderViewModel(INavigationService navigationService) : base(navigationService)
        {
            Proxy = new MangaProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
            this.Refresh = false;
            CandyLog = Resolve<ILoger>();
        }

        #region Property
        private bool _Refresh;
        public bool Refresh
        {
            get { return _Refresh; }
            set { SetProperty(ref _Refresh, value); }
        }
        private ObservableCollection<MangaBytesDto> _BytesSource;
        public ObservableCollection<MangaBytesDto> BytesSource
        {
            get => _BytesSource;
            set => SetProperty(ref _BytesSource, value);
        }
        private string _Url;
        public string Url
        {
            get => _Url;
            set => SetProperty(ref _Url, value);
        }
        private MangaContentResult _Result;
        public MangaContentResult Result
        {
            get => _Result;
            set => SetProperty(ref _Result, value);
        }
        #endregion

        #region Mothod
        private async Task<string> Tip(string Method, Exception ex)
        {
            await CandyLog.Insert(new CandyGlobalLogDto
            {
                Location = $"{nameof(CandyMangaReaderViewModel)}_{Method}",
                ErrorMsg = ex.Message,
                ErrorStack = ex.StackTrace
            });
            return String.Format(Soft.Toast, nameof(CandyMangaReaderViewModel), Method);
        }
        public async Task<string> Content()
        {
            try
            {
                this.Refresh = true;
                var MangaContent = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new MangaRequestInput
                    {
                        MangaType = MangaEnum.MangaContentApp,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.CacheTime,
                        Content = new MangaContent
                        {
                            Address = Url
                        }
                    };
                }).RunsAsync();
                Result = MangaContent.ContentResults;
                var arr = string.Join(",", MangaContent.ContentResults.ImageURL);
                var param = $"[{arr}]";
                return param;
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("Content", ex)))
                {
                    await Task.Delay(3000);
                }
                this.Refresh = false;
                return string.Empty;
            }
        }

        public async void ContentBytes(List<string> input)
        {
            try
            {
                var MangaBytes = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new MangaRequestInput
                    {
                        MangaType = MangaEnum.MangaDownLoad,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.CacheTime,
                        ContentByte = new MangaContentByte
                        {
                            ImageURL = input,
                            Header = Result.MangaHeader
                        }
                    };
                }).RunsAsync();

                var dto = MangaBytes.ContentByteResults.ImageBytes.Select(item => new MangaBytesDto
                {
                    Data = item
                }).ToList();

                BytesSource = new ObservableCollection<MangaBytesDto>(dto);
                this.Refresh = false;
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(await Tip("ContentBytes", ex)))
                {
                    await Task.Delay(3000);
                }
            }
        }
        #endregion

        #region Override
        public override void Initialize(INavigationParameters parameters)
        {
            Url = parameters.GetValue<string>("Route");
        }

        #endregion

        #region Command
        public ICommand RefreshsCommand => new DelegateCommand(async () =>
        {
            Refresh = true;
            await Task.Delay(Soft.WaitSpan);
            Refresh = false;
        });
        #endregion
    }
}
