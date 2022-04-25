using CandySugar.App.Controls;
using CandySugar.App.Views;
using Prism.Commands;
using Prism.Navigation;
using SDKCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace CandySugar.App.ViewModels
{
    public class CandyLoginViewModel : ViewModelNavigatBase
    {
        public CandyLoginViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region Property
        private string _Account;
        public string Account
        {
            get => _Account;
            set => SetProperty(ref _Account, value);
        }
        private string _PassWord;
        public string PassWord
        {
            get => _PassWord;
            set => SetProperty(ref _PassWord, value);
        }
        #endregion

        #region Command
        public ICommand LoginCommand => new DelegateCommand(() =>
         {
             Login();
         });
        #endregion

        #region Method
        public async void Login()
        {
            var IsLogin = License.Register(new LicenseModel
            {
                Account = Account,
                PassWord = PassWord
            });

            if (IsLogin)
                Application.Current.MainPage = new CandyIndexView();
            else
                using (await MaterialDialog.Instance.LoadingSnackbarAsync("请检查账号和密码是否正确"))
                {
                    await Task.Delay(3000);
                }
        }
        #endregion
    }
}
