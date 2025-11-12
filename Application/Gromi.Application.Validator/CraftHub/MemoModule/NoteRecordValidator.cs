using FluentValidation;
using Gromi.Infra.Entity.CraftHub.MemoModule.Dtos;

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
        }
    }
}