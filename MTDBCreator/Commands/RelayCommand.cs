using System;
using System.Diagnostics;
using System.Windows.Input;

namespace MTDBCreator.Commands
{
    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> m_execute;
        readonly Predicate<object> m_canExecute;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action<object> execute) : this(execute, null) { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null) throw new ArgumentNullException("execute"); m_execute = execute; m_canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return (m_canExecute == null || m_canExecute(parameter));
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            m_execute(parameter);
        }

        #endregion // ICommand Members }
    }
}
