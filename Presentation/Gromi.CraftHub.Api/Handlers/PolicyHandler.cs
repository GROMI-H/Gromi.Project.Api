using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Gromi.CraftHub.Api.Handler
{
    /// <summary>
    /// 角色授权处理
    /// </summary>
    public class PolicyHandler : AuthorizationHandler<PolicyRequirement>
    {
        /// <summary>
        /// 授权方式(cookie,bearer,oauth,openid)
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="httpContextAccessor"></param>
        public PolicyHandler(IAuthenticationSchemeProvider schemes, IHttpContextAccessor httpContextAccessor)
        {
            Schemes = schemes;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 授权处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            if (context.User == null || context.User.Identity == null || !context.User.Identity.IsAuthenticated || _httpContextAccessor.HttpContext == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // TODO 校验逻辑待优化

            // 获取角色信息
            var role = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value.ToString();
            if (role == "SuperAdmin")
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // 获取角色对应的路由信息

            // 判断当前路由在不在对应的路由之中
            var url = _httpContextAccessor.HttpContext.Request.Path.Value;

            return Task.CompletedTask;
        }
    }

    /// <summary>
    ///  自定义策略要求
    /// </summary>
    public class PolicyRequirement : IAuthorizationRequirement
    { }
}