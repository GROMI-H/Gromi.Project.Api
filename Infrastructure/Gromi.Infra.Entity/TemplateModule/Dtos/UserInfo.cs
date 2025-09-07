using FreeSql.DataAnnotations;
using Gromi.Infra.Entity.CommonModule.Attributes;

namespace Gromi.Infra.Entity.TemplateModule.Dtos
{
    /// <summary>
    /// 用户信息，Demo
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Column(IsPrimary = true)]
        [Snowflake]
        public long Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Column(StringLength = 25)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}