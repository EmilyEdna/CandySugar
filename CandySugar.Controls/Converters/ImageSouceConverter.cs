using CandySugar.Common.Enum;
using CandySugar.Controls.UIElementHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Wallpaper.SDK;
using Wallpaper.SDK.ViewModel;
using Wallpaper.SDK.ViewModel.Enums;
using Wallpaper.SDK.ViewModel.Request;
using XExten.Advance.StaticFramework;

namespace CandySugar.Controls.Converters
{
    /// <summary>
    /// 图片转换
    /// </summary>
    public class ImageSouceConverter : IValueConverter
    {
        public  object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (ImageConvertFuncEnum)parameter;
            switch (type)
            {
                case ImageConvertFuncEnum.Konachan:
                    try
                    {
                        var WallpaperDown =WallpaperFactory.Wallpaper(opt =>
                        {
                            opt.RequestParam = new WallpaperRequestInput
                            {
                                WallpaperType = WallpaperEnum.Download,
                                CacheSpan = 60,
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
                case ImageConvertFuncEnum.Manga:
                    return null;
                default:
                    return null;
                    break;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
