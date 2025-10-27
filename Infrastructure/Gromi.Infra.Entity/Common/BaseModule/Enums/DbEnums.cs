namespace Gromi.Infra.Entity.Common.BaseModule.Enums
{
    /// <summary>
    /// 数据库枚举
    /// </summary>
    public enum DbKey
    {
        /// <summary>
        /// 测试数据库
        /// </summary>
        DbTemp,

        /// <summary>
        /// CraftHub数据库
        /// </summary>
        DbCraftHub
    }

    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    public enum DbMode
    {
        /// <summary>
        /// SQLite
        /// </summary>
        SQLite,

        /// <summary>
        /// MySQL
        /// </summary>
        MySQL
    }
}