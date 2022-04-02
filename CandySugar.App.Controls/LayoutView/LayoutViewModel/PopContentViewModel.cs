using CandySugar.Xam.Common.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Prism.Ioc;
using CandySugar.Xam.Core.Service;
using System.Threading.Tasks;

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

        protected override  void OnViewLaunchAsync()
        {
            Candy = ContainerLocator.Container.Resolve<IYYLiShi>();
            //await Candy.GetPlayList()
            Yinyue = new ObservableCollection<CandyYYLiShiDto>();
            Yinyue.Add(new CandyYYLiShiDto
            {
                Address = "",
                CacheAddress = "",
                SongAlbum = "张三",
                SongArtist = "李四,mikejson",
                SongName = "With Me(孤勇者翻唱版本)",
                Platform = 1,
                SongId = "",
            });
            Yinyue.Add(new CandyYYLiShiDto
            {
                Address = "",
                CacheAddress = "",
                SongAlbum = "张三",
                SongArtist = "李四,mikejson",
                SongName = "With Me(孤勇者翻唱版本)",
                Platform = 1,
                SongId = "",
            });
        }
    }
}
