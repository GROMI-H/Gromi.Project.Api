using AutoMapper;
using Gromi.Infra.DataAccess.DbEntity.CraftHub.MemoModule;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.Common.BaseModule.Params;
using Gromi.Infra.Entity.CraftHub.MemoModule.Dtos;
using Gromi.Infra.Utils.Helpers;
using Gromi.Repository.CraftHub.MemoModule;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Application.CraftHub.MemoModule
{
    /// <summary>
    /// 笔记服务接口
    /// </summary>
    public interface INoteService
    {
        #region NoteTag

        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <returns></returns>
        Task<BaseResult<IEnumerable<NoteTagDto>>> GetNoteTagList();

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        Task<BaseResult<NoteTagDto>> AddNoteTag(NoteTagDto tag);

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult> DeletNoteTag(BaseParam param);

        #endregion NoteTag
    }

    /// <summary>
    /// 笔记服务实现
    /// </summary>
    [AutoInject(ServiceLifetime.Scoped, "Memo")]
    public class NoteService : INoteService
    {
        private readonly IMapper _mapper;
        private readonly IFlowRepository _flowRepository;
        private readonly INoteTagRepository _tagRepository;
        private readonly INoteRecordRepository _recordRepository;

        public NoteService(IMapper mapper, INoteRecordRepository recordRepository, INoteTagRepository tagRepository, IFlowRepository flowRepository)
        {
            _mapper = mapper;
            _recordRepository = recordRepository;
            _tagRepository = tagRepository;
            _flowRepository = flowRepository;
        }

        #region NoteTag

        public async Task<BaseResult<NoteTagDto>> AddNoteTag(NoteTagDto tag)
        {
            try
            {
                BaseResult<NoteTagDto> result = new BaseResult<NoteTagDto>();

                var addRes = await _tagRepository.InsertAsync(_mapper.Map<NoteTag>(tag));
                if (addRes != null)
                {
                    result.Code = ResponseCodeEnum.Success;
                    result.Msg = $"标签添加成功";
                    result.Data = _mapper.Map<NoteTagDto>(addRes);
                }
                else
                {
                    result.Code = ResponseCodeEnum.Fail;
                    result.Msg = $"标签添加失败";
                }
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"添加笔记标签失败：{ex.Message}");
                return await Task.FromResult(new BaseResult<NoteTagDto>(ResponseCodeEnum.InternalError, $"添加笔记标签失败：{ex.Message}"));
            }
        }

        public async Task<BaseResult> DeletNoteTag(BaseParam param)
        {
            try
            {
                BaseResult result = new BaseResult();
                if (param.Id == null)
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Msg = $"删除失败：参数不合法";
                    return result;
                }
                var delRes = await _tagRepository.DeleteNoteTagAsync(param.Id.Value);

                (result.Code, result.Msg) = delRes switch
                {
                    OperationResEnum.Success => (ResponseCodeEnum.Success, "删除成功"),
                    OperationResEnum.Fail => (ResponseCodeEnum.Fail, "删除失败"),
                    OperationResEnum.NotFound => (ResponseCodeEnum.NotFound, "删除失败：数据不存在"),
                    _ => (ResponseCodeEnum.Success, "删除成功")
                };

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"删除失败:{ex.Message}");
                return await Task.FromResult(new BaseResult(ResponseCodeEnum.InternalError, ex.Message));
            }
        }

        public async Task<BaseResult<IEnumerable<NoteTagDto>>> GetNoteTagList()
        {
            BaseResult<IEnumerable<NoteTagDto>> result = new BaseResult<IEnumerable<NoteTagDto>>
            {
                Code = ResponseCodeEnum.Fail,
                Data = Enumerable.Empty<NoteTagDto>()
            };

            try
            {
                var queryRes = await _tagRepository.GetAllAsync();
                if (queryRes != null)
                {
                    result.Data = _mapper.Map<IEnumerable<NoteTagDto>>(queryRes).ToList();
                    result.Code = ResponseCodeEnum.Success;
                    result.Msg = "查询成功";
                }
                else
                {
                    result.Msg = "查询失败";
                }
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"查询失败:{ex.Message}");
                return await Task.FromResult(result);
            }
        }

        #endregion NoteTag
    }
}