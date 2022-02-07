using CandySugar.CandyWindows;
using CandySugar.CandyWindows.CnadyWinViewModel;
using CandySugar.Common.Enum;
using CandySugar.Controls.ControlViewModel;
using System.Windows;
using System.Windows.Controls;

namespace CandySugar.UserControlView
{
    /// <summary>
    /// BasicHeader.xaml 的交互逻辑
    /// </summary>
    public partial class BasicHeader : UserControl
    {
        public BasicHeader()
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
                    Setting();
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
            Application.Current.Shutdown();
        }
        private void Setting()
        {
            CandySettingWin win = new CandySettingWin
            {
                DataContext = CandyViewModule.Container.Get<CandySettingViewModel>()
            };
            win.Show();
        }
    }
}
