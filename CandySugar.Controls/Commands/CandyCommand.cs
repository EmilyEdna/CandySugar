using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CandySugar.Controls.Commands
{
    public class CandyCommand:ICommand
    {
        private  Action<object> _Execute;
        private  Func<bool> _CanExecute;

        public CandyCommand(Action<object> Execute, Func<bool> CanExecute)
        {
            _Execute = Execute;
            _CanExecute = CanExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_CanExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (_CanExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }
        public bool CanExecute(object parameter)
        {
            return _CanExecute == null ? true : _CanExecute();
        }

        public void Execute(object parameter)
        {
            _Execute(parameter);
        }
    }
}
