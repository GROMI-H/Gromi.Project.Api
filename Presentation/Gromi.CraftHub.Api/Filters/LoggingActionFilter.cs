using Gromi.Infra.Utils.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Gromi.CraftHub.Api.Filters
{
    /// <summary>
    /// 请求日志过滤器
    /// </summary>
    public class LoggingActionFilter : Attribute, IAsyncActionFilter
    {
        /// <summary>
        /// 在控制器操作执行前后运行自定义逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            #region 前置逻辑,请求前

            // 请求Api
            var methodName = context.ActionDescriptor.DisplayName;
            // 请求参数
            var parameters = string.Join(", ", context.ActionArguments.Select(param => $"{param.Key}={JsonConvert.SerializeObject(param.Value)}"));
            LogHelper.Debug($"【{methodName}】【参数】{parameters}");

            #endregion 前置逻辑,请求前

            var resultContext = await next();

            #region 后置逻辑,请求后

            LogHelper.Debug($"【{methodName}】【响应】 {JsonConvert.SerializeObject(resultContext.Result)}");

            #endregion 后置逻辑,请求后
        }
    }
}