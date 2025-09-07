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
    }
}