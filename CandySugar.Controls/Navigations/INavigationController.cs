using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Common.Navigations
{
    public interface INavigationController
    {
        IActiveNotify Delegate { get; set; }
    }
}
