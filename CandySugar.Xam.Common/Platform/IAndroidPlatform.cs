using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Xam.Common.Platform
{
    public interface IAndroidPlatform
    {
        void HiddenStatusBar();
        void ShowStatusBar();
        string DirPath();
    }
}
