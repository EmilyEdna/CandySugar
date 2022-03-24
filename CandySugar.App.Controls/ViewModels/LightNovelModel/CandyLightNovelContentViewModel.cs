using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XExten.Advance.InternalFramework.Securities.Common;
using XExten.Advance.StaticFramework;

namespace CandySugar.App.Controls.ViewModels.LightNovelModel
{
    public class CandyLightNovelContentViewModel : ViewModelNavigatBase
    {
        public CandyLightNovelContentViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.TextTheme = Color.Black;
            this.Theme = Color.FromHex("DDCDA1");

        }

        #region Property
        private ObservableCollection<string> _Content;
        public ObservableCollection<string> Content
        {
            get => _Content;
            set => SetProperty(ref _Content, value);
        }

        private string _ChapterName;
        public string ChapterName
        {
            get => _ChapterName;
            set => SetProperty(ref _ChapterName, value);
        }

        private Color _Theme;
        public Color Theme
        {
            get { return _Theme; }
            set { SetProperty(ref _Theme, value); }
        }
        private Color _TextTheme;
        public Color TextTheme
        {
            get { return _TextTheme; }
            set { SetProperty(ref _TextTheme, value); }
        }
        #endregion

        #region Command
        public ICommand ThemeCommand => new DelegateCommand<dynamic>(input =>
        {
            if (input != null)
            {
                if ((Color)input == Color.Black)
                    this.TextTheme = Color.White;
                else
                    this.TextTheme = Color.Black;
                this.Theme = (Color)input;
            }
        });
        #endregion
        public override void Initialize(INavigationParameters parameters)
        {
            var res = SyncStatic.Decompress(parameters.GetValue<string>("Content"), SecurityType.Base64);
            ChapterName = parameters.GetValue<string>("ChapterName");
            var data =  res.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(t => $"\r\n\t\t\t\t{t}");
            Content = new ObservableCollection<string>(data);
        }
    }
}
