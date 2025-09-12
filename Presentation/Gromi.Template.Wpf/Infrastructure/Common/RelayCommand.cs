using System.Windows.Input;

namespace Gromi.Template.Wpf.Infrastructure.Common
{
    /// <summary>
    /// 处理命令逻辑
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// 执行命令时要调用的委托
        /// </summary>
        private readonly Action<object?>? _execute;

        /// <summary>
        /// 确定命令是否可以执行的逻辑
        /// </summary>
        private readonly Func<object?, bool>? _canExecute;

        /// <summary>
        /// 用来通知命令源是否需要重新查询命令的执行状态
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// 确定命令是否可以在指定参数下执行
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// 执行命令，调用_execute委托
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object? parameter)
        {
            _execute?.Invoke(parameter);
        }
    }
}