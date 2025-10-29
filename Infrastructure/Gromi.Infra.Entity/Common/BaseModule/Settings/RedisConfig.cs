namespace Gromi.Infra.Entity.Common.BaseModule.Settings
{
    /// <summary>
    /// Redis配置类
    /// </summary>
    public class RedisConfig
    {
        /// <summary>
        /// 连接地址
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; set; }
    }
}