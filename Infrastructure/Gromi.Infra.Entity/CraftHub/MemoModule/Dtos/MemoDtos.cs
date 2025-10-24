using Gromi.Infra.Entity.Common.Enums;

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
    }
}