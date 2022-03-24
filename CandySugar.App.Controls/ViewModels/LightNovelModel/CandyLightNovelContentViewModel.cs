using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.Advance.InternalFramework.Securities.Common;
using XExten.Advance.StaticFramework;

namespace CandySugar.App.Controls.ViewModels.LightNovelModel
{
    public class CandyLightNovelContentViewModel : ViewModelNavigatBase
    {
        public CandyLightNovelContentViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
        private string _Content;
        public string Content
        {
            get => _Content;
            set => SetProperty(ref _Content, value);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            Content = SyncStatic.Decompress(parameters.GetValue<string>("Image"), SecurityType.Base64);
        }
    }
}
