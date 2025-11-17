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
    /// 角色控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Permission")]
    [LoggingActionFilter]
    [TypeFilter(typeof(EncryptFilter))]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="roleService"></param>
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 接口绑定
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("BindApi")]
        [Description("接口绑定")]
        public async Task<IActionResult> BindRole([FromBody] BindApiParam param)
        {
            BaseResult res = await _roleService.BindApis(param);
            return Ok(res);
        }

        /// <summary>
        /// 接口解绑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("UnBindApi")]
        [Description("接口解绑")]
        public async Task<IActionResult> UnBindApi([FromBody] BindApiParam param)
        {
            BaseResult res = await _roleService.UnBindApis(param);
            return Ok(res);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("DeleteRole")]
        [Description("删除角色")]
        public async Task<IActionResult> DeleteRole([FromBody] BaseDeleteParam param)
        {
            BaseResult res = await _roleService.DeleteRole(param);
            return Ok(res);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("AddRole")]
        [Description("添加角色")]
        public async Task<IActionResult> AddSystemRole([FromBody] SystemRoleDto param)
        {
            BaseResult res = await _roleService.AddSystemRole(param);
            return Ok(res);
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("GetRole")]
        [Description("获取角色信息")]
        public async Task<IActionResult> GetSystemRole([FromQuery] BaseParam param)
        {
            BaseResult res = await _roleService.GetSystemRole(param);
            return Ok(res);
        }
    }
}