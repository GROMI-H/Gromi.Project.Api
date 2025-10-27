using FreeSql.DataAnnotations;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;

namespace Gromi.Infra.DataAccess.DbEntity.Common.SystemModule
{
    /// <summary>
    /// 系统组件
    /// </summary>
    [Table(Name = "sys_component")]
    public class SystemComponent : BaseEntity
    {
        #region 字段

        [Column(IsPrimary = true)]
        [Snowflake]
        public override long Id { get; set; }

        /// <summary>
        /// 组件名称
        /// </summary>
        [Column(StringLength = 25)]
        public string Name { get; set; }

        /// <summary>
        /// 组件描述
        /// </summary>
        [Column(StringLength = 255)]
        public string Description { get; set; }

        #endregion 字段
    }
}