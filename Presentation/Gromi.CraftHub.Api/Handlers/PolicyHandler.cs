using Gromi.Application.Common.SystemModule;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Gromi.CraftHub.Api.Handlers
{
    /// <summary>
    /// 角色授权处理
    /// </summary>
    public class PolicyHandler : AuthorizationHandler<PolicyRequirement>
    {
        private readonly bool IsDebug = false;
        private readonly IRoleService _roleService;

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
        /// <param name="roleService"></param>
        public PolicyHandler(IAuthenticationSchemeProvider schemes, IHttpContextAccessor httpContextAccessor, IRoleService roleService, IConfiguration configuration)
        {
            Schemes = schemes;
            IsDebug = Convert.ToBoolean(configuration["IsDebug"] ?? "false");
            _roleService = roleService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 授权处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            if (context.User == null || context.User.Identity == null || !context.User.Identity.IsAuthenticated || _httpContextAccessor.HttpContext == null)
            {
                context.Fail();
                return;
            }

            if (!IsDebug)
            {
                // 获取角色信息
                var role = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value.ToString();
                if (string.IsNullOrEmpty(role))
                {
                    context.Fail();
                    return;
                }
                var roleIds = JsonConvert.DeserializeObject<IEnumerable<long>>(role);
                if (roleIds == null)
                {
                    context.Fail();
                    return;
                }
                // 判断当前路由在不在对应的路由之中
                var verifyRes = await _roleService.VerifyUrl(roleIds.ToList(), _httpContextAccessor.HttpContext.Request.Path.Value);
                if (verifyRes.Code == ResponseCodeEnum.Success)
                {
                    context.Succeed(requirement);
                    return;
                }

                context.Fail();
                return;
            }
            else
            {
                context.Succeed(requirement);
                return;
            }
        }
    }

    /// <summary>
    ///  自定义策略要求
    /// </summary>
    public class PolicyRequirement : IAuthorizationRequirement
    { }
}