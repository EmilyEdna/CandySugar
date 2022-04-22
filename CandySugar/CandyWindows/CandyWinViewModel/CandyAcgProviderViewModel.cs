using SDKColloction.AcgAnimeSDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CandySugar.CandyWindows.CandyWinViewModel
{
    public class CandyAcgProviderViewModel: Screen
    {
        private readonly IContainer Container;
        public CandyAcgProviderViewModel(IContainer Container)
        {
            this.Container = Container;
        }

        #region Property
        private ObservableCollection<AcgAnimeTagsResult> _TagResult;
        public ObservableCollection<AcgAnimeTagsResult> TagResult
        {
            get => _TagResult;
            set => SetAndNotify(ref _TagResult, value);
        }
        private AcgAnimeBrandsResult _BrandResult;
        public AcgAnimeBrandsResult BrandResult
        {
            get => _BrandResult;
            set => SetAndNotify(ref _BrandResult, value);
        }
        public ObservableCollection<string> _HType;
        public ObservableCollection<string> HType
        {
            get => _HType;
            set => SetAndNotify(ref _HType, value);
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
