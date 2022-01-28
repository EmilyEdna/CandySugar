using CandySugar.Common.Enum;
using System.Windows;
using System.Windows.Controls;

namespace CandySugar.UserControlView
{
    /// <summary>
    /// MangaHeader.xaml 的交互逻辑
    /// </summary>
    public partial class MangaHeader : UserControl
    {
        public MangaHeader()
        {
            InitializeComponent();
        }
        private void CandySystemClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var types = (SysFuncEnum)btn.CommandParameter;
            switch (types)
            {
                case SysFuncEnum.Play:
                    break;
                case SysFuncEnum.Download:
                    break;
                case SysFuncEnum.Setting:
                    break;
                case SysFuncEnum.MinSize:
                    Min();
                    break;
                case SysFuncEnum.Close:
                    Close();
                    break;
                default:
                    break;
            }
        }

        private void Min()
        {
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Visibility = Visibility.Collapsed;
            }
        }
        private void Close()
        {
            Window.GetWindow(this).Close();
            //Application.Current.Shutdown();
        }
    }
}
