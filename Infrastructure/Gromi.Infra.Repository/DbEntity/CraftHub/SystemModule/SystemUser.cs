using FreeSql.DataAnnotations;
using Gromi.Infra.Entity.Common.Attributes;
using Gromi.Infra.Entity.Common.Dtos;
using Gromi.Infra.Repository.DbEntity.CraftHub.MemoModule;

namespace Gromi.Infra.Repository.DbEntity.CraftHub.SystemModule
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Table(Name = "sys_user_info")]
    public class UserInfo : BaseEntity
    {
        #region 字段

        /// <summary>
        /// 主键Id
        /// </summary>
        [Column(IsPrimary = true)]
        [Snowflake]
        public override long Id { get; set; }

        #endregion 字段

        #region 关联

        /// <summary>
        /// 笔记记录
        /// </summary>
        public virtual ICollection<NoteRecord> Notes { get; set; } = new List<NoteRecord>();

        /// <summary>
        /// 标签集合
        /// </summary>
        public virtual ICollection<NoteTag> Tags { get; set; } = new HashSet<NoteTag>();

        #endregion 关联
    }
}