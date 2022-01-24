using MaterialDesignThemes.Wpf;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.UserControlView
{
    public class HeaderViewModel : Screen
    {
        private Dictionary<string, int> _Handler;
        public Dictionary<string, int> Handler
        {
            get { return _Handler; }
            set { SetAndNotify(ref _Handler, value); }
        }
    }
}
