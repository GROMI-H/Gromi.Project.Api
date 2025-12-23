using System.Collections.Concurrent;

namespace Gromi.CraftHub.Api.Middlewares
{
    /// <summary>
    /// 幂等性中间件
    /// </summary>
    public class IdempotencyMiddleware
    {
        // TODO 使用Redis
        private readonly RequestDelegate _next;

        // 使用并发集合存储已处理的请求标识符
        private static readonly ConcurrentDictionary<string, bool> _progressedRequest = new ConcurrentDictionary<string, bool>();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="next"></param>
        public IdempotencyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 幂等判断逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put || context.Request.Method == HttpMethods.Delete)
            {
                var idempotencykey = context.Request.Headers["Idempotency-Key"].ToString();

                if (!string.IsNullOrEmpty(idempotencykey))
                {
                    if (_progressedRequest.ContainsKey(idempotencykey))
                    {
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        await context.Response.WriteAsync("请求已处理，请勿重复提交!");
                        return;
                    }
                    else
                    {
                        _progressedRequest.TryAdd(idempotencykey, true);
                    }
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("缺少Idempotency-Key请求头");
                }
            }
        }
    }
}