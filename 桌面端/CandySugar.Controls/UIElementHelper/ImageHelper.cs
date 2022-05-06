using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace CandySugar.Controls.UIElementHelper
{
    public class ImageHelper
    {
        /// <summary>
        /// 此方法不会立即回收内存
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        [Obsolete("此方法不会立即回收内存")]
        public static BitmapImage ByteToImageBit(byte[] bytes, int width = 1200)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(bytes);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.DecodePixelWidth = width;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// 转Bitmap
        /// </summary>
        /// <param name="srcFile">源文件</param>
        /// <returns></returns>
        public static BitmapSource BitmapToBitmapImage(byte[] bytes)
        {
            Bitmap bmp = Image.FromStream(new MemoryStream(bytes)) as Bitmap;
            var ptr = bmp.GetHbitmap();
            var source = Imaging.CreateBitmapSourceFromHBitmap(
                  ptr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            source.Freeze();
            bmp.Dispose();
            DeleteObject(ptr);
            return source;
        }
    }
}
