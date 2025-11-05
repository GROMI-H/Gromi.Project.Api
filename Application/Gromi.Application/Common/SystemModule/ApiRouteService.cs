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
    /// 接口路由服务接口
    /// </summary>
    public interface IApiRouteService
    {
        /// <summary>
        /// 更新添加接口路由
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult> UpsertApiRoute(List<ApiRouteDto> param);
    }

    /// <summary>
    /// 接口路由服务实现
    /// </summary>
    [AutoInject(ServiceLifetime.Scoped)]
    public class ApiRouteService : IApiRouteService
    {
        private readonly IApiRouteRepository _apiRouteRepository;

        public ApiRouteService(IApiRouteRepository apiRouteRepository)
        {
            _apiRouteRepository = apiRouteRepository;
        }

        public async Task<BaseResult> UpsertApiRoute(List<ApiRouteDto> param)
        {
            BaseResult result = new BaseResult
            {
                Code = ResponseCodeEnum.Fail
            };
            try
            {
                if (param == null || !param.Any())
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Msg = "更新失败,数据为空";
                    return result;
                }

                await _apiRouteRepository.UpsertApiRouteAsync(param.Adapt<IEnumerable<ApiRoute>>());
                result.Code = ResponseCodeEnum.Success;
                result.Msg = "更新成功";

                return result;
            }
            catch (Exception ex)
            {
                result.Msg = $"接口路由更新失败:{ex.Message}";
                LogHelper.Error(result.Msg);
                return await Task.FromResult(result);
            }
        }
    }
}