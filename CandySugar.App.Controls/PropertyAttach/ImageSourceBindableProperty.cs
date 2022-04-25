using FFImageLoading;
using FFImageLoading.Forms;
using SDKColloction.GalActorSDK;
using SDKColloction.GalActorSDK.ViewModel;
using SDKColloction.GalActorSDK.ViewModel.Eunms;
using SDKColloction.GalActorSDK.ViewModel.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SDKColloction.WallpaperSDK;
using SDKColloction.WallpaperSDK.ViewModel;
using SDKColloction.WallpaperSDK.ViewModel.Enums;
using SDKColloction.WallpaperSDK.ViewModel.Request;
using Xamarin.Forms;
using XExten.Advance.LinqFramework;

namespace CandySugar.App.Controls.PropertyAttach
{
    public class ImageSourceBindableProperty
    {
        #region Konachan
        public static string GetKonachanSource(BindableObject obj)
        {
            return (string)obj.GetValue(KonachanSourceProperty);
        }

        public static void SetKonachanSource(BindableObject obj, string value)
        {
            obj.SetValue(KonachanSourceProperty, value);
        }
     
        public static readonly BindableProperty KonachanSourceProperty =
            BindableProperty.CreateAttached("KonachanSource", typeof(string),
                typeof(ImageSourceBindableProperty),
                null, BindingMode.TwoWay, null, KonachanChanged);

        private async static void KonachanChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var FFImage = ((CachedImage)bindable);
            if (newValue != null)
            {
                FFImage.SetBinding(CachedImage.SourceProperty, new Binding
                {
                    Source = await KonachanSource(newValue.ToString())
                });
            }
        }

        private async static Task<ImageSource> KonachanSource(string route)
        {
            var WallpaperDown = await WallpaperFactory.Wallpaper(opt =>
            {
                opt.RequestParam = new WallpaperRequestInput
                {
                    WallpaperType = WallpaperEnum.ImgDownloads,
                    CacheSpan = 600,
                    Download = new WallpaperDownload()
                    {
                        Route = route
                    },
                    Proxy = new WallpaperProxy()
                };
            }).RunsAsync();
            return ImageSource.FromStream(() => new MemoryStream(WallpaperDown.DownloadResult.Bytes));
        }
        #endregion

        #region Axgle
        public static string GetAxgleSource(BindableObject obj)
        {
            return (string)obj.GetValue(AxgleSourceProperty);
        }

        public static void SetAxgleSource(BindableObject obj, string value)
        {
            obj.SetValue(AxgleSourceProperty, value);
        }

        public static readonly BindableProperty AxgleSourceProperty =
          BindableProperty.CreateAttached("AxgleSource", typeof(string),
              typeof(ImageSourceBindableProperty),
              null, BindingMode.TwoWay, null, AxgleChanged);
        private async static void AxgleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var FFImage = ((CachedImage)bindable);
            if (newValue != null)
            {
                FFImage.SetBinding(CachedImage.SourceProperty, new Binding
                {
                    Source = await AxgleSource(newValue.ToString())
                });
            }
        }

        private async static Task<ImageSource> AxgleSource(string route)
        {
            var GalCover = await GalActorFactory.GalActor(opt =>
            {
                opt.RequestParam = new GalActorRequestInput
                {
                    Galype = GalActorEnum.GalShows,
                    CacheSpan = 600,
                    Show = new GalActorShow
                    {
                        KeyWord = route
                    }
                };
            }).RunsAsync();
            return ImageSource.FromStream(() => new MemoryStream(GalCover.Bytes));
        }

        #endregion
    }
}
