using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.UserControlViews.UserViews
{
    public class UserViewModel : Screen
    {
        private readonly IContainer Container;
        public UserViewModel(IContainer Container)
        {
            this.Container = Container;
        }
    }
}
