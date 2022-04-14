using Prism.Mvvm;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;


namespace CandySugar.App.Controls
{
    public class ViewModelNavigatBase : ViewModelRepository, IInitialize, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelNavigatBase(INavigationService navigationService):base()
        {
            NavigationService = navigationService;

            OnViewLaunch();
        }

        protected virtual void OnViewLaunch()
        {

        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {
        }
    }
}
