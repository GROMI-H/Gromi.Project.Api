using FreeSql.DataAnnotations;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;

namespace Gromi.Infra.DataAccess.DbEntity.CraftHub.MemoModule
{
    /// <summary>
    /// 流程单项
    /// </summary>
    [Table(Name = "memo_flow_item")]
    public class FlowItem : BaseEntity
    {
        #region 字段

        /// <summary>
        /// 单项主键
        /// </summary>
        [Column(IsPrimary = true)]
        [Snowflake]
        public override long Id { get; set; }

        /// <summary>
        /// 单项内容
        /// </summary>
        [Column(StringLength = 50)]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 序列号
        /// </summary>
        public int SortIndex { get; set; }

        /// <summary>
        /// 笔记记录ID
        /// </summary>
        public long RecordId { get; set; }

        #endregion 字段

        #region 关联

        /// <summary>
        /// 笔记记录(导航属性)
        /// </summary>
        [Navigate(nameof(RecordId))]
        public virtual NoteRecord Note { get; set; }

        #endregion 关联
    }
}