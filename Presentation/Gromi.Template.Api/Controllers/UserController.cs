using Gromi.Application.Template;
using Gromi.Infra.Repository.DbEntity.Template;
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

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllUserInfo")]
        public async Task<IActionResult> GetAllUserInfo()
        {
            var result = await _userService.GetAllUserInfo();
            return Ok(result);
        }

        /// <summary>
        /// 获取指定用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetUserInfoById")]
        public async Task<IActionResult> GetUserInfoById([FromQuery] long id)
        {
            var result = await _userService.GetUserInfoById(id);
            return Ok(result);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UpdateUserInfo")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserInfo model)
        {
            var result = await _userService.UpdateUserInfo(model);
            return Ok(result);
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("DeleteUseInfo")]
        public async Task<IActionResult> DeleteUserInfo([FromBody] long id)
        {
            var result = await _userService.DeleteUserInfo(id);
            return Ok(result);
        }
    }
}