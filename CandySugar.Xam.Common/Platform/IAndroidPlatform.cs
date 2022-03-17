using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common.Platform
{
    public interface IAndroidPlatform
    {
        void HiddenStatusBar();
        void ShowStatusBar();
    }
}
