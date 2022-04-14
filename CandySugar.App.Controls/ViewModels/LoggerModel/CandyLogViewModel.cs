using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Prism.Ioc;
using CandySugar.Xam.Core.Service;
using System.Collections.ObjectModel;
using CandySugar.Xam.Common.DTO;
using System.Windows.Input;
using Prism.Commands;

namespace CandySugar.App.Controls.ViewModels.LoggerModel
{

    public class CandyLogViewModel : ViewModelNavigatBase
    {
        private ILoger CandyLog;
        public CandyLogViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        private ObservableCollection<CandyGlobalLogDto> _LogResult;
        public ObservableCollection<CandyGlobalLogDto> LogResult
        {
            get => _LogResult;
            set => SetProperty(ref _LogResult, value);
        }

        protected override void OnViewLaunch()
        {
            CandyLog = Resolve<ILoger>();
            Query();
        }

        public ICommand RemoveCommand => new DelegateCommand<CandyGlobalLogDto>(input =>
        {
            CandyLog.Delete(input);
            Query();
        });

        public async void Query()
        {
            var data = await CandyLog.Query();
            LogResult = new ObservableCollection<CandyGlobalLogDto>(data);
        }

    }
}
