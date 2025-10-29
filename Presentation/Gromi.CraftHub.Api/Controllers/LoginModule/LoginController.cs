using Gromi.Application.Common.LoginModle;
using Gromi.Infra.DataAccess.Shared;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.LoginModule.Params;
using Microsoft.AspNetCore.Mvc;

namespace Gromi.CraftHub.Api.Controllers.LoginModule
{
    /// <summary>
    /// 登录控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Filter.LoggingActionFilter]
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
        public async Task<IActionResult> Captcha()
        {
            BaseResult<byte[]> res = await _loginService.GetCaptcha();

            return File(res.Data, "image/png");
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Register")]
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
        public async Task<IActionResult> Login([FromBody] LoginParam param)
        {
            BaseResult res = await _loginService.Login(param);
            return Ok(res);
        }
    }
}