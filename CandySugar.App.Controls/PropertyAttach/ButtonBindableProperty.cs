using System;
using System.Collections.Generic;
using System.Text;
using Wallpaper.SDK;
using Wallpaper.SDK.ViewModel;
using Wallpaper.SDK.ViewModel.Enums;
using Wallpaper.SDK.ViewModel.Request;
using Xamarin.Forms;

namespace CandySugar.App.Controls.PropertyAttach
{
    public class ButtonBindableProperty
    {
        public static string GetSource(BindableObject obj)
        {
            return (string)obj.GetValue(SourceProperty);
        }

        public static void SetSource(BindableObject obj, string value)
        {
            obj.SetValue(SourceProperty, value);
        }

        public static readonly BindableProperty SourceProperty =
            BindableProperty.CreateAttached("Source", typeof(string), typeof(ButtonBindableProperty), null, BindingMode.TwoWay, null, OnImageChange);

        private static void OnImageChange(BindableObject bindable, object oldValue, object newValue)
        {
        }
    }
}
