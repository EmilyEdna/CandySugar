using Stylet;

namespace CandySugar.Upgrade.ViewModels
{
    public class RootViewModel : PropertyChangedBase
    {
        private string _title = "HandyControl Application";
        public string Title
        {
            get { return _title; }
            set { SetAndNotify(ref _title, value); }
        }
        //https://gitee.com/Mefelia/CandySugar/raw/master/File.zip
        public RootViewModel()
        {

        }
    }
}
