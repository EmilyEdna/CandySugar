using CandySugar.WPF.CandyWindows;
using CandySugar.WPF.CandyWindows.CandyWinViewModel;
using CandySugar.Common.Enum;
using CandySugar.Controls.ControlViewModel;
using CandySugar.WPF.Properties;
using CandySugar.WPF.UserControlView.UserControlEvent;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CandySugar.WPF.UserControlView
{
    /// <summary>
    /// BasicHeader.xaml 的交互逻辑
    /// </summary>
    public partial class BasicHeader : UserBaseControl
    {
        public BasicHeader()
        {
            InitializeComponent();
            ThemeCombox(ThemeBox);
        }   
    }
}
