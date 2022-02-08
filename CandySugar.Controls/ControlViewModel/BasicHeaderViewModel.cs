using CandySugar.Common.Enum;
using Stylet;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CandySugar.Controls.ControlViewModel
{
    public class BasicHeaderViewModel : Screen
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
        public BasicHeaderViewModel Basic()
        {
            Handler = new Dictionary<string, Tuple<Thickness, SysFuncEnum>>
            {
                { "CogOutline", new Tuple<Thickness, SysFuncEnum>( new Thickness(0,0,0,0),SysFuncEnum.Setting) },
                { "ArrowCollapse",new Tuple<Thickness, SysFuncEnum>( new Thickness(10,0,0,0), SysFuncEnum.MinSize) },
                { "PowerStandby", new Tuple<Thickness, SysFuncEnum>( new Thickness(10,0,0,0),SysFuncEnum.ShutDown) }
            };
            return this;
        }
    }
}
