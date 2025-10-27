using Gromi.Infra.Entity.Common.BaseModule.Enums;

namespace Gromi.Infra.Entity.Common.BaseModule.Settings
{
    /// <summary>
    /// FreeSql配置类
    /// </summary>
    public class DbConnectionConfig
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// 是否开启自动建表
        /// </summary>
        public bool EnableAutoSyncStructure { get; set; } = false;

        /// <summary>
        /// 是否开启实体延迟加载
        /// </summary>
        public bool EnableLazyLoading { get; set; } = false;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbMode DbMode { get; set; } = DbMode.SQLite;
    }
}