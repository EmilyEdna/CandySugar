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
        private Dictionary<string, Tuple<Thickness, SysFuncEnum>> _Handler;
        public Dictionary<string, Tuple<Thickness, SysFuncEnum>> Handler
        {
            get { return _Handler; }
            set { SetAndNotify(ref _Handler, value); }
        }

        public HeaderViewModel Baisc()
        {
            Handler = new Dictionary<string, Tuple<Thickness, SysFuncEnum>>
            {
                { "CogOutline", new Tuple<Thickness, SysFuncEnum>( new Thickness(0,0,0,0),SysFuncEnum.Setting) },
                { "ArrowCollapse",new Tuple<Thickness, SysFuncEnum>( new Thickness(10,0,0,0), SysFuncEnum.MinSize) },
                { "PowerStandby", new Tuple<Thickness, SysFuncEnum>( new Thickness(10,0,0,0),SysFuncEnum.Close) }
            };
            return this;
        }
    }
}
