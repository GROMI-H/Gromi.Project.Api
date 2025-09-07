namespace Gromi.Infra.Entity.CommonModule.Settings
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
    }
}