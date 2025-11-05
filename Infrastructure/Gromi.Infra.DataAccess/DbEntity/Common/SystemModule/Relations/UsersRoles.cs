using FreeSql.DataAnnotations;

namespace Gromi.Infra.DataAccess.DbEntity.Common.SystemModule.Relations
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    [Table(Name = "sys_users_roles")]
    public class UsersRoles
    {
        [Column(IsPrimary = true)]
        public long UserId { get; set; }

        [Column(IsPrimary = true)]
        public long RoleId { get; set; }

        /// <summary>
        /// 导航属性 User
        /// </summary>
        [Navigate(nameof(UserId))]
        public virtual UserInfo User { get; set; }

        /// <summary>
        /// 导航属性 Role
        /// </summary>
        [Navigate(nameof(RoleId))]
        public virtual SystemRole Role { get; set; }
    }
}