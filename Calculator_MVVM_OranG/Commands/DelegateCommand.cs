using System;
using System.Windows.Input;

namespace Calculator_MVVM_OranG.Commands
{
    class DelegateCommand : ICommand
    {
        private Action<string> _action;

        public DelegateCommand(Action<string> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string operation = (string)parameter;
            _action(operation);
        }
        public event EventHandler CanExecuteChanged;
    }
}
