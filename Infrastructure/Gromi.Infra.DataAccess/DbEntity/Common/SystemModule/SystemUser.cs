using FreeSql.DataAnnotations;
using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule.Relations;
using Gromi.Infra.DataAccess.DbEntity.CraftHub.MemoModule;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using System.Data;

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
        public string UserName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column(StringLength = 50)]
        public string Password { get; set; }

        /// <summary>
        /// 盐
        /// </summary>
        [Column(StringLength = 50)]
        public string Salt { get; set; }

        #endregion 字段

        #region 关联

        /// <summary>
        /// 笔记记录
        /// </summary>
        public virtual ICollection<NoteRecord> Notes { get; set; } = new List<NoteRecord>();

        /// <summary>
        /// 标签集合
        /// </summary>
        public virtual ICollection<NoteTag> Tags { get; set; } = new List<NoteTag>();

        /// <summary>
        /// 用户角色关系集合
        /// </summary>
        public virtual ICollection<UsersRoles> UsersRoles { get; set; } = new List<UsersRoles>();

        /// <summary>
        /// 角色集合
        /// </summary>
        public virtual ICollection<SystemRole> Roles => UsersRoles?.Select(ur => ur.Role).ToList();

        #endregion 关联
    }
}