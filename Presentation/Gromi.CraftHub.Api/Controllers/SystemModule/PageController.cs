using Gromi.Application.Common.SystemModule;
using Gromi.CraftHub.Api.Filters;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.SystemModule.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Gromi.CraftHub.Api.Controllers.SystemModule
{
    /// <summary>
    /// 页面控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Permission")]
    [LoggingActionFilter]
    [TypeFilter(typeof(EncryptFilter))]
    public class PageController : ControllerBase
    {
        private readonly IPageService _pageService;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pageService"></param>
        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        /// <summary>
        /// 页面路由添加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("AddPageRoute")]
        [Description("页面路由添加")]
        public async Task<IActionResult> AddPageRoute([FromBody] PageRouteDto param)
        {
            BaseResult result = await _pageService.AddPageRoute(param);
            return Ok(result);
        }
    }
}