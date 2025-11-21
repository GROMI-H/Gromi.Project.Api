using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule;
using Gromi.Infra.DataAccess.Shared;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Repository.Common.SystemModule
{
    /// <summary>
    /// 页面仓储
    /// </summary>
    public interface IPageRepository : IRepository<PageRoute>
    {
        /// <summary>
        /// 添加页面路由
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<bool> InsertPageRouteAsync(PageRoute param);
    }

    [AutoInject(ServiceLifetime.Scoped)]
    public class PageRepository : BaseRepository<PageRoute>, IPageRepository
    {
        public PageRepository(IMultiFreeSqlManager<DbKey> multiFreeSql) : base(multiFreeSql, DbKey.DbCraftHub)
        {
        }

        public async Task<bool> InsertPageRouteAsync(PageRoute param)
        {
            var repo = _fsql.GetRepository<PageRoute>();
            repo.DbContextOptions.EnableCascadeSave = true;

            var res = await repo.InsertAsync(param);
            return res != null;
        }
    }
}