using Gromi.Template.Wpf.Infrastructure.Common;
using Gromi.Template.Wpf.Infrastructure.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace Gromi.Template.Wpf.ViewModels
{
    public class TestViewModel : INotifyPropertyChanged
    {
        #region 初始化

        public TestViewModel()
        {
            Add = new RelayCommand(execute: _ => ExecuteAddClickCommand(), canExecute: _ => CanExecuteAddClickCommand());
            Sub = new RelayCommand(execute: _ => ExecuteSubClickCommand(), canExecute: _ => CanExecuteSubClickCommand());
        }

        #endregion 初始化

        #region 属性

        private TestModel _model = new TestModel();

        public double OperData_One
        {
            get { return _model.OperData_One; }
            set
            {
                _model.OperData_One = value;
                RaisePropertyChanged(nameof(OperData_One));
            }
        }

        public double OperData_Two
        {
            get { return _model.OperData_Two; }
            set
            {
                _model.OperData_Two = value;
                RaisePropertyChanged(nameof(OperData_Two));
            }
        }

        public double Result
        {
            get { return _model.Result; }
            set
            {
                _model.Result = value;
                RaisePropertyChanged(nameof(Result));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion 属性

        #region 命令

        #region Add

        public ICommand Add { get; }

        private void ExecuteAddClickCommand()
        {
            Result = OperData_One + OperData_Two;
        }

        private bool CanExecuteAddClickCommand()
        {
            return true;
        }

        #endregion Add

        #region Sub

        public ICommand Sub { get; }

        private void ExecuteSubClickCommand()
        {
            Result = OperData_One - OperData_Two;
        }

        private bool CanExecuteSubClickCommand()
        {
            return true;
        }

        #endregion Sub

        #endregion 命令
    }
}