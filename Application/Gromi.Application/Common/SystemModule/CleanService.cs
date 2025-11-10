using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Utils.Helpers;
using Gromi.Repository.Common.SystemModule;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Application.Common.SystemModule
{
    /// <summary>
    /// 清理服务接口
    /// </summary>
    public interface ICleanService
    {
        Task<BaseResult> ClearSoftDelete();
    }

    [AutoInject(ServiceLifetime.Scoped)]
    public class CleanService : ICleanService
    {
        private readonly IApiRouteRepository _apiRouteRepository;

        public CleanService(IApiRouteRepository apiRouteRepository)
        {
            _apiRouteRepository = apiRouteRepository;
        }

        public async Task<BaseResult> ClearSoftDelete()
        {
            BaseResult result = new BaseResult
            {
                Code = ResponseCodeEnum.InternalError
            };
            try
            {
                await _apiRouteRepository.ClearSoftDelAsync();

                result.Code = ResponseCodeEnum.Success;
                result.Msg = "清理成功";
            }
            catch (Exception ex)
            {
                result.Msg = $"清理中断：{ex.Message}";
                LogHelper.Error(result.Msg);
            }
            return result;
        }
    }
}