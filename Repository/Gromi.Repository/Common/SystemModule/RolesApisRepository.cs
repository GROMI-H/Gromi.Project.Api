using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule.Relations;
using Gromi.Infra.DataAccess.Shared;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Repository.Common.SystemModule
{
    /// <summary>
    /// 角色接口关联表
    /// </summary>
    public interface IRolesApisRepository
    {
        /// <summary>
        /// 添加关联
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<OperationResEnum> InsertRolesApisAsync(List<RolesApis> param);

        /// <summary>
        /// 删除关联
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<OperationResEnum> DeleteRolesApisAsync(List<RolesApis> param);

        /// <summary>
        /// 验证Url是否有效
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<bool> VerifyUrlAsync(List<long> roleIds, string url);
    }

    [AutoInject(ServiceLifetime.Scoped)]
    public class RolesApisRepository : IRolesApisRepository
    {
        private readonly IFreeSql _fsql;

        public RolesApisRepository(IMultiFreeSqlManager<DbKey> multiFreeSql)
        {
            _fsql = multiFreeSql.Get(DbKey.DbCraftHub);
        }

        public async Task<OperationResEnum> InsertRolesApisAsync(List<RolesApis> param)
        {
            await _fsql.GetRepository<RolesApis>().InsertAsync(param);
            return OperationResEnum.Success;
        }

        public async Task<OperationResEnum> DeleteRolesApisAsync(List<RolesApis> param)
        {
            await _fsql.GetRepository<RolesApis>().DeleteAsync(param);
            return OperationResEnum.Success;
        }

        public async Task<bool> VerifyUrlAsync(List<long> roleIds, string url)
        {
            var exists = await _fsql.Select<RolesApis>()
                .Include(x => x.Api)
                .Where(x => roleIds.Contains(x.RoleId) && url.EndsWith(x.Api.Route))
                .AnyAsync();
            return exists;
        }
    }
}