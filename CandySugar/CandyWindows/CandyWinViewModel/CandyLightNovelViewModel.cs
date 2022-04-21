using CandySugar.Controls.Commands;
using SDKColloction.LightNovelSDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CandySugar.CandyWindows.CandyWinViewModel
{
    public class CandyLightNovelViewModel: Screen
    {
        private readonly IContainer Container;
        public CandyLightNovelViewModel(IContainer Container)
        {
            this.Container = Container;
            this.FontSize = 22;
        }

        #region Property
        private LightNovelContentResult _LightNovelContent;
        public LightNovelContentResult LightNovelContent
        {
            get { return _LightNovelContent; }
            set { SetAndNotify(ref _LightNovelContent, value); }
        }
        private bool _Show;
        public bool Show
        {
            get { return _Show; }
            set { SetAndNotify(ref _Show, value); }
        }
        private int _FontSize;
        public int FontSize
        {
            get { return _FontSize; }
            set { SetAndNotify(ref _FontSize, value); }
        }
        private Visibility _Loading;
        public Visibility Loading
        {
            get { return _Loading; }
            set { SetAndNotify(ref _Loading, value); }
        }
        #endregion

        #region Method

        public ICommand SliderChange => new CandyCommand(args =>
        {
            FontSize = (int)args;
        }, null);
        #endregion
    }
}
