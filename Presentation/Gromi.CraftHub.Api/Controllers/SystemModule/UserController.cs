using Gromi.Application.Common.SystemModule;
using Gromi.CraftHub.Api.Filters;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Params;
using Gromi.Infra.Entity.Common.SystemModule.Dtos;
using Gromi.Infra.Entity.Common.SystemModule.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Gromi.CraftHub.Api.Controllers.SystemModule
{
    /// <summary>
    /// 系统用户控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Permission")]
    [LoggingActionFilter]
    [TypeFilter(typeof(EncryptFilter))]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 角色绑定
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("BindRole")]
        [Description("角色绑定")]
        public async Task<IActionResult> BindRole([FromBody] BindRoleParam param)
        {
            BaseResult res = await _userService.BindRoles(param);
            return Ok(res);
        }

        /// <summary>
        /// 角色解绑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("UnBindRole")]
        [Description("角色解绑")]
        public async Task<IActionResult> UnBindRole([FromBody] BindRoleParam param)
        {
            BaseResult res = await _userService.UnBindRoles(param);
            return Ok(res);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("ResetPassword")]
        [Description("重置密码")]
        public async Task<IActionResult> ResetPassword([FromBody] BaseParam param)
        {
            BaseResult res = await _userService.ResetPassrod(param);
            return Ok(res);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userInfoDto"></param>
        /// <returns></returns>
        [HttpPost("UpdateUserInfo")]
        [Description("更新用户信息")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserInfoDto userInfoDto)
        {
            BaseResult res = await _userService.UpdateUserInfo(userInfoDto);
            return Ok(res);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("DeleteUser")]
        [Description("删除用户")]
        public async Task<IActionResult> DeleteUser([FromBody] BaseDeleteParam param)
        {
            BaseResult res = await _userService.DeleteUser(param);
            return Ok(res);
        }
    }
}