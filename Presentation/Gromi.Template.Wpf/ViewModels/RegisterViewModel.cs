using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.Common.LoginModule.Params;
using Gromi.Template.Wpf.Infrastructure.Common;
using Gromi.Template.Wpf.Infrastructure.Services;
using HandyControl.Controls;
using System.ComponentModel;
using System.Windows.Input;

namespace Gromi.Template.Wpf.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        #region 初始化

        private readonly UserService _userService = new UserService();

        public RegisterViewModel()
        {
            Register = new RelayCommand(execute: _ => ExecuteRegisterClickCommand(), canExecute: _ => CanExecuteRegisterClickCommand());
        }

        #endregion 初始化

        #region 属性

        public RegisterParam _model = new RegisterParam();

        /// <summary>
        /// 注册参数
        /// </summary>
        public RegisterParam Model
        {
            get { return _model; }
            set
            {
                _model = value;
                RaisePropertyChanged(nameof(Model));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion 属性

        #region 命令

        public ICommand Register { get; }

        private async void ExecuteRegisterClickCommand()
        {
            var res = await _userService.Register(_model);
            if (res.Code == ResponseCodeEnum.Success)
            {
                Growl.Success(res.Msg);
            }
            else
            {
                Growl.Error(res.Msg);
            }
        }

        private bool CanExecuteRegisterClickCommand()
        {
            return true;
        }

        #endregion 命令
    }
}