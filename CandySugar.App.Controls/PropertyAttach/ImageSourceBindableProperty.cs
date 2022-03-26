using FFImageLoading;
using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Wallpaper.SDK;
using Wallpaper.SDK.ViewModel;
using Wallpaper.SDK.ViewModel.Enums;
using Wallpaper.SDK.ViewModel.Request;
using Xamarin.Forms;
using XExten.Advance.LinqFramework;

namespace CandySugar.App.Controls.PropertyAttach
{
    public class ImageSourceBindableProperty
    {
        public static string GetSuperSource(BindableObject obj)
        {
            return (string)obj.GetValue(SuperSourceProperty);
        }

        public static void SetSuperSource(BindableObject obj, string value)
        {
            obj.SetValue(SuperSourceProperty, value);
        }

        public static readonly BindableProperty SuperSourceProperty =
            BindableProperty.CreateAttached("SuperSource", typeof(string),
                typeof(ImageSourceBindableProperty),
                null, BindingMode.TwoWay, null, OnImageChanged);

        public static byte[] GetSourceBytes(BindableObject obj)
        {
            return (byte[])obj.GetValue(SourceBytesProperty);
        }

        public static void SetSourceBytes(BindableObject obj, byte[] value)
        {
            obj.SetValue(SourceBytesProperty, value);
        }

        public static readonly BindableProperty SourceBytesProperty =
         BindableProperty.CreateAttached("SourceBytes", typeof(byte[]),
             typeof(ImageSourceBindableProperty),
             null, BindingMode.TwoWay, null, OnBytesImageChanged);

        private static void OnBytesImageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var FFImage = ((CachedImage)bindable);
            if (newValue != null)
            {
                var source = ImageSource.FromStream(() => new MemoryStream((byte[])newValue));
                FFImage.SetBinding(CachedImage.SourceProperty, new Binding
                {
                    Source = source
                });
            }
        }

        private async static void OnImageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var FFImage = ((CachedImage)bindable);
            if (newValue != null)
            {
                FFImage.SetBinding(CachedImage.SourceProperty, new Binding
                {
                    Source = await Source(newValue.ToString())
                });
            }
        }

        private async static Task<ImageSource> Source(string route)
        {
            var WallpaperDown = await WallpaperFactory.Wallpaper(opt =>
            {
                opt.RequestParam = new WallpaperRequestInput
                {
                    WallpaperType = WallpaperEnum.Downloads,
                    CacheSpan = 180,
                    Download = new WallpaperDownload()
                    {
                        Route = route
                    },
                    Proxy = new WallpaperProxy()
                };
            }).RunsAsync();
            return ImageSource.FromStream(() => new MemoryStream(WallpaperDown.DownloadResult.Bytes));
        }
    }
}
