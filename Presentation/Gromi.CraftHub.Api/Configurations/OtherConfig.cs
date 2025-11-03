using Yitter.IdGenerator;

namespace Gromi.CraftHub.Api.Configurations
{
    /// <summary>
    /// 其他配置
    /// </summary>
    public static class OtherConfig
    {
        /// <summary>
        /// 添加其他配置
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOtherConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 1. 雪花ID生成器
            var options = new IdGeneratorOptions(workerId: 1);
            YitIdHelper.SetIdGenerator(options);

            // 2.Session配置
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(60);
                options.Cookie.HttpOnly = true; // 使cookie只能通过Http请求访问
                options.Cookie.IsEssential = true; // 确保cookie总是被发送
            });
        }
    }
}