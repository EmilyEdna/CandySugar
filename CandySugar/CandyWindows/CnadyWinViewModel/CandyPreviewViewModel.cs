using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.CandyWindows.CnadyWinViewModel
{
    public class CandyPreviewViewModel: Screen
    {
        #region Property
        private string _FileURL;
        public string FileURL
        {
            get { return _FileURL; }
            set { SetAndNotify(ref _FileURL, value); }
        }
        #endregion
    }
}
