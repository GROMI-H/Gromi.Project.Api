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
}