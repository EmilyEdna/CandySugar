using CandySugar.Common;
using CandySugar.Controls.Commands;
using CandySugar.Controls.UIElementHelper;
using CandySugar.WPF.Properties;
using SDKColloction.MangaSDK;
using SDKColloction.MangaSDK.ViewModel;
using SDKColloction.MangaSDK.ViewModel.Emums;
using SDKColloction.MangaSDK.ViewModel.Request;
using SDKColloction.MangaSDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using XExten.Advance.HttpFramework.MultiFactory;
using XExten.Advance.StaticFramework;

namespace CandySugar.WPF.CandyWindows.CandyWinViewModel
{
    public class CandyMangaReaderViewModel : Screen
    {
        private readonly IContainer Container;
        private readonly MangaProxy Proxy;
        public CandyMangaReaderViewModel(IContainer Container)
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
        private ObservableCollection<MangaChapterResult> _Chapters;
        public ObservableCollection<MangaChapterResult> Chapters
        {
            get { return _Chapters; }
            set { SetAndNotify(ref _Chapters, value); }
        }
        private ObservableCollection<BitmapSource> _Bit;
        public ObservableCollection<BitmapSource> Bit
        {
            get { return _Bit; }
            set { SetAndNotify(ref _Bit, value); }
        }
        private int _Index;
        /// <summary>
        /// 索引
        /// </summary>
        public int Index
        {
            get { return _Index; }
            set { SetAndNotify(ref _Index, value); }
        }

        private int _Total;
        /// <summary>
        /// 总数
        /// </summary>
        public int Total
        {
            get { return _Total; }
            set { SetAndNotify(ref _Total, value); }
        }

        private Visibility _Loading;
        public Visibility Loading
        {
            get { return _Loading; }
            set { SetAndNotify(ref _Loading, value); }
        }

        public ArrayList Names { get; set; }
        #endregion

        #region Method

        public ICommand GoChapter() => new CandyCommand(args =>
        {
            //上一章
            if (args.Equals("0"))
            {
                if (Index < 0)
                    return;
                Index -= 1;
                Loading = Visibility.Visible;
                Bit = new ObservableCollection<BitmapSource>();
                InitCurrent();
            }
            else
            {
                if (Index > Total)
                    return;
                Index += 1;
                Loading = Visibility.Visible;
                Bit = new ObservableCollection<BitmapSource>();
                InitCurrent();
            }
        }, null);

        public async Task InitCurrent()
        {
            HelpUtilty.Dispose();

            var MangaContent = await MangaFactory.Manga(opt =>
            {
                opt.RequestParam = new MangaRequestInput
                {
                    MangaType = MangaEnum.MangaContent,
                    Proxy = this.Proxy,
                    Content = new MangaContent
                    {
                        Address = Chapters[Index].Address
                    }
                };
            }).RunsAsync();
            await CacheLocal(MangaContent.ContentResults, Chapters[Index].TagKey);
            Loading = Visibility.Hidden;
            HelpUtilty.Dispose();
        }

        protected async Task CacheLocal(MangaContentResult Result, string key)
        {
            var dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "CandSugarResource", "MangaCaches", key));

            var all = Directory.GetFiles(dir).OrderBy(t => t).ToList();
            //读取本地
            if (all.Count() == Result.ImageURL.Count)
            {
                Bit = new ObservableCollection<BitmapSource>();
                Names = new ArrayList();
                for (int index = 0; index < all.Count(); index++)
                {
                    FileStream fileStream = new FileStream(all[index], FileMode.Open, FileAccess.Read);
                    byte[] array = new byte[fileStream.Length];
                    fileStream.Read(array, 0, array.Length);
                    Names.Add(all[index]);
                    Bit.Add(ImageHelper.BitmapToBitmapImage(array));
                    fileStream.Close();
                }
            }
            else
            {
                SyncStatic.DeleteFolder(dir);

                var dirs = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "CandSugarResource", "MangaCaches", key));

                var MangaBytes = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new MangaRequestInput
                    {
                        MangaType = MangaEnum.MangaDownLoad,
                        Proxy = this.Proxy,
                        CacheSpan = Soft.Default.CacheTime,
                        ContentByte = new MangaContentByte
                        {
                            ImageURL = Result.ImageURL,
                            Header = Result.MangaHeader
                        }
                    };
                }).RunsAsync();

                Bit = new ObservableCollection<BitmapSource>();
                Names = new ArrayList();
                MangaBytes.ContentByteResults.ImageBytes.ForEach(item =>
                {
                    var file = SyncStatic.CreateFile(Path.Combine(dirs, DateTime.Now.ToString("yyyyMMddHHmmssffff")));
                    SyncStatic.WriteFile(item, file);
                    Names.Add(file);
                    Bit.Add(ImageHelper.BitmapToBitmapImage(item));
                });
            }
        }
        #endregion
    }
}
