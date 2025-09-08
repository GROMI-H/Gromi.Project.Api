using Gromi.Infra.Entity.CommonModule.Dtos;
using Gromi.Infra.Entity.CommonModule.Enums;

namespace Gromi.Infra.Repository
{
    /// <summary>
    /// 通用仓储（支持多数据库和异步操作）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly IFreeSql _fsql;

        public BaseRepository(IMultiFreeSqlManager<DbKey> multiFreeSql, DbKey dbKey)
        {
            _fsql = multiFreeSql.Get(dbKey); // 根据传入的枚举获取对应数据库
        }

        #region 异步方法

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            return await _fsql.GetRepository<TEntity>().InsertAsync(entity);
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _fsql.GetRepository<TEntity>().Select.ToListAsync();
        }

        public virtual async Task<TEntity?> GetModelAsync(long id)
        {
            return await _fsql.GetRepository<TEntity>().Select.WhereDynamic(id).FirstAsync();
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            var existingEntity = await _fsql.GetRepository<TEntity>().Select.WhereDynamic(entity.Id).FirstAsync();
            if (existingEntity == null)
            {
                return false;
            }
            entity.CreateTime = existingEntity.CreateTime;
            var effectRows = await _fsql.GetRepository<TEntity>().UpdateAsync(entity);
            return effectRows > 0;
        }

        public virtual async Task<bool> DeleteAsync(long id)
        {
            var entity = await _fsql.GetRepository<TEntity>().Select.WhereDynamic(id).FirstAsync();
            if (entity == null) return false;

            var effectRows = await _fsql.GetRepository<TEntity>().DeleteAsync(entity);
            return effectRows > 0;
        }

        #endregion 异步方法
    }
}