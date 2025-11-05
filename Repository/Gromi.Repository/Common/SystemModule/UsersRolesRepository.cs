using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule.Relations;
using Gromi.Infra.DataAccess.Shared;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Repository.Common.SystemModule
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    public interface IUsersRolesRepository
    {
        /// <summary>
        /// 添加关联
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<OperationResEnum> InsertUsersRolesAsync(List<UsersRoles> param);

        /// <summary>
        /// 删除关联
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<OperationResEnum> DeleteUsersRolesAsync(List<UsersRoles> param);
    }

    [AutoInject(ServiceLifetime.Scoped)]
    public class UsersRolesRepository : IUsersRolesRepository
    {
        private readonly IFreeSql _fsql;

        public UsersRolesRepository(IMultiFreeSqlManager<DbKey> multiFreeSql)
        {
            _fsql = multiFreeSql.Get(DbKey.DbCraftHub);
        }

        public async Task<OperationResEnum> InsertUsersRolesAsync(List<UsersRoles> param)
        {
            await _fsql.GetRepository<UsersRoles>().InsertAsync(param);
            return OperationResEnum.Success;
        }

        public async Task<OperationResEnum> DeleteUsersRolesAsync(List<UsersRoles> param)
        {
            await _fsql.GetRepository<UsersRoles>().DeleteAsync(param);
            return OperationResEnum.Success;
        }
    }
}