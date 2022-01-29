using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.CandyWindows.CnadyWinViewModel
{
    public class CandyDPlayViewModel:Screen
    {
        private readonly IContainer Container;
        public CandyDPlayViewModel(IContainer Container)
        {
            this.Container = Container;
        }
    }
}
