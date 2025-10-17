using FreeSql.DataAnnotations;
using Gromi.Infra.Entity.Common.Attributes;

namespace Gromi.Infra.Entity.Common.Dtos
{
    /// <summary>
    /// 基础实体
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [Column(IsPrimary = true)]
        [Snowflake]
        public virtual long Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(CanUpdate = false)]
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }
    }
}