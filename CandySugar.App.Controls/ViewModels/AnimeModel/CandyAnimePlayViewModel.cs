using Prism.Navigation;

namespace CandySugar.App.Controls.ViewModels.AnimeModel
{
    public class CandyAnimePlayViewModel : ViewModelNavigatBase
    {
        public CandyAnimePlayViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region Property
        private string _PlayURL;
        public string PlayURL
        {
            get { return _PlayURL; }
            set { SetProperty(ref _PlayURL, value); }
        }
        #endregion

        #region Override
        public override void Initialize(INavigationParameters parameters)
        {
            PlayURL = parameters.GetValue<string>("WatchAddress");
        }
        #endregion
    }
}
