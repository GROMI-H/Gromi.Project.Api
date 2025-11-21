using FreeSql.DataAnnotations;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;

namespace Gromi.Infra.DataAccess.DbEntity.Common.SystemModule
{
    /// <summary>
    /// 页面路由
    /// </summary>
    [Table(Name = "sys_page")]
    public class PageRoute : BaseEntity
    {
        #region 元数据

        /// <summary>
        /// 排序
        /// </summary>
        public string MetaOrder { get; set; }

        /// <summary>
        /// 标题名称
        /// </summary>
        public string MetaTitle { get; set; }

        /// <summary>
        /// 无基础框架
        /// </summary>
        public string MetaNoBasicLayout { get; set; }

        /// <summary>
        /// TODO 待补充
        /// </summary>
        public string MetaAffixTab { get; set; }

        #endregion 元数据

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 重定向
        /// </summary>
        public string Redirect { get; set; }

        /// <summary>
        /// 组件
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// 父菜单ID,顶级菜单为-1
        /// </summary>
        public long Pid { get; set; } = -1;

        /// <summary>
        /// 子菜单
        /// </summary>
        [Navigate(nameof(Pid))]
        public List<PageRoute> Children { get; set; } = new List<PageRoute>();
    }
}