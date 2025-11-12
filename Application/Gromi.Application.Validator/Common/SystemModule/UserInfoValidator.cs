using FluentValidation;
using Gromi.Infra.Entity.Common.AuthModule.Params;
using Gromi.Infra.Entity.Common.SystemModule.Dtos;

namespace Gromi.Application.Validator.Common.SystemModule
{
    /// <summary>
    /// 用户信息校验器
    /// </summary>
    public class UserInfoValidator : AbstractValidator<UserInfoDto>
    {
        public UserInfoValidator()
        {
        }
    }

    public class RegisterValidator : AbstractValidator<RegisterParam>
    {
        public RegisterValidator()
        {
            RuleFor(reg => reg.Name).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(reg => reg.Account)
                .NotEmpty().WithMessage("账号不能为空")
                .Length(5, 15).WithMessage("账号长度必须在5到15位之间");
            RuleFor(reg => reg.Password)
                .NotEmpty().WithMessage("密码不能为空")
                .Length(6, 12).WithMessage("密码长度必须在6到12位之间");
        }
    }
}