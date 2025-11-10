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

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult> ResetPassrod(BaseParam param);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult> UpdateUserInfo(UserInfoDto param);
    }

    [AutoInject(ServiceLifetime.Scoped)]
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUsersRolesRepository _usersRolesRepository;

        public UserService(IUserRepository userRepository, IUsersRolesRepository usersRolesRepository)
        {
            _userRepository = userRepository;
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

        public async Task<BaseResult> ResetPassrod(BaseParam param)
        {
            BaseResult result = new BaseResult
            {
                Code = ResponseCodeEnum.InternalError
            };
            try
            {
                if (param.Id == null)
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Msg = "重置失败，参数有误";
                    return result;
                }
                var resetRes = await _userRepository.ResetPassword(param.Id.Value, EncryptHelper.Md5("123456"));
                result.Code = resetRes ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Msg = resetRes ? "重置成功" : "重置失败";
            }
            catch (Exception ex)
            {
                result.Msg = $"重置失败:{ex.Message}";
                LogHelper.Error(result.Msg);
            }
            return result;
        }

        public async Task<BaseResult> UpdateUserInfo(UserInfoDto param)
        {
            BaseResult result = new BaseResult();

            try
            {
                var updateRes = await _userRepository.UpdateAsync(param.Adapt<UserInfo>());
                result.Code = updateRes ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Msg = updateRes ? "更新成功" : "更新失败";
            }
            catch (Exception ex)
            {
                result.Msg = $"更新失败:{ex.Message}";
                LogHelper.Error(result.Msg);
            }

            return result;
        }
    }
}