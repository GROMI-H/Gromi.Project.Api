using Gromi.Infra.Entity.Common.BaseModule.Enums;

namespace Gromi.Infra.Entity.Common.SystemModule.Dtos
{
    /// <summary>
    /// 系统角色Dto
    /// </summary>
    public class SystemRoleDto
    {
        public long Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 角色状态
        /// </summary>
        public StatusEnum Enable { get; set; }
    }
}