using Gromi.Infra.DataAccess.DbEntity.CraftHub.MemoModule;
using Gromi.Infra.DataAccess.Shared;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Repository.CraftHub.MemoModule
{
    /// <summary>
    /// 笔记记录仓储接口
    /// </summary>
    public interface INoteRecordRepository : IRepository<NoteRecord>
    {
        /// <summary>
        /// 删除记录 - 软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<OperationResEnum> DeleteNoteRecordAsync(List<long> ids);
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

        public async Task<OperationResEnum> DeleteNoteRecordAsync(List<long> ids)
        {
            var res = await _fsql.GetRepository<NoteRecord>().UpdateDiy
                .Set(entiy => entiy.Status, DeleteEnum.Deleted)
                .Where(entiy => ids.Contains(entiy.Id))
                .ExecuteAffrowsAsync();
            return res > 0 ? OperationResEnum.Success : OperationResEnum.Fail;
        }
    }
}