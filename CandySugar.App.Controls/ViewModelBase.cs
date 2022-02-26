using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.App.Controls
{
    public class ViewModelBase: BindableBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}
