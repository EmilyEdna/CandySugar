using CandySugar.Common.Enum;
using CandySugar.Controls.ControlViewModel;
using CandySugar.WPF.UserControlView.UserControlEvent;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CandySugar.WPF.UserControlView
{
    /// <summary>
    /// NormalHeader.xaml 的交互逻辑
    /// </summary>
    public partial class NormalHeader : UserBaseControl
    {
        public NormalHeader()
        {
            InitializeComponent();
            ThemeCombox(ThemeBox);
        }
    }
}
