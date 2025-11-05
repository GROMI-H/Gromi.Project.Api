using Mapster;

namespace Gromi.CraftHub.Api.Configurations
{
    /// <summary>
    /// Mapster映射配置
    /// </summary>
    public static class MapsterConfig
    {
        /// <summary>
        /// 添加Mapster映射配置
        /// </summary>
        /// <param name="services"></param>
        public static void AddMapsterConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 全局配置
            TypeAdapterConfig.GlobalSettings.Default
                .NameMatchingStrategy(NameMatchingStrategy.Flexible) //灵活匹配策略
                .PreserveReference(true); // 保留对象引用

            #region 自定义映射配置

            // 覆盖全局配置

            #endregion 自定义映射配置
        }
    }
}