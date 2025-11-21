using FluentValidation;
using Gromi.Infra.Entity.Common.SystemModule.Dtos;

namespace Gromi.Application.Validator.Common.SystemModule
{
    public class PageRouteValidator : AbstractValidator<PageRouteDto>
    {
        public PageRouteValidator()
        {
            RuleFor(page => page.Name).NotEmpty().WithMessage("页面名称不能为空");
            RuleFor(page => page.Path).NotEmpty().WithMessage("页面路径不能为空");
        }
    }
}