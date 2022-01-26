using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.UserControlViews.MusicViews
{
    public class MusicViewModel : Screen
    {
        private readonly IContainer Container;
        public MusicViewModel(IContainer Container)
        {
            this.Container = Container;
        }
    }
}
