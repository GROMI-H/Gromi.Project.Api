using FreeSql.DataAnnotations;
using Gromi.Infra.Entity.Common.Dtos;
using Gromi.Infra.Entity.Common.Enums;

namespace Gromi.Infra.Repository.DbEntity.CraftHub.SystemModule
{
    /// <summary>
    /// 系统配置实体
    /// </summary>
    public class SystemSetting : BaseEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        [Column(DbType = "varchar(20)")]
        public string SettingKey { get; set; } = string.Empty;

        /// <summary>
        /// 配置内容
        /// </summary>
        [Column(DbType = "nvarchar(50)")]
        public string SettingValue { get; set; } = string.Empty;

        /// <summary>
        /// 是否启用：0-未启用，1-启用
        /// </summary>
        [Column(MapType = typeof(int))]
        public StatusEnum Enable { get; set; } = StatusEnum.Unknown;
    }
}