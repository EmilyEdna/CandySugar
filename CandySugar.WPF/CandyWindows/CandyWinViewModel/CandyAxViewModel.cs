using SDKColloction.AnimeSDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CandySugar.WPF.CandyWindows.CandyWinViewModel
{
    public class CandyAxViewModel : Screen
    {
        private readonly IContainer Container;
        public CandyAxViewModel(IContainer Container)
        {
            this.Container = Container;
        }
        #region Property
        private string _Watch;
        public string Watch
        {
            get { return _Watch; }
            set { SetAndNotify(ref _Watch, value); }
        }
        private Visibility _Loading;
        public Visibility Loading
        {
            get { return _Loading; }
            set { SetAndNotify(ref _Loading, value); }
        }
        #endregion
    }
}
