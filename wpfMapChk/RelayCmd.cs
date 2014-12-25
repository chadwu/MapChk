using System;
using System.Diagnostics;
using System.Windows.Input;

namespace wpfMapChk
{
    class RelayCmd : ICommand
    {
        readonly Func<bool> _canExecute;
        readonly Action<string> _execute;

        public RelayCmd(Action<string> execute)
            : this(execute, null)
        {
        }

        public RelayCmd(Action<string> execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        //[DebuggerStepThrough]
        public Boolean CanExecute(Object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute(parameter as string);
        }
    }
}
