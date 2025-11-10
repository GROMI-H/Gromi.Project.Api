using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule;
using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule.Relations;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.Common.BaseModule.Params;
using Gromi.Infra.Entity.Common.SystemModule.Dtos;
using Gromi.Infra.Entity.Common.SystemModule.Params;
using Gromi.Infra.Utils.Helpers;
using Gromi.Repository.Common.SystemModule;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Application.Common.SystemModule
{
    /// <summary>
    /// 角色服务接口
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// 添加系统角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<BaseResult<SystemRoleDto>> AddSystemRole(SystemRoleDto role);

        /// <summary>
        /// 角色接口绑定
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult> BindApis(BindApiParam param);

        /// <summary>
        /// 角色接口解绑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult> UnBindApis(BindApiParam param);

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult<SystemRole>> GetSystemRole(BaseParam param);

        /// <summary>
        /// 验证当前角色是否支持路由
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<BaseResult> VerifyUrl(List<long> roleIds, string? url);
    }

    [AutoInject(ServiceLifetime.Scoped)]
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRolesApisRepository _routesApisRepository;

        public RoleService(IRoleRepository roleRepository, IRolesApisRepository routesApisRepository)
        {
            _roleRepository = roleRepository;
            _routesApisRepository = routesApisRepository;
        }

        public async Task<BaseResult<SystemRoleDto>> AddSystemRole(SystemRoleDto role)
        {
            BaseResult<SystemRoleDto> result = new BaseResult<SystemRoleDto>
            {
                Code = ResponseCodeEnum.InternalError,
                Data = role
            };

            try
            {
                var addRes = await _roleRepository.InsertAsync(role.Adapt<SystemRole>());
                if (addRes != null)
                {
                    result.Code = ResponseCodeEnum.Success;
                    result.Data = addRes.Adapt<SystemRoleDto>();
                    result.Message = "角色添加成功";
                }
                else
                {
                    result.Code = ResponseCodeEnum.Fail;
                    result.Message = "角色添加失败";
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Message = $"角色添加失败:{ex.Message}";
                LogHelper.Error(result.Message);
                return await Task.FromResult(result);
            }
        }

        public async Task<BaseResult> BindApis(BindApiParam param)
        {
            BaseResult result = new BaseResult
            {
                Code = ResponseCodeEnum.InternalError
            };
            try
            {
                if (param.RoleId == 0 || !param.ApiIds.Any())
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Message = "绑定失败，参数有误";
                    return result;
                }

                var roleApi = new List<RolesApis>();
                foreach (var item in param.ApiIds)
                {
                    roleApi.Add(new RolesApis { RoleId = param.RoleId, ApiId = item });
                }
                await _routesApisRepository.InsertRolesApisAsync(roleApi);
                result.Code = ResponseCodeEnum.Success;
                result.Message = "绑定成功";
                return result;
            }
            catch (Exception ex)
            {
                result.Message = $"绑定失败:{ex.Message}";
                LogHelper.Error(result.Message);
                return await Task.FromResult(result);
            }
        }

        public async Task<BaseResult> UnBindApis(BindApiParam param)
        {
            BaseResult result = new BaseResult
            {
                Code = ResponseCodeEnum.InternalError
            };
            try
            {
                if (param.RoleId == 0 || !param.ApiIds.Any())
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Message = "解绑失败，参数有误";
                    return result;
                }

                var roleApi = new List<RolesApis>();
                foreach (var item in param.ApiIds)
                {
                    roleApi.Add(new RolesApis { RoleId = param.RoleId, ApiId = item });
                }
                await _routesApisRepository.DeleteRolesApisAsync(roleApi);
                result.Code = ResponseCodeEnum.Success;
                result.Message = "解绑成功";
                return result;
            }
            catch (Exception ex)
            {
                result.Message = $"解绑失败:{ex.Message}";
                LogHelper.Error(result.Message);
                return await Task.FromResult(result);
            }
        }

        public Task<BaseResult<SystemRole>> GetSystemRole(BaseParam param)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResult> VerifyUrl(List<long> roleIds, string? url)
        {
            BaseResult result = new BaseResult
            {
                Code = ResponseCodeEnum.InternalError
            };
            try
            {
                if (!roleIds.Any() || string.IsNullOrEmpty(url))
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Message = "校验失败，参数有误";
                    return result;
                }

                var verifyRes = await _routesApisRepository.VerifyUrlAsync(roleIds, url);
                result.Code = verifyRes ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Message = verifyRes ? "校验成功" : "校验失败";
                return result;
            }
            catch (Exception ex)
            {
                result.Message = $"解绑失败:{ex.Message}";
                LogHelper.Error(result.Message);
                return await Task.FromResult(result);
            }
        }
    }
}