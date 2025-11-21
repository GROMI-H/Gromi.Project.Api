using FreeSql.DataAnnotations;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.SystemModule.Enums;

namespace Gromi.Infra.DataAccess.DbEntity.Common.SystemModule
{
    /// <summary>
    /// 系统权限码
    /// </summary>
    [Table(Name = "sys_access_code")]
    public class AccessCode : BaseEntity
    {
        /// <summary>
        /// 权限码类型
        /// </summary>
        public AccessCodeEnum Type { get; set; } = AccessCodeEnum.Button;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 权限码
        /// </summary>
        public string Code { get; set; }
    }
}