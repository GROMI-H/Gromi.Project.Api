using Gromi.Application.TemplateModule;
using Gromi.Infra.Entity.TemplateModule.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Gromi.Template.Api.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Filter.LoggingActionFilter]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserInfo model)
        {
            var result = await _userService.CreateUserInfo(model);
            return Ok(result);
        }
    }
}