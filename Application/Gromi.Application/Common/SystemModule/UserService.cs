using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule.Relations;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.Common.SystemModule.Params;
using Gromi.Infra.Utils.Helpers;
using Gromi.Repository.Common.SystemModule;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Application.Common.SystemModule
{
    /// <summary>
    /// 系统用户服务
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 用户角色绑定
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult> BindRoles(BindRoleParam param);

        /// <summary>
        /// 用户角色解绑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult> UnBindRoles(BindRoleParam param);
    }

    [AutoInject(ServiceLifetime.Scoped)]
    public class UserService : IUserService
    {
        private readonly IUsersRolesRepository _usersRolesRepository;

        public UserService(IUsersRolesRepository usersRolesRepository)
        {
            _usersRolesRepository = usersRolesRepository;
        }

        public async Task<BaseResult> BindRoles(BindRoleParam param)
        {
            BaseResult result = new BaseResult
            {
                Code = ResponseCodeEnum.InternalError
            };
            try
            {
                if (param.UserId == 0 || !param.RoleIds.Any())
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Msg = "绑定失败，参数有误";
                    return result;
                }

                var userRole = new List<UsersRoles>();
                foreach (var item in param.RoleIds)
                {
                    userRole.Add(new UsersRoles { UserId = param.UserId, RoleId = item });
                }
                await _usersRolesRepository.InsertUsersRolesAsync(userRole);
                result.Code = ResponseCodeEnum.Success;
                result.Msg = "绑定成功";
                return result;
            }
            catch (Exception ex)
            {
                result.Msg = $"绑定失败:{ex.Message}";
                LogHelper.Error(result.Msg);
                return await Task.FromResult(result);
            }
        }

        public async Task<BaseResult> UnBindRoles(BindRoleParam param)
        {
            BaseResult result = new BaseResult
            {
                Code = ResponseCodeEnum.InternalError
            };
            try
            {
                if (param.UserId == 0 || !param.RoleIds.Any())
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Msg = "解绑失败，参数有误";
                    return result;
                }

                var userRole = new List<UsersRoles>();
                foreach (var item in param.RoleIds)
                {
                    userRole.Add(new UsersRoles { UserId = param.UserId, RoleId = item });
                }
                await _usersRolesRepository.DeleteUsersRolesAsync(userRole);
                result.Code = ResponseCodeEnum.Success;
                result.Msg = "解绑成功";
                return result;
            }
            catch (Exception ex)
            {
                result.Msg = $"解绑失败:{ex.Message}";
                LogHelper.Error(result.Msg);
                return await Task.FromResult(result);
            }
        }
    }
}