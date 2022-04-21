using CandySugar.Xam.Common;
using SDKColloction.MusicSDK;
using SDKColloction.MusicSDK.ViewModel;
using SDKColloction.MusicSDK.ViewModel.Enums;
using SDKColloction.MusicSDK.ViewModel.Request;
using SDKColloction.MusicSDK.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;
using XF.Material.Forms.UI.Dialogs;
using Prism.Ioc;
using Prism.Commands;
using System.Windows.Input;
using CandySugar.App.Controls.ViewModels.MusicModel;

namespace CandySugar.App.Controls.LayoutView.LayoutViewModel
{
    public class PopSheetContentViewModel : ViewModelBase
    {
        private readonly MusicProxy Proxy;
        public PopSheetContentViewModel()
        {
            this.Proxy = new MusicProxy
            {
                IP = Soft.ProxyIP,
                Port = Soft.ProxyPort,
                PassWord = Soft.ProxyPwd,
                UserName = Soft.ProxyAccount
            };
            this.PageIndex = 1;
        }

        #region Property
        private ObservableCollection<MusicSongItem> _SheetDetail;
        public ObservableCollection<MusicSongItem> SheetDetail
        {
            get { return _SheetDetail; }
            set { SetProperty(ref _SheetDetail, value); }
        }

        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { SetProperty(ref _PageIndex, value); }
        }

        public MusicPlatformEnum Platform { get; set; }
        #endregion

        #region Commod
        public ICommand AddPlayCommand => new DelegateCommand<MusicSongItem>(input =>
        {
            Resolve<CandyMusicViewModel>().LoadMusic(input, Platform);
        });
        #endregion

        #region Method
        private string Tip(string Method)
        {
            return String.Format(Soft.Toast, nameof(PopAlbumContentViewModel), Method);
        }
        public async void SearchSheet(MusicSongSheetItem input, MusicPlatformEnum platform)
        {
            try
            {
                var SongSheet = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new MusicRequestInput
                    {
                        Proxy = this.Proxy,
                        MusicPlatformType = platform,
                        MusicType = MusicTypeEnum.SheetDetail,
                        SheetSearch = new MusicSheetSearch
                        {
                            Page = PageIndex,
                            Id = input.SongSheetId.AsString()
                        }
                    };
                }).RunsAsync();

                if (this.SheetDetail == null)
                    this.SheetDetail = new ObservableCollection<MusicSongItem>(SongSheet.SongSheetDetailResult.SongItems);
                else
                    SongSheet.SongSheetDetailResult.SongItems.ForEach(item =>
                    {
                        this.SheetDetail.Add(item);
                    });

                if (SongSheet.SongSheetDetailResult.MusicNum < SheetDetail.Count)
                {
                    PageIndex++;
                    SearchSheet(input, platform);
                }

                Platform = SongSheet.SongSheetDetailResult.MusicPlatformType.Value;
            }
            catch (Exception ex)
            {
                using (await MaterialDialog.Instance.LoadingSnackbarAsync(Tip("SearchSheet")))
                {
                    await Task.Delay(3000);
                }
            }
        }
        #endregion
    }
}
