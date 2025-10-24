using Gromi.Application.CraftHub.MemoModule;
using Gromi.Infra.Entity.Common.Dtos;
using Gromi.Infra.Entity.CraftHub.MemoModule.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Gromi.CraftHub.Api.Controllers.MemoModule
{
    /// <summary>
    /// 笔记控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Filter.LoggingActionFilter]
    public class MemoController : ControllerBase
    {
        private readonly INoteService _noteService;

        /// <summary>
        ///  初始化
        /// </summary>
        /// <param name="noteService"></param>
        public MemoController(INoteService noteService)
        {
            _noteService = noteService;
        }

        /// <summary>
        /// 添加笔记标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost("AddNoteTag")]
        public async Task<IActionResult> AddNoteTag([FromBody] NoteTagDto tag)
        {
            BaseResult<NoteTagDto> res = await _noteService.AddNoteTag(tag);
            return Ok(res);
        }
    }
}