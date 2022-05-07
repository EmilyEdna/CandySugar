using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CandySugar.App.Controls.ViewModels.LightNovelModel
{
    public class CandyLightNovelImageViewModel : ViewModelNavigatBase
    {
        public CandyLightNovelImageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        private ObservableCollection<string> _Image;
        public ObservableCollection<string> Image
        {
            get => _Image;
            set => SetProperty(ref _Image, value);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            Image = new ObservableCollection<string>(parameters.GetValue<List<string>>("Image"));
        }
    }
}
