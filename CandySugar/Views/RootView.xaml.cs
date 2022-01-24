using CandySugar.Common.Enum;
using CandySugar.Controls.UIElementHelper;
using CandySugar.Controls.UserControls;
using CandySugar.Controls.UserControlView;
using StyletIoC;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CandySugar.Views
{
    public partial class RootView : CandyWindow
    {
        public RootView()
        {
            InitializeComponent();
            HeaderViewModel HeaderView = new HeaderViewModel();
            HeaderView.Handler = new Dictionary<string, int>
            {
                { "CogOutline", (int)SysFuncEnum.Setting },
                { "ArrowCollapse", (int)SysFuncEnum.MinSize },
                { "PowerStandby", (int)SysFuncEnum.Close }
            };
            Header.DataContext = HeaderView;

        }

        private void WindowMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
