using CandySugar.Common.Enum;
using CandySugar.Controls.Commands;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Controls.ControlViewModel
{
    public class HeaderViewModel : CandyCommand
    {
        private Dictionary<string, int> _Handler;
        public Dictionary<string, int> Handler
        {
            get { return _Handler; }
            set { SetAndNotify(ref _Handler, value); }
        }

        public HeaderViewModel Baisc()
        {
            Handler  = new Dictionary<string, int>
            {
                { "CogOutline", (int)SysFuncEnum.Setting },
                { "ArrowCollapse", (int)SysFuncEnum.MinSize },
                { "PowerStandby", (int)SysFuncEnum.Close }
            };
            return this;
        }
    }
}
