using CandySugar.App.Controls.LayoutView;
using CandySugar.App.Controls.LayoutView.LayoutViewModel;
using CandySugar.App.Controls.ViewModels.MusicModel;
using CandySugar.Xam.Common;
using CandySugar.Xam.Common.Enum;
using Music.SDK.ViewModel.Response;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CandySugar.App.Controls.Views.Music
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandyMusicView : ContentPage
    {
        public CandyMusicView()
        {
            InitializeComponent();
            Flag = 0;
            Menu.Source = ImageSource.FromFile("repeat.png");
        }
        private int Flag;
        private void ChangedClick(object sender, EventArgs e)
        {
            if (Flag == 0)
            {
                Menu.Source = ImageSource.FromFile("repeat2.png");
                Flag = 1;
            }
            else
            {
                Menu.Source = ImageSource.FromFile("repeat.png");
                Flag = 0;
            }
        }

        private void PopupOpened(object sender, EventArgs e)
        {
            PopPlayList();
        }
        private void PopupSheetOpened(object sender, EventArgs e)
        {
            PopSheet((MusicSongSheetItem)(sender as Button).CommandParameter);
        }
        private void PopupAlbumOpened(object sender, EventArgs e)
        {
            PopAlbum((MusicSongItem)(sender as Button).CommandParameter);
        }

        private void PopPlayList()
        {
            var HeaderView = new PopPlayHeaderView();
            var ContentView = new PopPlayContentView();
            (ContentView.BindingContext as PopPlayContentViewModel).PopPlayHeaderView = HeaderView;
            PopCommon(HeaderView, ContentView);
            Pop.Show(20, 300);
        }
        private void PopSheet(MusicSongSheetItem input)
        {
            var HeaderView = new PopSheetHeaderView();
            ((PopSheetHeaderViewModel)HeaderView.BindingContext).Title = input.SongSheetName;
            var ContentView = new PopSheetContentView();
            ((PopSheetContentViewModel)ContentView.BindingContext).SearchSheet(input, ((CandyMusicViewModel)BindingContext).Platform);
            PopCommon(HeaderView, ContentView);
            Pop.Show(20, 300);
        }
        private void PopAlbum(MusicSongItem input)
        {
            var HeaderView = new PopAlbumHeaderView();
            ((PopAlbumHeaderViewModel)HeaderView.BindingContext).Title = input.SongAlbumName;
            var ContentView = new PopAlbumContentView();
            ((PopAlbumContentViewModel)ContentView.BindingContext).SearchAlbum(input, ((CandyMusicViewModel)BindingContext).Platform);
            PopCommon(HeaderView, ContentView);
            Pop.Show(20, 300);
        }
        private void PopCommon(ContentView HeaderView, ContentView ContentView)
        {
            Pop.PopupView.PopupStyle = new PopupStyle
            {
                CornerRadius = 45,
            };
            Pop.PopupView.HeaderTemplate = new DataTemplate(() => HeaderView);
            Pop.PopupView.ShowCloseButton = false;
            Pop.PopupView.ShowFooter = false;
            Pop.PopupView.MinimumWidthRequest = Soft.ScreenWidth - 40;
            Pop.PopupView.MinimumHeightRequest = Soft.ScreenHeight / 2;
            Pop.PopupView.AnimationEasing = AnimationEasing.SinIn;
            Pop.PopupView.AnimationMode = AnimationMode.SlideOnBottom;
            Pop.PopupView.ContentTemplate = new DataTemplate(() => ContentView);
        }

        private void PauseClick(object sender, EventArgs e)
        {
           
        }

        private async void PlayClick(object sender, EventArgs e)
        {
            var  data = await (this.BindingContext as CandyMusicViewModel).Query();

            if (Flag == 0)
            {
                data.Select(t=>t.CacheAddress)
            }
        }
    }
}