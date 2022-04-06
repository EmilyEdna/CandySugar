using CandySugar.Xam.Common.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Prism.Ioc;
using CandySugar.Xam.Core.Service;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;

namespace CandySugar.App.Controls.LayoutView.LayoutViewModel
{
    public class PopContentViewModel : ViewModelBase
    {
        private IYYLiShi Candy;

        #region Property

        private ObservableCollection<CandyYYLiShiDto> _Yinyue;
        public ObservableCollection<CandyYYLiShiDto> Yinyue
        {
            get => _Yinyue;
            set => SetProperty(ref _Yinyue, value);
        }

        #endregion

        protected override void OnViewLaunch()
        {
            Candy = ContainerLocator.Container.Resolve<IYYLiShi>();
            Query();
        }

        #region Command
        public ICommand RemoveCommand => new DelegateCommand<CandyYYLiShiDto>(input =>
        {
            if (input != null)
                Delete(input.PId);
        });
        #endregion

        #region Method
        public async void Query()
        {
            var data = await Candy.GetPlayList();
            Yinyue = new ObservableCollection<CandyYYLiShiDto>(data);
        }
        public async void Delete(Guid Id)
        {
            await Candy.RemovePlayList(Id);
            Query();
            ContainerLocator.Container.Resolve<PopHeaderViewModel>().Refresh();
        }
        #endregion
    }
}
