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
    public class HeaderViewModel : CandyCommand
    {
        private Dictionary<string, Tuple<Thickness, int>> _Handler;
        public Dictionary<string, Tuple<Thickness, int>> Handler
        {
            get { return _Handler; }
            set { SetAndNotify(ref _Handler, value); }
        }

        public HeaderViewModel Baisc()
        {
            Handler = new Dictionary<string, Tuple<Thickness, int>>
            {
                { "CogOutline", new Tuple<Thickness, int>( new Thickness(0,0,0,0),(int)SysFuncEnum.Setting) },
                { "ArrowCollapse",new Tuple<Thickness, int>( new Thickness(10,0,0,0), (int)SysFuncEnum.MinSize) },
                { "PowerStandby", new Tuple<Thickness, int>( new Thickness(10,0,0,0),(int)SysFuncEnum.Close) }
            };
            return this;
        }
    }
}
