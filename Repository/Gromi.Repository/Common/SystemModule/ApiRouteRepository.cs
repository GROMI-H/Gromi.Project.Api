using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule;
using Gromi.Infra.DataAccess.Shared;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Repository.Common.SystemModule
{
    /// <summary>
    /// 接口路由仓储接口
    /// </summary>
    public interface IApiRouteRepository : IRepository<ApiRoute>
    {
        /// <summary>
        /// 添加更新接口路由
        /// </summary>
        /// <param name="apiRoutes"></param>
        /// <returns></returns>
        Task<bool> UpsertApiRouteAsync(IEnumerable<ApiRoute> apiRoutes);

        /// <summary>
        /// 清理软删除
        /// </summary>
        /// <returns></returns>
        Task<bool> ClearSoftDelAsync();
    }

    /// <summary>
    /// 接口路由仓储实现
    /// </summary>
    [AutoInject(ServiceLifetime.Scoped)]
    public class ApiRouteRepository : BaseRepository<ApiRoute>, IApiRouteRepository
    {
        public ApiRouteRepository(IMultiFreeSqlManager<DbKey> multiFreeSql) : base(multiFreeSql, DbKey.DbCraftHub)
        {
        }

        public async Task<bool> UpsertApiRouteAsync(IEnumerable<ApiRoute> apiRoutes)
        {
            using (var uow = _fsql.CreateUnitOfWork())
            {
                try
                {
                    // 全部设置为已删除
                    await uow.Orm.GetRepository<ApiRoute>().UpdateDiy
                        .Set(api => api.IsDeleted, DeleteEnum.Deleted)
                        .Where(api => api.IsDeleted != DeleteEnum.Deleted)
                        .ExecuteAffrowsAsync();

                    var curApis = await uow.Orm.GetRepository<ApiRoute>().Select.ToListAsync();

                    // 筛选出apiRoutes中curApis中不存在的记录
                    var newApis = apiRoutes.Where(api => !curApis.Select(item => item.Route).Contains(api.Route));

                    // 将apiRoutes和数据库都存在的记录设置未删除
                    await uow.Orm.GetRepository<ApiRoute>().UpdateDiy
                        .Set(api => api.IsDeleted, DeleteEnum.NotDeleted)
                        .Where(api => apiRoutes.Select(item => item.Route).Contains(api.Route))
                        .ExecuteAffrowsAsync();

                    // 插入新添加的路径
                    if (newApis.Any())
                    {
                        await uow.Orm.GetRepository<ApiRoute>().InsertAsync(newApis);
                    }

                    uow.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    throw new Exception($"ApiRoute更新失败：{ex.Message}");
                }
            }
        }

        public async Task<bool> ClearSoftDelAsync()
        {
            // TODO "冷静"天数采用配置
            var res = await _fsql.Delete<ApiRoute>()
                .Where(api => api.IsDeleted == DeleteEnum.Deleted && (DateTime.Now - api.UpdateTime).TotalDays > 15)
                .ExecuteAffrowsAsync();
            return res > 0;
        }
    }
}