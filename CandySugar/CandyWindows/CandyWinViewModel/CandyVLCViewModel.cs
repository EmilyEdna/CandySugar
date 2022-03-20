using Anime.SDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CandySugar.CandyWindows.CandyWinViewModel
{
    public class CandyVLCViewModel:Screen
    {
        private readonly IContainer Container;
        public CandyVLCViewModel(IContainer Container)
        {
            this.Container = Container;
        }
        #region Property
        private AnimePlayResult _WatchResult;
        public AnimePlayResult WatchResult
        {
            get { return _WatchResult; }
            set { SetAndNotify(ref _WatchResult, value); }
        }
        private Visibility _Loading;
        public Visibility Loading
        {
            get { return _Loading; }
            set { SetAndNotify(ref _Loading, value); }
        }
        #endregion
    }
}
