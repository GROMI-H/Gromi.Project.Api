using FluentValidation;
using Gromi.Infra.Entity.CraftHub.MemoModule.Dtos;

namespace Gromi.Application.Validator.CraftHub.MemoModule
{
    /// <summary>
    /// 笔记标签校验器
    /// </summary>
    public class NoteTagValidator : AbstractValidator<NoteTagDto>
    {
        public NoteTagValidator()
        {
            RuleFor(tag => tag.Name).NotEmpty().WithMessage("名称不能为空");
            RuleFor(tag => tag.UserId).NotEqual(0).WithMessage("用户信息异常");
        }
    }
}