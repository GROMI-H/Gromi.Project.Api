using FreeSql.DataAnnotations;
using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.CraftHub.MemoModule.Enums;

namespace Gromi.Infra.DataAccess.DbEntity.CraftHub.MemoModule
{
    /// <summary>
    /// 笔记记录
    /// </summary>
    [Table(Name = "memo_note_record")]
    public class NoteRecord : BaseEntity
    {
        #region 字段

        /// <summary>
        /// 主键ID
        /// </summary>
        [Column(IsPrimary = true)]
        [Snowflake]
        public override long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(StringLength = 25)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 笔记类型
        /// </summary>
        [Column(MapType = typeof(int))]
        public NoteType Type { get; set; } = NoteType.Default;

        /// <summary>
        /// 账号 - Type=NoteType.Password时使用
        /// </summary>
        [Column(StringLength = 25)]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 内容，存放密码或者一般内容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 是否删除
        /// </summary>
        [Column(MapType = typeof(int))]
        public DeleteEnum Status { get; set; } = DeleteEnum.NotDeleted;

        /// <summary>
        /// 标签Id
        /// </summary>
        public long TagId { get; set; }

        #endregion 字段

        #region 关联

        /// <summary>
        /// 标签(导航属性)
        /// </summary>
        [Navigate(nameof(TagId))]
        public virtual NoteTag Tag { get; set; }

        /// <summary>
        /// 流程(导航属性)
        /// </summary>
        public virtual ICollection<FlowItem> FlowItems { get; set; } = new List<FlowItem>();

        #endregion 关联
    }
}