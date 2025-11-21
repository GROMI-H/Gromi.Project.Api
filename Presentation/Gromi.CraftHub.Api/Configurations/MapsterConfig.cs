using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule;
using Gromi.Infra.Entity.Common.SystemModule.Dtos;
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

            #region PageRouteDto <=> PageRoute

            TypeAdapterConfig<PageRoute, PageRouteDto>.NewConfig()
                .Map(dest => dest.Meta.Title, src => src.MetaTitle)
                .Map(dest => dest.Meta.Order, src => src.MetaOrder)
                .Map(dest => dest.Meta.NoBasicLayout, src => src.MetaNoBasicLayout)
                .Map(dest => dest.Meta.AffixTab, src => src.MetaAffixTab)
                .Map(dest => dest.Children, src => src.Children);

            TypeAdapterConfig<PageRouteDto, PageRoute>.NewConfig()
                .Map(dest => dest.MetaTitle, src => src.Meta.Title)
                .Map(dest => dest.MetaOrder, src => src.Meta.Order)
                .Map(dest => dest.MetaAffixTab, src => src.Meta.AffixTab)
                .Map(dest => dest.MetaNoBasicLayout, src => src.Meta.NoBasicLayout)
                .Map(dest => dest.Children, src => src.Children);

            #endregion PageRouteDto <=> PageRoute

            #endregion 自定义映射配置
        }
    }
}