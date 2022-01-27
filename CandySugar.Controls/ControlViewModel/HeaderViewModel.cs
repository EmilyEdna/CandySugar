using CandySugar.Common.Enum;
using CandySugar.Controls.Commands;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CandySugar.Controls.ControlViewModel
{
    public class HeaderViewModel : Screen
    {
        private Dictionary<string, Tuple<Thickness, SysFuncEnum>> _Handler;
        public Dictionary<string, Tuple<Thickness, SysFuncEnum>> Handler
        {
            get { return _Handler; }
            set { SetAndNotify(ref _Handler, value); }
        }

        private bool _Loading;
        public bool Loading
        {
            get { return _Loading; }
            set { SetAndNotify(ref _Loading, value); }
        }

        public HeaderViewModel Baisc()
        {
            Handler = new Dictionary<string, Tuple<Thickness, SysFuncEnum>>
            {
                { "CogOutline", new Tuple<Thickness, SysFuncEnum>( new Thickness(0,0,0,0),SysFuncEnum.Setting) },
                { "ArrowCollapse",new Tuple<Thickness, SysFuncEnum>( new Thickness(10,0,0,0), SysFuncEnum.MinSize) },
                { "PowerStandby", new Tuple<Thickness, SysFuncEnum>( new Thickness(10,0,0,0),SysFuncEnum.Close) }
            };
            Loading = false;
            return this;
        }
        public HeaderViewModel Reader()
        {
            Handler = new Dictionary<string, Tuple<Thickness, SysFuncEnum>>
            {
                { "CloudDownloadOutline", new Tuple<Thickness, SysFuncEnum>( new Thickness(0,0,0,0),SysFuncEnum.Download) },
                { "ArrowCollapse",new Tuple<Thickness, SysFuncEnum>( new Thickness(10,0,0,0), SysFuncEnum.MinSize) },
                { "PowerStandby", new Tuple<Thickness, SysFuncEnum>( new Thickness(10,0,0,0),SysFuncEnum.Close) }
            };
            Loading = false;
            return this;
        }
        public void LoadState(bool loading) 
        {
            Loading = loading;
        }
    }
}
