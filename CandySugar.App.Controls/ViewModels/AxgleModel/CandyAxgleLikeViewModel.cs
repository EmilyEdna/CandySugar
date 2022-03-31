using CandySugar.Xam.Core.Service;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Prism.Ioc;
using CandySugar.Xam.Common.DTO;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using CandySugar.App.Controls.Views.Axgle;
using XExten.Advance.LinqFramework;

namespace CandySugar.App.Controls.ViewModels.AxgleModel
{
    public class CandyAxgleLikeViewModel : ViewModelNavigatBase
    {
        public CandyAxgleLikeViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        #region Field
        private IAXLiShi Candy;
        #endregion

        #region  Property
        private string _KeyWord;
        public string KeyWord
        {
            get => _KeyWord;
            set => SetProperty(ref _KeyWord, value);
        }

        private int _PageIndex;
        public int PageIndex
        {
            get => _PageIndex;
            set => SetProperty(ref _PageIndex, value);
        }

        private int _Total;
        public int Total
        {
            get => _Total;
            set => SetProperty(ref _Total, value);
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { SetProperty(ref _IsBusy, value); }
        }

        private double _Blur;
        public double Blur
        {
            get => _Blur;
            set => SetProperty(ref _Blur, value);
        }

        private ObservableCollection<CandyAXLiShiDto> _LikeData;
        public ObservableCollection<CandyAXLiShiDto> LikeData
        {
            get => _LikeData;
            set => SetProperty(ref _LikeData, value);
        }
        #endregion

        #region Override
        protected override void OnViewLaunch()
        {
            this.PageIndex = 1;
            Candy = ContainerLocator.Container.Resolve<IAXLiShi>();
            Query();
        }
        #endregion

        #region Method
        public async void Query()
        {
            IsBusy = true;
            var res = await Candy.Query(KeyWord, PageIndex, 10);
            LikeData = new ObservableCollection<CandyAXLiShiDto>(res.Item1);
            Total = res.Item2;
            IsBusy = false;
        }
        public async void Delete(CandyAXLiShiDto input)
        {
            await Candy.Remove(input);
        }
        public async void Navigation(string input)
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Route", input);
            await NavigationService.NavigateAsync(new Uri(nameof(CandyAxglePlayView), UriKind.Relative), param);
        }
        #endregion

        #region Command
        public ICommand RemoveCommand => new DelegateCommand<CandyAXLiShiDto>(input =>
        {
            Delete(input);
            this.PageIndex = 1;
            Query();
        });
        public ICommand ShowMoreCommand => new DelegateCommand(() =>
        {
            PageIndex += 1;
            if (PageIndex <= Total)
                Query();
        });
        public ICommand SearchCommand => new DelegateCommand<string>(input =>
        {
            this.KeyWord = input;
            this.PageIndex = 1;
            Query();
        });
        public ICommand PlayCommand => new DelegateCommand<string>(input =>
        {
            if (!input.IsNullOrEmpty())
                Navigation(input);
        });
        #endregion
    }
}
