using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.UserControlViews.MangaViews
{
    public class MangaViewModel : Screen
    {
        private readonly IContainer Container;
        public MangaViewModel(IContainer Container)
        {
            this.Container = Container;
        }
    }
}
