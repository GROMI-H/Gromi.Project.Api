using FreeSql.DataAnnotations;
using Gromi.Infra.Entity.Common.Attributes;
using Gromi.Infra.Entity.Common.Dtos;

namespace Gromi.Infra.Repository.DbEntity.CraftHub.SystemModule
{
    /// <summary>
    /// 系统配置实体
    /// </summary>
    public class SystemSetting : BaseEntity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Column(IsPrimary = true)]
        [Snowflake]
        public override long Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        public string SettingKey { get; set; } = string.Empty;

        /// <summary>
        /// 配置内容
        /// </summary>
        public string SettingValue { get; set; } = string.Empty;
    }
}