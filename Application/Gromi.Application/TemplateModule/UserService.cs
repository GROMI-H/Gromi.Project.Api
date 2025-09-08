using Gromi.Infra.Entity.CommonModule.Attributes;
using Gromi.Infra.Entity.CommonModule.Dtos;
using Gromi.Infra.Entity.CommonModule.Enums;
using Gromi.Infra.Entity.TemplateModule.Dtos;
using Gromi.Infra.Repository;
using Gromi.Infra.Utils.Helpers;
using Gromi.Repository.TemplateModule;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Application.TemplateModule
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 创建用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResult<UserInfo>> CreateUserInfo(UserInfo model);

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        Task<BaseResult<IEnumerable<UserInfo>>> GetAllUserInfo();

        /// <summary>
        /// 获取指定用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<UserInfo>> GetUserInfoById(long id);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResult> UpdateUserInfo(UserInfo model);

        /// <summary>
        /// 删除指定用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult> DeleteUserInfo(long id);
    }

    /// <summary>
    /// 用户服务实现
    /// </summary>
    [AutoInject(ServiceLifetime.Scoped, "Temp")]
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<UserInfo> _userBaseRespory;

        public UserService(IUserRepository userRepository, IRepository<UserInfo> repository)
        {
            _userRepository = userRepository;
            _userBaseRespory = repository;
        }

        public async Task<BaseResult<UserInfo>> CreateUserInfo(UserInfo model)
        {
            try
            {
                BaseResult<UserInfo> result = new BaseResult<UserInfo>();

                var createRes = await _userBaseRespory.InsertAsync(model);
                result.Code = createRes != null ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Msg = createRes != null ? "用户创建成功" : "用户创建失败";
                result.Data = createRes;
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"用户创建失败:{ex.Message}");
                return await Task.FromResult(new BaseResult<UserInfo>(ResponseCodeEnum.InternalError, $"用户创建失败:{ex.Message}"));
            }
        }

        public async Task<BaseResult<IEnumerable<UserInfo>>> GetAllUserInfo()
        {
            try
            {
                BaseResult<IEnumerable<UserInfo>> result = new BaseResult<IEnumerable<UserInfo>>();
                var queryRes = await _userBaseRespory.GetAllAsync();
                bool isQuery = queryRes != null && queryRes.Count > 0;
                result.Code = isQuery ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Msg = isQuery ? "查询成功" : "查询失败";
                result.Data = queryRes;
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"查询所有用户信息失败：{ex.Message}");
                return await Task.FromResult(new BaseResult<IEnumerable<UserInfo>>(ResponseCodeEnum.InternalError, $"查询所有用户信息失败：{ex.Message}", Enumerable.Empty<UserInfo>()));
            }
        }

        public async Task<BaseResult<UserInfo>> GetUserInfoById(long id)
        {
            try
            {
                BaseResult<UserInfo> result = new BaseResult<UserInfo>();
                var queryRes = await _userBaseRespory.GetModelAsync(id);
                bool isQuery = queryRes != null;
                result.Code = isQuery ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Msg = isQuery ? "查询成功" : "查询失败";
                result.Data = queryRes;
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"查询用户信息失败：{ex.Message}");
                return await Task.FromResult(new BaseResult<UserInfo>(ResponseCodeEnum.InternalError, $"查询用户信息失败：{ex.Message}", null));
            }
        }

        public async Task<BaseResult> UpdateUserInfo(UserInfo model)
        {
            try
            {
                BaseResult result = new BaseResult();
                var queryRes = await _userBaseRespory.UpdateAsync(model);
                result.Code = queryRes ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Msg = queryRes ? "更新成功" : "更新失败";
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"更新用户信息失败：{ex.Message}");
                return await Task.FromResult(new BaseResult(ResponseCodeEnum.InternalError, $"更新用户信息失败：{ex.Message}"));
            }
        }

        public async Task<BaseResult> DeleteUserInfo(long id)
        {
            try
            {
                BaseResult result = new BaseResult();
                var queryRes = await _userBaseRespory.DeleteAsync(id);
                result.Code = queryRes ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Msg = queryRes ? "删除成功" : "删除失败";
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"删除用户信息失败：{ex.Message}");
                return await Task.FromResult(new BaseResult(ResponseCodeEnum.InternalError, $"删除用户信息失败：{ex.Message}"));
            }
        }
    }
}