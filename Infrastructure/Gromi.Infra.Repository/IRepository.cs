namespace Gromi.Infra.Repository
{
    /// <summary>
    /// 异步仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 异步添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回添加后的实体</returns>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// 异步获取所有实体
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetAllAsync();

        /// <summary>
        /// 异步获取指定ID实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity?> GetModelAsync(long id);

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>是否更新成功</returns>
        Task<bool> UpdateAsync(TEntity entity);

        /// <summary>
        /// 异步删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteAsync(long id);
    }
}