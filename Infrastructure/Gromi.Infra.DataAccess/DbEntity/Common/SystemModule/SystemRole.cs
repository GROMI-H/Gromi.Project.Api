using FreeSql.DataAnnotations;
using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule.Relations;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using System.ComponentModel;

namespace Gromi.Infra.DataAccess.DbEntity.Common.SystemModule
{
    /// <summary>
    /// 系统角色表
    /// </summary>
    [Table(Name = "sys_role")]
    public class SystemRole : BaseEntity
    {
        #region 字段

        /// <summary>
        /// 主键ID
        /// </summary>
        [Column(IsPrimary = true)]
        [Snowflake]
        public override long Id { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        [Column(StringLength = 25)]
        public string Code { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Column(StringLength = 25)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 角色描述
        /// </summary>
        [Column(StringLength = 255)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 角色状态
        /// </summary>
        [Column(MapType = typeof(int))]
        public StatusEnum Enable { get; set; } = StatusEnum.Enabled;

        #endregion 字段

        #region 关联

        /// <summary>
        /// 角色用户关联集合
        /// </summary>
        public virtual ICollection<UsersRoles> UsersRoles { get; set; } = new List<UsersRoles>();

        /// <summary>
        /// User集合
        /// </summary>
        public virtual ICollection<UserInfo> Users => UsersRoles?.Select(ur => ur.User).ToList();

        /// <summary>
        /// 角色接口关联集合
        /// </summary>
        public List<RolesApis> RolesApis { get; set; } = new List<RolesApis>();

        /// <summary>
        /// 接口集合
        /// </summary>
        public virtual ICollection<ApiRoute> Apis => RolesApis?.Select(ra => ra.Api).ToList();

        #endregion 关联
    }
}