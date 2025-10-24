using Gromi.Infra.Entity.Common.Attributes;
using Gromi.Infra.Entity.Common.Enums;
using Gromi.Infra.Repository.DbEntity.CraftHub.MemoModule;
using Gromi.Infra.Repository.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Repository.CraftHub.MemoModule
{
    /// <summary>
    /// 笔记记录仓储接口
    /// </summary>
    public interface INoteRecordRepository : IRepository<NoteRecord>
    {
    }

    /// <summary>
    /// 笔记记录仓储实现
    /// </summary>
    [AutoInject(ServiceLifetime.Scoped, "Memo")]
    public class NoteRecordRepository : BaseRepository<NoteRecord>, INoteRecordRepository
    {
        public NoteRecordRepository(IMultiFreeSqlManager<DbKey> multiFreeSql) : base(multiFreeSql, DbKey.DbCraftHub)
        {
        }
    }
}