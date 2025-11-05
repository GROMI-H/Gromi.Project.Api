using FreeSql.DataAnnotations;

namespace Gromi.Infra.DataAccess.DbEntity.Common.SystemModule.Relations
{
    /// <summary>
    /// 角色Api关联表
    /// </summary>
    [Table(Name = "sys_roles_apis")]
    public class RolesApis
    {
        [Column(IsPrimary = true)]
        public long RoleId { get; set; }

        [Column(IsPrimary = true)]
        public long ApiId { get; set; }

        /// <summary>
        /// 导航属性 Role
        /// </summary>
        [Navigate(nameof(RoleId))]
        public virtual SystemRole Role { get; set; }

        /// <summary>
        /// 导航属性 ApiRoute
        /// </summary>
        [Navigate(nameof(ApiId))]
        public virtual ApiRoute Api { get; set; }
    }
}