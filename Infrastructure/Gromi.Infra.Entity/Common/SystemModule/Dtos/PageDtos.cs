namespace Gromi.Infra.Entity.Common.SystemModule.Dtos
{
    /// <summary>
    /// 页面路由
    /// </summary>
    public class PageRouteDto
    {
        /// <summary>
        /// 元数据信息
        /// </summary>
        public MetaInfoDto Meta { get; set; } = new MetaInfoDto();

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
        /// 子菜单
        /// </summary>
        public List<PageRouteDto> Children { get; set; } = new List<PageRouteDto>();
    }

    /// <summary>
    /// 元数据信息
    /// </summary>
    public class MetaInfoDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// 无基础框架
        /// </summary>
        public string NoBasicLayout { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string AffixTab { get; set; }
    }
}