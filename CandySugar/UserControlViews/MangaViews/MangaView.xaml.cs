using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CandySugar.UserControlViews.MangaViews
{
    /// <summary>
    /// MangaView.xaml 的交互逻辑
    /// </summary>
    public partial class MangaView : UserControl
    {
        public MangaView()
        {
            InitializeComponent();
        }

        private void MangaChanged(object sender, ScrollChangedEventArgs e)
        {
            var vm = (this.DataContext as MangaViewModel);
            if (vm.PageIndex == 1 && e.VerticalOffset == 0)
                return;
            if (vm.PageIndex > 1 && e.VerticalChange >= -48 && e.VerticalOffset == 0)
            {
                Dispatcher.Invoke(() => vm.LoadMore(false));
                var source = (e.OriginalSource as HandyControl.Controls.ScrollViewer);
                source.ScrollToHome();
            }
            if (e.ExtentHeight <= 687)
            {
                if (e.VerticalOffset >= 59)
                {
                    Dispatcher.Invoke(() => vm.LoadMore(true));
                    var source = (e.OriginalSource as HandyControl.Controls.ScrollViewer);
                    source.ScrollToHome();
                }
            }
        }
    }
}
