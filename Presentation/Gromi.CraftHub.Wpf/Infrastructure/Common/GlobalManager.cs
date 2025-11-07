using System.Configuration;

namespace Gromi.CraftHub.Wpf.Infrastructure.Common
{
    /// <summary>
    /// 全局数据管理器
    /// </summary>
    public static class GlobalManager
    {
        private static string _token = string.Empty;

        /// <summary>
        /// Jwt Token
        /// </summary>
        public static string Token
        {
            get => _token;
            set => _token = value;
        }

        /// <summary>
        /// 基础路由
        /// </summary>
        public static readonly string BaseUrl = ConfigurationManager.AppSettings["BaseUrl"] ?? "http://localhost:5093/";
    }
}