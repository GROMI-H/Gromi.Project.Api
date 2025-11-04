using Gromi.Application.CraftHub.MemoModule;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Params;
using Gromi.Infra.Entity.CraftHub.MemoModule.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Gromi.CraftHub.Api.Controllers.MemoModule
{
    /// <summary>
    /// 笔记控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Permission")]
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

        #region 记录

        /// <summary>
        /// 添加笔记记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        [HttpPost("Record/Add")]
        [Description("添加笔记记录")]
        public async Task<IActionResult> AddNoteRecord([FromBody] NoteRecordDto record)
        {
            BaseResult<NoteRecordDto> res = await _noteService.AddNoteRecord(record);
            return Ok(res);
        }

        /// <summary>
        /// 删除笔记记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Record/Delete")]
        [Description("删除笔记记录")]
        public async Task<IActionResult> DeleteNoteRecord([FromBody] BaseDeleteParam param)
        {
            BaseResult result = await _noteService.DeleteNoteRecord(param);
            return Ok(result);
        }

        /// <summary>
        /// 查询记录列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("Record/GetList")]
        [Description("查询记录列表")]
        public async Task<IActionResult> GetNoteTagList()
        {
            BaseResult<IEnumerable<NoteRecordDto>> result = await _noteService.GetNoteRecordList();
            return Ok(result);
        }

        #endregion 记录

        #region 标签

        /// <summary>
        /// 添加笔记标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost("Tag/Add")]
        [Description("添加笔记标签")]
        public async Task<IActionResult> AddNoteTag([FromBody] NoteTagDto tag)
        {
            BaseResult<NoteTagDto> res = await _noteService.AddNoteTag(tag);
            return Ok(res);
        }

        /// <summary>
        /// 删除笔记标签
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Tag/Delete")]
        [Description("删除笔记标签")]
        public async Task<IActionResult> DeleteNoteTag([FromBody] BaseDeleteParam param)
        {
            BaseResult result = await _noteService.DeleteNoteTag(param);
            return Ok(result);
        }

        /// <summary>
        /// 查询标签列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("Tag/GetList")]
        [Description("查询标签列表")]
        public async Task<IActionResult> GetNoteRecordList()
        {
            BaseResult<IEnumerable<NoteTagDto>> result = await _noteService.GetNoteTagList();
            return Ok(result);
        }

        #endregion 标签
    }
}