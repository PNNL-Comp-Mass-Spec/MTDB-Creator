using System;
using System.Windows.Input;

namespace MTDBCreator
{
    public class DelegateCommand : ICommand
    {
        private bool m_canExecute;

        public DelegateCommand(Action actionToTake)
        {
            m_canExecute = true;
            m_action = actionToTake;
        }
        public bool CanExecute(object parameter)
        {
            return m_canExecute;
        }

        public bool Executable
        {
            set
            {
                m_canExecute = value;
                if (CanExecuteChanged != null)
                    CanExecuteChanged(this, null);
            }
        }

        public event EventHandler CanExecuteChanged;
        private Action m_action;

        public void Execute(object parameter)
        {
            m_action();
        }
    }
}
