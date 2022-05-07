using CandySugar.Common.WinDTO;
using CandySugar.WPF.Properties;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.WPF.CandyWindows.CandyWinViewModel
{
    public class CandySettingViewModel : Screen
    {
        private IContainer Container;
        public CandySettingViewModel(IContainer Container)
        {
            this.Container = Container;
            Root = Soft.Default.ToJson().ToModel<GlobalOption>();
        }
        private GlobalOption _Root;
        public GlobalOption Root
        {
            get { return _Root; }
            set { SetAndNotify(ref _Root, value); }
        }
    }
}
