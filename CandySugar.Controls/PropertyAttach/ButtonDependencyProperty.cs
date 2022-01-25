using CandySugar.Controls.UIElementHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wallpaper.SDK;
using Wallpaper.SDK.ViewModel;
using Wallpaper.SDK.ViewModel.Enums;
using Wallpaper.SDK.ViewModel.Request;
using XExten.Advance.StaticFramework;

namespace CandySugar.Controls.PropertyAttach
{
    public class ButtonDependencyProperty
    {
        public static string GetBackImage(DependencyObject obj)
        {
            return (string)obj.GetValue(BackImageProperty);
        }
        public static void SetBackImage(DependencyObject obj, string value)
        {
            obj.SetValue(BackImageProperty, value);
        }
        public static readonly DependencyProperty BackImageProperty =
            DependencyProperty.RegisterAttached("BackImage", typeof(string), typeof(ButtonDependencyProperty), new PropertyMetadata("", OnBackImageChange));

        private static void OnBackImageChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var btn = d as Button;
            if (btn != null)
            {
                SyncStatic.TryCatch(() =>
                {
                    OK(btn);
                }, ex => Error(btn));
            }
        }
        private static void OK(Button btn)
        {
            var WallpaperDown = WallpaperFactory.Wallpaper(opt =>
            {
                opt.RequestParam = new WallpaperRequestInput
                {
                    WallpaperType = WallpaperEnum.Download,
                    CacheSpan = 60,
                    Download = new WallpaperDownload()
                    {
                        Route = GetBackImage(btn)
                    },
                    Proxy = new WallpaperProxy()
                };
            }).Runs();
            var img = ImageHelper.BitmapToBitmapImage(WallpaperDown.DownloadResult.Bytes);
            var brush = new ImageBrush(img)
            {
                Stretch = Stretch.UniformToFill,
                TileMode = TileMode.None,
                Opacity = 0.5d
            };
            btn.Background = brush;
        }

        private static void Error(Button btn)
        {
            var uri = new Uri(GetBackImage(btn));
            var img = new BitmapImage(uri);
            var brush = new ImageBrush(img)
            {
                Stretch = Stretch.UniformToFill,
                TileMode = TileMode.None,
                Opacity = 0.5d
            };
            btn.Background = brush;
        }
    }
}
