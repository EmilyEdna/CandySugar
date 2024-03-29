﻿using CandySugar.Common.DTO;
using CandySugar.Controls.UIElementHelper;
using CandySugar.Controls.UserControls;
using CandySugar.Core.Service;
using CandySugar.Core.ServiceImpl;
using SDKColloction.GalActorSDK;
using SDKColloction.GalActorSDK.ViewModel;
using SDKColloction.GalActorSDK.ViewModel.Eunms;
using SDKColloction.GalActorSDK.ViewModel.Request;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using XExten.Advance.HttpFramework.MultiFactory;

namespace CandySugar.Controls.PropertyAttach
{
    public class ImageSouceDependencyProperty
    {
        public static Uri GetSourceUri(DependencyObject obj)
        {
            return (Uri)obj.GetValue(SourceUriProperty);
        }
        public static void SetSourceUri(DependencyObject obj, Uri value)
        {
            obj.SetValue(SourceUriProperty, value);
        }

        public static readonly DependencyProperty SourceUriProperty = DependencyProperty.RegisterAttached("SourceUri", typeof(Uri), typeof(ImageSouceDependencyProperty), new PropertyMetadata(ImageChaged));

        private static async void ImageChaged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is CandyImgButton)
            {
                ((CandyImgButton)obj).SetBinding(CandyImgButton.ImageProperty, new Binding()
                {
                    Source =await Binds(e.NewValue.ToString(), true),
                    IsAsync = true
                });
            }
            else
            {
                ((Image)obj).SetBinding(Image.SourceProperty, new Binding()
                {
                    Source = await Binds(e.NewValue.ToString()),
                    IsAsync = true,
                });
            }
        }

        private async static Task<BitmapSource> Binds(string Uri, bool Save = false)
        {
            try
            {
                IAx service = new Ax();
                if (Save)
                {
                    var bytes = service.GetCover(Uri);
                    if (bytes.Length > 0)
                        return ImageHelper.BitmapToBitmapImage(bytes);
                }

                var GalCover = await GalActorFactory.GalActor(opt =>
                {
                    opt.RequestParam = new GalActorRequestInput
                    {
                        Galype = GalActorEnum.GalShows,
                        CacheSpan = 300,
                        Show = new GalActorShow
                        {
                            KeyWord = Uri
                        }
                    };
                }).RunsAsync();

                if (Save)
                {
                    service.SaveCover(new CandyGalCoverDto
                    {
                        Cover = Uri,
                        Source = GalCover.Bytes
                    });
                }

                return ImageHelper.BitmapToBitmapImage(GalCover.Bytes);
            }
            catch
            {
                return new BitmapImage(new Uri("pack://application:,,,/CandySugar.Resource;component/Assets/Cover.jpg"));
            }
        }
    }
}
