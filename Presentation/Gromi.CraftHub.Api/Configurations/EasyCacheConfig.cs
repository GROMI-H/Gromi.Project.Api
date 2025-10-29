using EasyCaching.CSRedis;
using Gromi.Infra.DataAccess.Shared;
using Gromi.Infra.Entity.Common.BaseModule.Settings;

namespace Gromi.CraftHub.Api.Configurations
{
    /// <summary>
    /// Redis配置
    /// </summary>
    public static class EasyCacheConfig
    {
        /// <summary>
        /// 添加Redis配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddEasyCacheConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var redisConfig = configuration.GetSection("Redis").Get<RedisConfig>() ?? new RedisConfig();
            string connectionStr = $"{redisConfig.Server},password={redisConfig.Password}";

            var redisOptions = new CSRedisDBOptions
            {
                ConnectionStrings = new List<string> { connectionStr },
                ReadOnly = false
            };
            services.AddEasyCaching(options =>
            {
                options.WithMessagePack("msgpack");
                // Json序列化
                options.UseCSRedis(config =>
                {
                    config.DBConfig = redisOptions;
                    config.SerializerName = "msgpack";
                }, "CsRedisWithMsgpack");
            });

            services.AddSingleton(typeof(RedisServer<>));
        }
    }
}