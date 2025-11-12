using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace Gromi.Infra.Utils.Helpers
{
    /// <summary>
    /// 会话帮助类
    /// </summary>
    public static class SessionHelper
    {
        private static IHttpContextAccessor? _httpContextAccessor;

        public static void Init(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取session对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object? GetSession(string name)
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext.Session.TryGetValue(name, out var value))
            {
                return JsonConvert.DeserializeObject<object>(Encoding.UTF8.GetString(value));
            }
            return null;
        }

        /// <summary>
        /// 设置Session对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetSession(string name, object value)
        {
            if (_httpContextAccessor != null)
            {
                var byteValue = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
                _httpContextAccessor.HttpContext.Session.Set(name, byteValue);
            }
        }

        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveSession(string name)
        {
            if (_httpContextAccessor != null)
            {
                _httpContextAccessor.HttpContext.Session.Remove(name);
            }
        }

        /// <summary>
        /// 清除所有的Session
        /// </summary>
        public static void ClearSession()
        {
            if (_httpContextAccessor != null)
            {
                _httpContextAccessor.HttpContext.Session.Clear();
            }
        }
    }
}