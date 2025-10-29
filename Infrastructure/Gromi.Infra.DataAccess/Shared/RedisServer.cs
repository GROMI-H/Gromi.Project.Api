using EasyCaching.Core;
using Gromi.Infra.Entity.Common.BaseModule.Settings;
using Microsoft.Extensions.Configuration;

namespace Gromi.Infra.DataAccess.Shared
{
    /// <summary>
    /// 缓存通用服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RedisServer<T>
    {
        private readonly string PREFIX;
        private readonly IEasyCachingProvider _redisProvieder;

        public RedisServer(IEasyCachingProviderFactory factory, IConfiguration configuration)
        {
            _redisProvieder = factory.GetCachingProvider("CsRedisWithMsgpack");
            var redisConfig = configuration.GetSection("Redis").Get<RedisConfig>();
            PREFIX = redisConfig != null ? redisConfig.Prefix : string.Empty;
        }

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string key)
        {
            var cached = await _redisProvieder.GetAsync<T>($"{PREFIX}{key}");
            return cached.HasValue ? cached.Value : default(T);
        }

        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        public Task SetAsync(string key, T value, TimeSpan? expiration = null)
        {
            expiration = expiration ?? TimeSpan.FromHours(2); // 默认两个小时
            return _redisProvieder.SetAsync($"{PREFIX}{key}", value, expiration.Value);
        }

        /// <summary>
        /// 移除缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task RemoveAsync(string key)
        {
            return _redisProvieder.RemoveAsync($"{PREFIX}{key}");
        }
    }
}