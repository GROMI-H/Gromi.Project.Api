using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule;
using Gromi.Infra.DataAccess.Shared;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Repository.Common.SystemModule
{
    /// <summary>
    /// 系统角色仓储
    /// </summary>
    public interface IRoleRepository : IRepository<SystemRole>
    {
        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> BatchDeleteRoleAsync(List<long> ids);
    }

    [AutoInject(ServiceLifetime.Scoped)]
    public class RoleRepository : BaseRepository<SystemRole>, IRoleRepository
    {
        public RoleRepository(IMultiFreeSqlManager<DbKey> multiFreeSql) : base(multiFreeSql, DbKey.DbCraftHub)
        {
        }

        public async Task<bool> BatchDeleteRoleAsync(List<long> ids)
        {
            using (var uow = _fsql.CreateUnitOfWork())
            {
                try
                {
                    var repo = _fsql.GetRepository<SystemRole>();

                    var groups = await repo.Select
                        .IncludeMany(a => a.UsersRoles)
                        .Where(b => ids.Contains(b.Id))
                        .ToListAsync();

                    var delRes = await repo.DeleteAsync(groups);
                    uow.Commit();
                    return delRes > 0;
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}