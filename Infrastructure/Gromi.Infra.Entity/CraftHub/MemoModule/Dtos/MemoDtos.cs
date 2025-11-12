using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.CraftHub.MemoModule.Enums;

namespace Gromi.Infra.Entity.CraftHub.MemoModule.Dtos
{
    /// <summary>
    /// 笔记标签Dto
    /// </summary>
    public class NoteTagDto
    {
        public long Id { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 标签描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 是否删除
        /// </summary>
        public DeleteEnum IsDeleted { get; set; } = DeleteEnum.NotDeleted;

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
    }

    /// <summary>
    /// 笔记记录Dto
    /// </summary>
    public class NoteRecordDto
    {
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public NoteType Type { get; set; } = NoteType.Default;

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DeleteEnum Status { get; set; } = DeleteEnum.NotDeleted;

        /// <summary>
        /// 标签Id
        /// </summary>
        public long TagId { get; set; }
    }
}