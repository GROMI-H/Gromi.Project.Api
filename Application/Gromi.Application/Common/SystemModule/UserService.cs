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

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult> DeleteUser(BaseDeleteParam param);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult<UserInfoDto>> GetUserInfo(BaseParam param);
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
                    result.Message = "绑定失败，参数有误";
                    return result;
                }

                var userRole = new List<UsersRoles>();
                foreach (var item in param.RoleIds)
                {
                    userRole.Add(new UsersRoles { UserId = param.UserId, RoleId = item });
                }
                await _usersRolesRepository.InsertUsersRolesAsync(userRole);
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
                    result.Message = "解绑失败，参数有误";
                    return result;
                }

                var userRole = new List<UsersRoles>();
                foreach (var item in param.RoleIds)
                {
                    userRole.Add(new UsersRoles { UserId = param.UserId, RoleId = item });
                }
                await _usersRolesRepository.DeleteUsersRolesAsync(userRole);
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
                    result.Message = "重置失败，参数有误";
                    return result;
                }

                var userInfo = await _userRepository.GetModelAsync(param.Id.Value);
                if (userInfo == null)
                {
                    result.Code = ResponseCodeEnum.NotFound;
                    result.Message = "当前用户不存在";
                    return result;
                }
                var resetRes = await _userRepository.ResetPassword(param.Id.Value, EncryptHelper.Md5("123456" + userInfo.Salt));
                result.Code = resetRes ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Message = resetRes ? "重置成功" : "重置失败";
            }
            catch (Exception ex)
            {
                result.Message = $"重置失败:{ex.Message}";
                LogHelper.Error(result.Message);
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
                result.Message = updateRes ? "更新成功" : "更新失败";
            }
            catch (Exception ex)
            {
                result.Message = $"更新失败:{ex.Message}";
                LogHelper.Error(result.Message);
            }

            return result;
        }

        public async Task<BaseResult> DeleteUser(BaseDeleteParam param)
        {
            BaseResult result = new BaseResult() { Code = ResponseCodeEnum.InternalError };

            try
            {
                if (param.Id == null && param.Ids.Count == 0)
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Message = "删除失败，用户Id为空";
                    return result;
                }
                if (param.Id != null)
                {
                    param.Ids.Add(param.Id.Value);
                }

                var delRes = await _userRepository.BatchDeleteUserAsync(param.Ids);
                result.Code = delRes ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Message = delRes ? "删除成功" : "删除失败";
            }
            catch (Exception ex)
            {
                result.Message = $"删除失败{ex.Message}";
                LogHelper.Error(result.Message);
            }
            return result;
        }

        public async Task<BaseResult<UserInfoDto>> GetUserInfo(BaseParam param)
        {
            BaseResult<UserInfoDto> result = new BaseResult<UserInfoDto>
            {
                Code = ResponseCodeEnum.InternalError,
                Data = new UserInfoDto()
            };

            try
            {
                if (param.Id == null)
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Message = "参数有误";
                    return result;
                }
                var queryRes = await _userRepository.GetModelAsync(param.Id.Value);
                result.Code = queryRes != null ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Message = queryRes != null ? "查询成功" : "数据不存在";
                if (queryRes != null)
                {
                    result.Data = queryRes.Adapt<UserInfoDto>();
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Message = $"查询失败:{ex.Message}";
                LogHelper.Error(result.Message);
            }
            return result;
        }
    }
}