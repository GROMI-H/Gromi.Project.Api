using FreeSql.DataAnnotations;
using Gromi.Infra.DataAccess.DbEntity.CraftHub.MemoModule;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;

namespace Gromi.Infra.DataAccess.DbEntity.Common.SystemModule
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Table(Name = "sys_user")]
    [Index("uk_account", "Account", true)]
    public class UserInfo : BaseEntity
    {
        #region 字段

        /// <summary>
        /// 主键Id
        /// </summary>
        [Column(IsPrimary = true)]
        [Snowflake]
        public override long Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Column(StringLength = 25)]
        public string Account { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Column(StringLength = 10)]
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column(StringLength = 50)]
        public string Password { get; set; }

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