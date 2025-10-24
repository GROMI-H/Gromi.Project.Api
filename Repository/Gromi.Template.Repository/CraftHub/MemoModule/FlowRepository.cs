using Gromi.Infra.Entity.Common.Attributes;
using Gromi.Infra.Entity.Common.Enums;
using Gromi.Infra.Repository.DbEntity.CraftHub.MemoModule;
using Gromi.Infra.Repository.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Repository.CraftHub.MemoModule
{
    /// <summary>
    /// 流程内容仓储接口
    /// </summary>
    public interface IFlowRepository : IRepository<FlowItem>
    {
    }

    /// <summary>
    /// 流程内容仓储实现
    /// </summary>
    [AutoInject(ServiceLifetime.Scoped, "Memo")]
    public class FlowRepository : BaseRepository<FlowItem>, IFlowRepository
    {
        public FlowRepository(IMultiFreeSqlManager<DbKey> multiFreeSql) : base(multiFreeSql, DbKey.DbCraftHub)
        {
        }
    }
}