namespace Gromi.Infra.Repository.Shared
{
    /// <summary>
    /// MultiFreeSql管理器接口
    /// </summary>
    /// <typeparam name="TDBKey"></typeparam>
    public interface IMultiFreeSqlManager<TDBKey>
    {
        /// <summary>
        /// 获取FreeSql实例
        /// </summary>
        /// <param name="dBKey"></param>
        /// <returns></returns>
        IFreeSql Get(TDBKey dBKey);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="dBKey"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        IMultiFreeSqlManager<TDBKey> Register(TDBKey dBKey, Func<IFreeSql> create);
    }

    /// <summary>
    /// MultiFreeSql管理器
    /// </summary>
    /// <typeparam name="TDBKey"></typeparam>
    public class MultiFreeSqlManager<TDBKey> : IMultiFreeSqlManager<TDBKey>
    {
        private readonly IdleBus<TDBKey, IFreeSql> _ib = new IdleBus<TDBKey, IFreeSql>(TimeSpan.MinValue);

        public IFreeSql Get(TDBKey dBKey) => _ib.Get(dBKey);

        public IMultiFreeSqlManager<TDBKey> Register(TDBKey dBKey, Func<IFreeSql> create)
        {
            if (!_ib.TryRegister(dBKey, create))
            {
                throw new Exception($"数据库{dBKey}注册失败");
            }
            return this;
        }
    }
}