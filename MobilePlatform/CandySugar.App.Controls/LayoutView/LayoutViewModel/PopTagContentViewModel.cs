using SDKColloction.AcgAnimeSDK.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CandySugar.App.Controls.LayoutView.LayoutViewModel
{
    public class PopTagContentViewModel: ViewModelBase
    {

        private ObservableCollection<AcgAnimeTagsResult> _TagResult;
        public ObservableCollection<AcgAnimeTagsResult> TagResult
        {
            get => _TagResult;
            set => SetProperty(ref _TagResult, value);
        }

        private AcgAnimeBrandsResult _BrandResult;
        public AcgAnimeBrandsResult BrandResult
        {
            get => _BrandResult;
            set => SetProperty(ref _BrandResult, value);
        }

        public ObservableCollection<string> _HType;
        public ObservableCollection<string> HType
        {
            get => _HType;
            set => SetProperty(ref _HType, value);
        }
    }
}
