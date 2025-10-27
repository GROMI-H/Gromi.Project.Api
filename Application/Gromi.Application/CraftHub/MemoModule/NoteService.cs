using AutoMapper;
using Gromi.Infra.DataAccess.DbEntity.CraftHub.MemoModule;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
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
        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        Task<BaseResult<NoteTagDto>> AddNoteTag(NoteTagDto tag);
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
    }
}