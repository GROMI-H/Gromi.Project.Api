using Gromi.Infra.Entity.Common.BaseModule.Enums;

namespace Gromi.Infra.Entity.Common.SystemModule.Dtos
{
    /// <summary>
    /// Api路由Dto
    /// </summary>
    public class ApiRouteDto
    {
        public long Id { get; set; }

        /// <summary>
        /// 路由名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 路由类型
        /// </summary>
        public ApiTypeEnum RouteType { get; set; } = ApiTypeEnum.GET;

        /// <summary>
        /// 路由描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 路由内容
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary
        public DeleteEnum IsDeleted { get; set; } = DeleteEnum.NotDeleted;

        public DateTime CreateTime { get; set; }
    }
}