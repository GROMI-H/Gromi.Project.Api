using FreeSql.DataAnnotations;
using Gromi.Infra.Entity.Common.Dtos;
using Gromi.Infra.Entity.Common.Attributes;

namespace Gromi.Infra.Repository.DbEntity.Template
{
    /// <summary>
    /// 用户信息，Demo
    /// </summary>
    public class UserInfo : BaseEntity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Column(IsPrimary = true)]
        [Snowflake]
        public override long Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Column(StringLength = 25)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
    }
}