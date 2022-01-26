using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.UserControlViews.WallpaperViews
{
    public class WallpaperViewModel : Screen
    {
        private readonly IContainer Container;
        public WallpaperViewModel(IContainer Container)
        {
            this.Container = Container;
        }
    }
}
