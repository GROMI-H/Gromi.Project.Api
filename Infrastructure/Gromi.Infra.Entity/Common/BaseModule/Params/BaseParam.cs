namespace Gromi.Infra.Entity.Common.BaseModule.Params
{
    /// <summary>
    /// 基础参数
    /// </summary>
    public class BaseParam
    {
        /// <summary>
        /// JwtToken
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// 主键ID
        /// </summary>
        public long? Id { get; set; }
    }

    /// <summary>
    /// 基础删除参数
    /// </summary>
    public class BaseDeleteParam
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 主键列表，批量操作使用
        /// </summary>
        public List<long> Ids { get; set; } = new List<long>();
    }
}