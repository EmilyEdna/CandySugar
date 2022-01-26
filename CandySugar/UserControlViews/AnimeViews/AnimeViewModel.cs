using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.UserControlViews.AnimeViews
{
    public class AnimeViewModel:Screen
    {
        private readonly IContainer Container;
        public AnimeViewModel(IContainer Container)
        {
            this.Container = Container;
        }
    }
}
