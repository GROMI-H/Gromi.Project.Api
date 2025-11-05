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
    { }

    [AutoInject(ServiceLifetime.Scoped)]
    public class RoleRepository : BaseRepository<SystemRole>, IRoleRepository
    {
        public RoleRepository(IMultiFreeSqlManager<DbKey> multiFreeSql) : base(multiFreeSql, DbKey.DbCraftHub)
        {
        }
    }
}