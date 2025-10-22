using FreeSql.DataAnnotations;
using Gromi.Infra.Entity.Common.Attributes;
using Gromi.Infra.Entity.Common.Dtos;
using Gromi.Infra.Repository.DbEntity.CraftHub.SystemModule;

namespace Gromi.Infra.Repository.DbEntity.CraftHub.MemoModule
{
    /// <summary>
    /// 笔记标签
    /// </summary>
    [Table(Name = "memo_note_tag")]
    public class NoteTag : BaseEntity
    {
        #region 字段

        /// <summary>
        /// 标签主键
        /// </summary>
        [Column(IsPrimary = true)]
        [Snowflake]
        public override long Id { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        [Column(StringLength = 25)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        #endregion 字段

        #region 关联

        [Navigate(nameof(UserId))]
        public virtual UserInfo User { get; set; }

        /// <summary>
        /// 笔记
        /// </summary>
        public virtual ICollection<NoteRecord> Notes { get; set; } = new List<NoteRecord>();

        #endregion 关联
    }
}