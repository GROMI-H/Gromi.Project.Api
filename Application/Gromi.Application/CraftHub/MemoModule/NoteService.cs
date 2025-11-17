using Gromi.Application.Validator.CraftHub.MemoModule;
using Gromi.Infra.DataAccess.DbEntity.CraftHub.MemoModule;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.Common.BaseModule.Params;
using Gromi.Infra.Entity.CraftHub.MemoModule.Dtos;
using Gromi.Infra.Utils.Helpers;
using Gromi.Repository.CraftHub.MemoModule;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Application.CraftHub.MemoModule
{
    /// <summary>
    /// 笔记服务接口
    /// </summary>
    public interface INoteService
    {
        #region Record

        /// <summary>
        /// 获取笔记列表
        /// </summary>
        /// <returns></returns>
        Task<BaseResult<IEnumerable<NoteRecordDto>>> GetNoteRecordList();

        /// <summary>
        /// 删除笔记记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult> DeleteNoteRecord(BaseDeleteParam param);

        /// <summary>
        /// 添加笔记记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult<NoteRecordDto>> AddNoteRecord(NoteRecordDto param);

        /// <summary>
        /// 更新笔记记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult> UpdateNoteRecord(NoteRecordDto param);

        #endregion Record

        #region Tag

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
        Task<BaseResult> DeleteNoteTag(BaseDeleteParam param);

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult> UpdateNotetag(NoteTagDto param);

        #endregion Tag
    }

    /// <summary>
    /// 笔记服务实现
    /// </summary>
    [AutoInject(ServiceLifetime.Scoped, "Memo")]
    public class NoteService : INoteService
    {
        private readonly IFlowRepository _flowRepository;
        private readonly INoteTagRepository _tagRepository;
        private readonly INoteRecordRepository _recordRepository;

        public NoteService(INoteRecordRepository recordRepository, INoteTagRepository tagRepository, IFlowRepository flowRepository)
        {
            _recordRepository = recordRepository;
            _tagRepository = tagRepository;
            _flowRepository = flowRepository;
        }

        #region Record

        public async Task<BaseResult<IEnumerable<NoteRecordDto>>> GetNoteRecordList()
        {
            BaseResult<IEnumerable<NoteRecordDto>> result = new BaseResult<IEnumerable<NoteRecordDto>>
            {
                Code = ResponseCodeEnum.InternalError,
                Data = Enumerable.Empty<NoteRecordDto>()
            };
            try
            {
                var queryRes = await _recordRepository.GetAllAsync();
                if (queryRes != null)
                {
                    result.Data = queryRes.Adapt<IEnumerable<NoteRecordDto>>().ToList();
                    result.Code = ResponseCodeEnum.Success;
                    result.Message = "查询成功";
                }
                else
                {
                    result.Code = ResponseCodeEnum.Fail;
                    result.Message = "查询失败";
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Message = $"获取笔记列表失败:{ex.Message}";
                LogHelper.Error(result.Message);
                return await Task.FromResult(result);
            }
        }

        public async Task<BaseResult> DeleteNoteRecord(BaseDeleteParam param)
        {
            BaseResult result = new BaseResult
            {
                Code = ResponseCodeEnum.InternalError
            };

            try
            {
                if (param.Id == null || param.Ids == null || param.Ids.Count > 0)
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Message = $"删除失败：参数不合法";
                    return result;
                }

                if (param.Ids == null || param.Ids.Count > 0)
                {
                    param.Ids = new List<long> { param.Id.Value };
                }

                var delRes = await _recordRepository.DeleteNoteRecordAsync(param.Ids);

                (result.Code, result.Message) = delRes switch
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
                result.Message = $"删除失败:{ex.Message}";
                LogHelper.Error(result.Message);
                return await Task.FromResult(result);
            }
        }

        public async Task<BaseResult<NoteRecordDto>> AddNoteRecord(NoteRecordDto param)
        {
            BaseResult<NoteRecordDto> result = new BaseResult<NoteRecordDto>
            {
                Code = ResponseCodeEnum.InternalError,
                Data = param,
                Message = "添加失败"
            };
            try
            {
                var validateRes = new NoteRecordValidator().Validate(param);
                if (!validateRes.IsValid)
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Message = string.Join(";", validateRes.Errors);
                    return result;
                }
                var addRes = await _recordRepository.InsertRecordAsync(param.Adapt<NoteRecord>());
                if (addRes != null)
                {
                    result.Code = ResponseCodeEnum.Success;
                    result.Data = addRes.Adapt<NoteRecordDto>();
                    result.Message = "添加成功";
                }
                else
                {
                    result.Code = ResponseCodeEnum.Fail;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Message = $"笔记添加失败:{ex.Message}";
                LogHelper.Error(result.Message);
                return await Task.FromResult(result);
            }
        }

        public async Task<BaseResult> UpdateNoteRecord(NoteRecordDto param)
        {
            BaseResult result = new BaseResult { Code = ResponseCodeEnum.InternalError };
            try
            {
                var validateRes = new NoteRecordValidator().Validate(param);
                if (!validateRes.IsValid)
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Message = string.Join(";", validateRes.Errors);
                    return result;
                }

                var updateRes = await _recordRepository.UpdateAsync(param.Adapt<NoteRecord>());
                result.Code = updateRes ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Message = updateRes ? "更新成功" : "更新失败";
            }
            catch (Exception ex)
            {
                result.Message = $"更新失败:{ex.Message}";
                LogHelper.Error(result.Message);
            }
            return result;
        }

        #endregion Record

        #region Tag

        public async Task<BaseResult<NoteTagDto>> AddNoteTag(NoteTagDto tag)
        {
            BaseResult<NoteTagDto> result = new BaseResult<NoteTagDto>()
            {
                Code = ResponseCodeEnum.InternalError,
                Data = tag
            };
            try
            {
                var validateRes = new NoteTagValidator().Validate(tag);
                if (!validateRes.IsValid)
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Message = string.Join(";", validateRes.Errors);
                    return result;
                }

                var addRes = await _tagRepository.InsertAsync(tag.Adapt<NoteTag>());
                if (addRes != null)
                {
                    result.Code = ResponseCodeEnum.Success;
                    result.Message = $"标签添加成功";
                    result.Data = addRes.Adapt<NoteTagDto>();
                }
                else
                {
                    result.Code = ResponseCodeEnum.Fail;
                    result.Message = $"标签添加失败";
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Message = $"添加笔记标签失败：{ex.Message}";
                LogHelper.Error(result.Message);
                return await Task.FromResult(result);
            }
        }

        public async Task<BaseResult> DeleteNoteTag(BaseDeleteParam param)
        {
            BaseResult result = new BaseResult
            {
                Code = ResponseCodeEnum.InternalError,
                Message = "删除失败"
            };

            try
            {
                if (param.Id == null || param.Ids == null || param.Ids.Count > 0)
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Message = $"删除失败：参数不合法";
                    return result;
                }

                if (param.Ids == null || param.Ids.Count > 0)
                {
                    param.Ids = new List<long> { param.Id.Value };
                }

                var delRes = await _tagRepository.DeleteNoteTagAsync(param.Ids);

                (result.Code, result.Message) = delRes switch
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
                result.Message = $"删除失败:{ex.Message}";
                LogHelper.Error(result.Message);
                return await Task.FromResult(result);
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
                    result.Data = queryRes.Adapt<IEnumerable<NoteTagDto>>().ToList();
                    result.Code = ResponseCodeEnum.Success;
                    result.Message = "查询成功";
                }
                else
                {
                    result.Message = "查询失败";
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Message = $"查询失败:{ex.Message}";
                LogHelper.Error(result.Message);
                return await Task.FromResult(result);
            }
        }

        public async Task<BaseResult> UpdateNotetag(NoteTagDto param)
        {
            BaseResult result = new BaseResult();
            try
            {
                var validateRes = new NoteTagValidator().Validate(param);
                if (!validateRes.IsValid)
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Message = string.Join(";", validateRes.Errors);
                    return result;
                }
                var updateRes = await _tagRepository.UpdateAsync(param.Adapt<NoteTag>());
                result.Code = updateRes ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Message = updateRes ? "更新成功" : "更新失败";
            }
            catch (Exception ex)
            {
                result.Message = $"更新失败:{ex.Message}";
                LogHelper.Error(result.Message);
            }
            return result;
        }

        #endregion Tag
    }
}