using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.CandyWindows.CandyWinViewModel
{
    public class CandyLyricViewModel:Screen
    {
        private readonly IContainer Container;
        public CandyLyricViewModel(IContainer Container)
        {
            this.Container = Container;
            this.Lyric = "歌词准备中...";
        }

        #region Property
        private string _Lyric;
        public string Lyric
        {
            get { return _Lyric; }
            set { SetAndNotify(ref _Lyric, value); }
        }
        #endregion
    }
}
