using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Common.Navigations
{
    public interface IActiveNotify
    {
        void NavigateTo(IScreen screen);
    }
}
