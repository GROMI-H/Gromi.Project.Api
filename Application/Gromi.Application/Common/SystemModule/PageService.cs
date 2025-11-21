using Gromi.Application.Validator.Common.SystemModule;
using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.Common.SystemModule.Dtos;
using Gromi.Infra.Utils.Helpers;
using Gromi.Repository.Common.SystemModule;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Application.Common.SystemModule
{
    /// <summary>
    /// 页面服务
    /// </summary>
    public interface IPageService
    {
        /// <summary>
        /// 添加页面
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult> AddPageRoute(PageRouteDto param);
    }

    [AutoInject(ServiceLifetime.Scoped)]
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;

        public PageService(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public async Task<BaseResult> AddPageRoute(PageRouteDto param)
        {
            BaseResult result = new BaseResult() { Code = ResponseCodeEnum.InternalError };

            try
            {
                var validateRes = new PageRouteValidator().Validate(param);
                if (!validateRes.IsValid)
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Message = string.Join(";", validateRes.Errors);
                    return result;
                }

                var addRes = await _pageRepository.InsertPageRouteAsync(param.Adapt<PageRoute>());
                result.Code = addRes ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Message = addRes ? "添加成功" : "添加失败";
            }
            catch (Exception ex)
            {
                result.Message = $"页面添加失败:{ex.Message}";
                LogHelper.Error(result.Message);
            }
            return result;
        }
    }
}