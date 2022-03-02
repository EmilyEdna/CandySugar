using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CandySugar.App.Controls.Behaviors
{
    public class ListViewBehavior : Behavior<ListView>
    {
        public static ICommand GetAttachBehavior(BindableObject view)
        {
            return (ICommand)view.GetValue(CommandProperty);
        }

        public static void SetAttachBehavior(BindableObject view, bool value)
        {
            view.SetValue(CommandProperty, value);
        }


        public static readonly BindableProperty CommandProperty =
        BindableProperty.CreateAttached("Command", typeof(ICommand), typeof(ListViewBehavior), null, BindingMode.OneWay, null, ItemSelected);

        private static void ItemSelected(BindableObject bindable, object oldValue, object newValue)
        {
            var entry = bindable as ListView;
            if (entry == null)
            {
                return;
            }
        }
    }
}
