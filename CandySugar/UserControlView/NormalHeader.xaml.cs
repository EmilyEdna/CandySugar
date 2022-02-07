using CandySugar.CandyWindows;
using CandySugar.CandyWindows.CnadyWinViewModel;
using CandySugar.Common.Enum;
using CandySugar.Controls.ControlViewModel;
using System.Windows;
using System.Windows.Controls;

namespace CandySugar.UserControlView
{
    /// <summary>
    /// NormalHeader.xaml 的交互逻辑
    /// </summary>
    public partial class NormalHeader : UserControl
    {
        public NormalHeader()
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
            var window = Window.GetWindow(this);

            if (window is CandyVLCWin)
            {
                var VLCWindow = ((CandyVLCWin)window);
                VLCWindow.MediaPlayers?.Dispose();
                VLCWindow.LibVlcs?.Dispose();
                VLCWindow.Timer?.Stop();
                VLCWindow.Close();
            }
            else 
                window.Close();
        }
    }
}
