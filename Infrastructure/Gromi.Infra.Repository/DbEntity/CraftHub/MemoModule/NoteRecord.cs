using FreeSql.DataAnnotations;
using Gromi.Infra.Entity.Common.Attributes;
using Gromi.Infra.Entity.Common.Dtos;
using Gromi.Infra.Entity.Common.Enums;
using Gromi.Infra.Entity.CraftHub.MemoModule.Enums;
using Gromi.Infra.Repository.DbEntity.CraftHub.SystemModule;

namespace Gromi.Infra.Repository.DbEntity.CraftHub.MemoModule
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
        /// 是否删除
        /// </summary>
        [Column(MapType = typeof(int))]
        public DeleteEnum Status { get; set; } = DeleteEnum.NotDeleted;

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 标签Id
        /// </summary>
        public long TagId { get; set; }

        #endregion 字段

        #region 关联

        /// <summary>
        /// 用户信息(导航属性)
        /// </summary>
        [Navigate(nameof(UserId))]
        public virtual UserInfo User { get; set; }

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