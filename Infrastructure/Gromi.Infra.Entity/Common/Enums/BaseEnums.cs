namespace Gromi.Infra.Entity.Common.Enums
{
    /// <summary>
    /// 基础状态枚举：-1-未知，0-未启用，1-启用
    /// </summary>
    public enum StatusEnum
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// 未启用
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// 启用
        /// </summary>
        Enabled = 1
    }

    /// <summary>
    /// 删除状态枚举：0-未删除，1-已删除
    /// </summary>
    public enum DeleteEnum
    {
        /// <summary>
        /// 未删除
        /// </summary>
        NotDeleted = 0,

        /// <summary>
        /// 已删除
        /// </summary>
        Deleted = 1
    }
}