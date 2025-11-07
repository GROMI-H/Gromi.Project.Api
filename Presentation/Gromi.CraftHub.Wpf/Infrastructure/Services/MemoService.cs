using Gromi.CraftHub.Wpf.Infrastructure.Common;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.Common.BaseModule.Params;
using Gromi.Infra.Entity.CraftHub.MemoModule.Dtos;
using Gromi.Infra.Utils.Helpers;
using Newtonsoft.Json;
using System.Security.Cryptography.Xml;

namespace Gromi.CraftHub.Wpf.Infrastructure.Services
{
    public class MemoService
    {
        #region 标签

        /// <summary>
        /// 添加笔记标签
        /// </summary>
        /// <param name="noteTagDto"></param>
        /// <returns></returns>
        public async Task<BaseResult<NoteTagDto>> AddNoteTag(NoteTagDto noteTagDto)
        {
            string jsonParam = JsonConvert.SerializeObject(noteTagDto);
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                {"Authorization", $"Bearer {GlobalManager.Token}" }
            };
            string opRes = await HttpHelper.PostAsync($"{GlobalManager.BaseUrl}api/Memo/Tag/Add", jsonParam, headers);
            var result = JsonConvert.DeserializeObject<BaseResult<NoteTagDto>>(opRes);
            return result != null ? result : new BaseResult<NoteTagDto>(ResponseCodeEnum.InternalError, "返回错误，请重试");
        }

        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResult<IEnumerable<NoteTagDto>>> GetNoteTagList()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                {"Authorization", $"Bearer {GlobalManager.Token}" }
            };
            string opRes = await HttpHelper.PostAsync($"{GlobalManager.BaseUrl}api/Memo/Tag/GetList", string.Empty, headers: headers);
            var result = JsonConvert.DeserializeObject<BaseResult<IEnumerable<NoteTagDto>>>(opRes);
            return result != null ? result : new BaseResult<IEnumerable<NoteTagDto>>(ResponseCodeEnum.InternalError, "返回错误，请重试");
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<BaseResult> DeleteNoteTag(BaseDeleteParam param)
        {
            string jsonParam = JsonConvert.SerializeObject(param);
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                {"Authorization", $"Bearer {GlobalManager.Token}" }
            };
            string opRes = await HttpHelper.PostAsync($"{GlobalManager.BaseUrl}api/Memo/Tag/Delete", jsonParam, headers);
            var result = JsonConvert.DeserializeObject<BaseResult>(opRes);
            return result != null ? result : new BaseResult(ResponseCodeEnum.InternalError, "返回错误，请重试");
        }

        #endregion 标签

        #region 记录

        //

        #endregion 记录
    }
}