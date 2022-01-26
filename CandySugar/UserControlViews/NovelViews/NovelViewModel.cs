using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.UserControlViews.NovelViews
{
    public class NovelViewModel : Screen
    {
        private readonly IContainer Container;
        public NovelViewModel(IContainer Container)
        {
            this.Container = Container;
        }
    }
}
