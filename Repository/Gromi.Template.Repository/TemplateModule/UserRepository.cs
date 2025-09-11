using Gromi.Infra.Entity.CommonModule.Attributes;
using Gromi.Infra.Entity.CommonModule.Enums;
using Gromi.Infra.Entity.TemplateModule.Dtos;
using Gromi.Infra.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Repository.TemplateModule
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