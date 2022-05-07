using CandySugar.Controls.UIElementHelper;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CandySugar.Controls.Behaviors
{
    public class ListBoxBehavior: Behavior<ListBox>
    {

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(ListBoxBehavior),
            new PropertyMetadata(null));

        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseWheel += ListBoxWheel;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseWheel -= ListBoxWheel;
            base.OnDetaching();
        }

        private void ListBoxWheel(object sender, MouseWheelEventArgs e)
        {
            ListBox Box = (ListBox)sender;
            ScrollViewer scroll = UlHelper.FindVisualChild<ScrollViewer>(Box).FirstOrDefault();

            if (scroll != null)
            {
                if (e.Delta > 0)
                {
                    //上一页
                    if (scroll.VerticalOffset == 0)
                        if (this.Command != null)
                            Command.Execute(new Dictionary<string, int> { { Box.Name, -1 } });
                }
                else
                {
                    //下一页
                    if (scroll.VerticalOffset + scroll.ViewportHeight == scroll.ExtentHeight)
                    {
                        Command.Execute(new Dictionary<string, int> { { Box.Name, 1 } });
                        scroll.ScrollToTop();
                    }
                }
            }
        }
    }
}
