using Gromi.Infra.DataAccess.DbEntity.CraftHub.MemoModule;
using Gromi.Infra.DataAccess.Shared;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Repository.CraftHub.MemoModule
{
    /// <summary>
    /// 笔记标签仓储接口
    /// </summary>
    public interface INoteTagRepository : IRepository<NoteTag>
    {
        /// <summary>
        /// 删除标签 - 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OperationResEnum> DeleteNoteTagAsync(long id);
    }

    /// <summary>
    /// 笔记标签仓储实现
    /// </summary>
    [AutoInject(ServiceLifetime.Scoped, "Memo")]
    public class NoteTagRepository : BaseRepository<NoteTag>, INoteTagRepository
    {
        public NoteTagRepository(IMultiFreeSqlManager<DbKey> multiFreeSql) : base(multiFreeSql, DbKey.DbCraftHub)
        {
        }

        public async Task<OperationResEnum> DeleteNoteTagAsync(long id)
        {
            var tag = await _fsql.GetRepository<NoteTag>().Where(t => t.Id == id).FirstAsync();
            if (tag == null) return OperationResEnum.NotFound;

            tag.IsDeleted = DeleteEnum.Deleted;
            var res = await _fsql.GetRepository<NoteTag>().UpdateAsync(tag);
            return res > 0 ? OperationResEnum.Success : OperationResEnum.Fail;
        }
    }
}