using System;
using System.Windows.Input;

namespace RegistryMonitorWPF.Helpers
{
    /// <summary>
    /// Универсальная реализация команды для MVVM, поддерживающая параметры.
    /// </summary>
    /// <typeparam name="T">Тип параметра, передаваемого в команду.</typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// Создает новую команду.
        /// </summary>
        /// <param name="execute">Действие, выполняемое при вызове команды.</param>
        /// <param name="canExecute">Функция, определяющая, можно ли выполнить команду.</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Определяет, можно ли выполнить команду.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>true, если команду можно выполнить; иначе false.</returns>
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute((T)parameter);

        /// <summary>
        /// Выполняет команду.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object parameter) => _execute((T)parameter);

        /// <summary>
        /// Событие, вызываемое при изменении возможности выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    /// <summary>
    /// Универсальная реализация команды для MVVM без параметров.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Создает новую команду.
        /// </summary>
        /// <param name="execute">Действие, выполняемое при вызове команды.</param>
        /// <param name="canExecute">Функция, определяющая, можно ли выполнить команду.</param>
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Определяет, можно ли выполнить команду.
        /// </summary>
        /// <param name="parameter">Не используется.</param>
        /// <returns>true, если команду можно выполнить; иначе false.</returns>
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        /// <summary>
        /// Выполняет команду.
        /// </summary>
        /// <param name="parameter">Не используется.</param>
        public void Execute(object parameter) => _execute();

        /// <summary>
        /// Событие, вызываемое при изменении возможности выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
