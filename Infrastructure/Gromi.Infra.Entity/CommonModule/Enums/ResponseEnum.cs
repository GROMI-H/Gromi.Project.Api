namespace Gromi.Infra.Entity.CommonModule.Enums
{
    /// <summary>
    /// 响应码枚举
    /// </summary>
    public enum ResponseCodeEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 失败
        /// </summary>
        Fail = 1,

        /// <summary>
        /// 非法参数
        /// </summary>
        InvalidParameter = 2,

        /// <summary>
        /// 资源未找到
        /// </summary>
        NotFound = 3,

        /// <summary>
        /// 认证失败
        /// </summary>
        Unauthorized = 4,

        /// <summary>
        /// 内部错误
        /// </summary>
        InternalError = 5,

        /// <summary>
        /// 超时
        /// </summary>
        Timeout = 6,
    }
}