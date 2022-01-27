using CandySugar.Controls.UIElementHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CandySugar.Controls.PropertyAttach
{
    public class ScrollViewerDependencyProperty
    {
        public static ICommand GetAtEndCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(AtEndCommandProperty);
        }
        public static void SetAtEndCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(AtEndCommandProperty, value);
        }
        public static readonly DependencyProperty AtEndCommandProperty = DependencyProperty.RegisterAttached("AtEndCommand", typeof(ICommand), typeof(ScrollViewerDependencyProperty), new PropertyMetadata(OnAtEndCommandChanged));
        public static void OnAtEndCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)d;
            if (element != null)
            {
                element.Loaded -= element_Loaded;
                element.Loaded += element_Loaded;
            }
        }
        private static void element_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;

            element.Loaded -= element_Loaded;

            ScrollViewer scrollViewer = UlHelper.FindVisualChild<ScrollViewer>(element).FirstOrDefault();

            if (scrollViewer == null)
            {
                throw new InvalidOperationException("ScrollViewer not found.");
            }

            scrollViewer.ScrollChanged += delegate
            {
                bool atBottom = scrollViewer.VerticalOffset
                                 >= scrollViewer.ScrollableHeight;

                if (atBottom)
                {
                    var atEnd = GetAtEndCommand(element);
                    if (atEnd != null)
                    {
                        atEnd.Execute(null);
                    }
                }
            };
        }
    }
}
