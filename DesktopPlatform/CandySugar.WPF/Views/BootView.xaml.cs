using CandySugar.Controls.UserControls;
using HandyControl.Tools.Extension;
using SDKCore;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CandySugar.WPF.Views
{
    /// <summary>
    /// BootView.xaml 的交互逻辑
    /// </summary>
    public partial class BootView : CandyWindow
    {
        public BootView()
        {
            InitializeComponent();
        }

        private void WindowMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CandyClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private async void CompletedEvents(object sender, RoutedEventArgs e)
        {
            if(Account.Text.IsNullOrEmpty())
                Msg.Text = "请输入账号！";

            var module = License.Register(new LicenseModel
            {
                Account = Account.Text,
                PassWord =PassWord.Password,
            });
            if (!module)Msg.Text = "通行证错误！";
            else
            {
                Msg.Text = "通行证正确！1秒后关闭窗口";
                await Task.Delay(1000);
                DialogResult = true;
            }
        }
    }
}
