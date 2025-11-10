using Gromi.Infra.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Text;

namespace Gromi.CraftHub.Api.Filters
{
    /// <summary>
    /// 加解密过滤器
    /// </summary>
    public class EncryptFilter : Attribute, IAsyncActionFilter
    {
        private readonly bool IS_DEBUG = false;
        private readonly string AES_KEY = "WUGYnNB6u0m8v9uNazbCMTjF7r4Gtu5s";

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configuration"></param>
        public EncryptFilter(IConfiguration configuration)
        {
            IS_DEBUG = Convert.ToBoolean(configuration["IsDebug"] ?? "false");
            AES_KEY = configuration["Encrypt:AesKey"] ?? AES_KEY;
        }

        /// <summary>
        /// 在控制器操作执行前后运行自定义逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!IS_DEBUG)
            {
                if (context.HttpContext.Request.Body.CanRead)
                {
                    using (var reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8, leaveOpen: true))
                    {
                        context.HttpContext.Request.Body.Position = 0;
                        var body = await reader.ReadToEndAsync();
                        var decryptedBody = EncryptHelper.DecryptAes(body, AES_KEY);
                        context.HttpContext.Request.Body.Position = 0;
                        var bytes = Encoding.UTF8.GetBytes(decryptedBody);
                        context.HttpContext.Request.Body = new MemoryStream(bytes);
                    }
                }

                var resultContext = await next();

                if (resultContext.Result is ObjectResult objectResult)
                {
                    if (objectResult.Value != null)
                    {
                        var jsonResult = JsonConvert.SerializeObject(objectResult.Value);
                        var encryptedResult = EncryptHelper.EncryptAes(jsonResult, AES_KEY);
                        objectResult.Value = encryptedResult;
                    }
                }
            }
            else
            {
                await next();
                return;
            }
        }
    }
}