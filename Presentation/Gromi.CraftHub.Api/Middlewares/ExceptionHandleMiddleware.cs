using Gromi.Infra.Utils.Helpers;

namespace Gromi.CraftHub.Api.Middlewares
{
    /// <summary>
    /// 异常处理中间件
    /// </summary>
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="next"></param>
        public ExceptionHandleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 异常捕获
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleException(context, e);
            }
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private Task HandleException(HttpContext context, Exception ex)
        {
            EmailHelper.SendEmail();
            return Task.CompletedTask;
        }
    }
}