using FreeSql.DataAnnotations;
using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule.Relations;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using System.ComponentModel;

namespace Gromi.Infra.DataAccess.DbEntity.Common.SystemModule
{
    /// <summary>
    /// 系统接口路由表
    /// </summary>
    [Table(Name = "sys_route")]
    [Index("uk_route", "Route", true)]
    public class ApiRoute : BaseEntity
    {
        [Column(IsPrimary = true)]
        [Snowflake]
        public override long Id { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        [Column(StringLength = 25)]
        [Description("控制器名称")]
        public string Name { get; set; }

        /// <summary>
        /// 接口类型
        /// </summary>
        [Column(MapType = typeof(int))]
        [Description("接口类型")]
        public ApiTypeEnum RouteType { get; set; } = ApiTypeEnum.GET;

        /// <summary>
        /// 接口描述
        /// </summary>
        [Column(StringLength = 25)]
        [Description("接口描述")]
        public string Description { get; set; }

        /// <summary>
        /// 接口内容
        /// </summary>
        [Column(StringLength = 25)]
        [Description("接口内容")]
        public string Route { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        [Column(MapType = typeof(int))]
        [Description("删除状态")]
        public DeleteEnum IsDeleted { get; set; } = DeleteEnum.NotDeleted;

        /// <summary>
        /// 角色接口关联集合
        /// </summary>
        public virtual ICollection<RolesApis> RolesApis { get; set; }

        /// <summary>
        /// 接口集合
        /// </summary>
        public virtual ICollection<SystemRole> Roles => RolesApis?.Select(ra => ra.Role).ToList();
    }
}