using CandySugar.Xam.Common;
using Manga.SDK;
using Manga.SDK.ViewModel;
using Manga.SDK.ViewModel.Emums;
using Manga.SDK.ViewModel.Request;
using Manga.SDK.ViewModel.Response;
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
using Xamarin.Forms;
using XExten.Advance.CacheFramework;
using XExten.Advance.InternalFramework.Securities.Common;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;
using XF.Material.Forms.UI.Dialogs;

namespace CandySugar.App.Controls.ViewModels.MangaModel
{
    public class CandyMangaReaderViewModel : ViewModelNavigatBase
    {
        private readonly MangaProxy Proxy;
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
        }

        #region Property
        private bool _Refresh;
        public bool Refresh
        {
            get { return _Refresh; }
            set { SetProperty(ref _Refresh, value); }
        }
        private ObservableCollection<byte[]> _Source;
        public ObservableCollection<byte[]> Source
        {
            get => _Source;
            set => SetProperty(ref _Source, value);
        }
        private string _Url;
        public string Url
        {
            get => _Url;
            set => SetProperty(ref _Url, value);
        }
        private MangaContentResult _Result;
        public MangaContentResult Result {
            get => _Result;
            set => SetProperty(ref _Result, value);
        }
        #endregion

        #region Mothod
        public async Task<string> Content()
        {
            try
            {
                this.Refresh = true;
                var key  = Url.ToMd5();
                var data = Caches.RunTimeCacheGet<string>(key);
                if (!data.IsNullOrEmpty())
                    return data;
                var MangaContent = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new MangaRequestInput
                    {
                        MangaType = MangaEnum.ContentApp,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.CacheTime,
                        Content = new MangaContent
                        {
                            Address = Url
                        }
                    };
                }).RunsAsync();
                Result = MangaContent.ContentResults ;
                var arr = string.Join(",", MangaContent.ContentResults.ImageURL);
                var param = $"[{arr}]";
                Caches.RunTimeCacheSet(key, param,900,true);
                return param;
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(Soft.Toast))
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
                var key =  string.Join(",", input).ToMd5();
                var data = Caches.RunTimeCacheGet<List<byte[]>>(key);
                if (!data.IsNullOrEmpty())
                    Source = new ObservableCollection<byte[]>(data);
                var MangaBytes = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new MangaRequestInput
                    {
                        MangaType = MangaEnum.DownLoad,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.CacheTime,
                        ContentByte = new MangaContentByte
                        {
                            ImageURL = input,
                            Header = Result.MangaHeader
                        }
                    };
                }).RunsAsync();
                Caches.RunTimeCacheSet(key, MangaBytes.ContentByteResults.ImageBytes, 900, true);
                Source = new ObservableCollection<byte[]>(MangaBytes.ContentByteResults.ImageBytes);
                this.Refresh = false;
            }
            catch (Exception)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(Soft.Toast))
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
