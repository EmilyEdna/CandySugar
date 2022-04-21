using CandySugar.Common.Enum;
using CandySugar.Controls.UIElementHelper;
using SDKColloction.GalActorSDK;
using SDKColloction.GalActorSDK.ViewModel;
using SDKColloction.GalActorSDK.ViewModel.Eunms;
using SDKColloction.GalActorSDK.ViewModel.Request;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using SDKColloction.WallpaperSDK;
using SDKColloction.WallpaperSDK.ViewModel;
using SDKColloction.WallpaperSDK.ViewModel.Enums;
using SDKColloction.WallpaperSDK.ViewModel.Request;
using XExten.Advance.StaticFramework;

namespace CandySugar.Controls.Converters
{
    /// <summary>
    /// 图片转换
    /// </summary>
    public class ImageSouceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (ImageConvertFuncEnum)parameter;
            switch (type)
            {
                case ImageConvertFuncEnum.Konachan:
                    try
                    {
                        var WallpaperDown = WallpaperFactory.Wallpaper(opt =>
                         {
                             opt.RequestParam = new WallpaperRequestInput
                             {
                                 WallpaperType = WallpaperEnum.Download,
                                 CacheSpan = 180,
                                 Download = new WallpaperDownload()
                                 {
                                     Route = value.ToString()
                                 },
                                 Proxy = new WallpaperProxy()
                             };
                         }).Runs();
                        return ImageHelper.BitmapToBitmapImage(WallpaperDown.DownloadResult.Bytes);
                    }
                    catch
                    {
                        return value;
                    }
                case ImageConvertFuncEnum.Axgle:
                    try
                    {
                        var GalCover = GalActorFactory.GalActor(opt =>
                        {
                            opt.RequestParam = new GalActorRequestInput
                            {
                                Galype = GalActorEnum.Show,
                                CacheSpan = 180,
                                Proxy = new GalActorProxy(),
                                Show = new GalActorShow
                                {
                                    KeyWord = value.ToString()
                                }
                            };
                        }).Runs();

                        return ImageHelper.BitmapToBitmapImage(GalCover.Bytes);
                        throw new Exception();
                    }
                    catch
                    {
                      return new BitmapImage(new Uri("pack://application:,,,/CandySugar.Resource;component/Assets/Cover.jpg"));
                    }
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
