using FluentValidation;
using Gromi.Infra.Entity.CraftHub.MemoModule.Dtos;
using Gromi.Infra.Entity.CraftHub.MemoModule.Enums;

namespace Gromi.Application.Validator.CraftHub.MemoModule
{
    /// <summary>
    /// 笔记记录校验器
    /// </summary>
    public class NoteRecordValidator : AbstractValidator<NoteRecordDto>
    {
        public NoteRecordValidator()
        {
            RuleFor(record => record.Name).NotEmpty().WithMessage("名称不能为空");
            RuleFor(record => record.TagId).NotEmpty().NotEqual(0).WithMessage("标签ID不能为空");
            RuleFor(record => record.FlowItems).NotEmpty().When(record => record.Type == NoteType.Flow).WithMessage("流程内容为空");
            RuleFor(record => record.Account).NotEmpty().When(record => record.Type == NoteType.Password).WithMessage("账号不能为空");
            RuleFor(record => record.Content).NotEmpty().When(record => record.Type != NoteType.Flow).WithMessage("内容不能为空");
        }
    }
}