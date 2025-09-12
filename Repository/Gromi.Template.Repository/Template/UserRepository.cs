using Gromi.Infra.Entity.Common.Attributes;
using Gromi.Infra.Entity.Common.Enums;
using Gromi.Infra.Repository.DbEntity.Template;
using Gromi.Infra.Repository.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Repository.Template
{
    /// <summary>
    /// 用户仓储接口
    /// </summary>
    public interface IUserRepository : IRepository<UserInfo>
    {
    }

    /// <summary>
    /// 用户仓储实现
    /// </summary>
    [AutoInject(ServiceLifetime.Scoped, "Temp")]
    public class UserRepository : BaseRepository<UserInfo>, IUserRepository
    {
        public UserRepository(IMultiFreeSqlManager<DbKey> multiFreeSql) : base(multiFreeSql, DbKey.DbTemp)
        {
        }
    }
}