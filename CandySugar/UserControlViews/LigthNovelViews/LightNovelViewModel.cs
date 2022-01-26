using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.UserControlViews.LigthNovelViews
{
    public class LightNovelViewModel : Screen
    {
        private readonly IContainer Container;
        public LightNovelViewModel(IContainer Container)
        {
            this.Container = Container;
        }
    }
}
