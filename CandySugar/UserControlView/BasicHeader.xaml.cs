using CandySugar.CandyWindows;
using CandySugar.CandyWindows.CnadyWinViewModel;
using CandySugar.Common.Enum;
using CandySugar.Controls.ControlViewModel;
using CandySugar.Properties;
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

        private void ThemeSelected(object sender, SelectionChangedEventArgs e)
        {
            var select = (sender as ComboBox);
            var item = (ThemeFuncEnum)(select.Items[select.SelectedIndex] as ComboBoxItem).TabIndex;
            switch (item)
            {
                case ThemeFuncEnum.BaoShilv:
                    Soft.Default.Theme = "#FF2CCFA0";
                    break;
                case ThemeFuncEnum.TaoHuafen:
                    Soft.Default.Theme = "#FFFF9999";
                    break;
                case ThemeFuncEnum.XuZilan:
                    Soft.Default.Theme = "#FF10AEC2";
                    break;
                case ThemeFuncEnum.ShanChahong:
                    Soft.Default.Theme = "#FFED556A";
                    break;
                case ThemeFuncEnum.MoYuhei:
                    Soft.Default.Theme = "#FF000000";
                    break;
                case ThemeFuncEnum.ZiShuhong:
                    Soft.Default.Theme = "#FFEF4289";
                    break;
                default:
                    break;
            }
        }
    }
}
