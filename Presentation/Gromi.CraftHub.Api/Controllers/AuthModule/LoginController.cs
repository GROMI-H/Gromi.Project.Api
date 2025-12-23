using Gromi.Application.Common.AuthModule;
using Gromi.CraftHub.Api.Filters;
using Gromi.Infra.Entity.Common.AuthModule.Dtos;
using Gromi.Infra.Entity.Common.AuthModule.Params;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Gromi.CraftHub.Api.Controllers.AuthModule
{
    /// <summary>
    /// 登录控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [LoggingActionFilter]
    [TypeFilter(typeof(EncryptFilter))]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="loginService"></param>
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        /// <summary>
        /// 获取验证码图片
        /// </summary>
        /// <returns></returns>
        [HttpGet("Captcha")]
        [Description("获取验证码图片")]
        public async Task<IActionResult> Captcha()
        {
            BaseResult<string> res = new BaseResult<string>();
            //BaseResult<string> res = await _loginService.GetCaptcha();
            EmailHelper.SendEmail();
            return Ok(res);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        [Description("用户注册")]
        public async Task<IActionResult> Register([FromBody] RegisterParam param)
        {
            BaseResult res = await _loginService.Register(param);
            return Ok(res);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [Description("用户登录")]
        public async Task<IActionResult> Login([FromBody] LoginParam param)
        {
            BaseResult<LoginResponse> res = await _loginService.Login(param);
            return Ok(res);
        }
    }
}