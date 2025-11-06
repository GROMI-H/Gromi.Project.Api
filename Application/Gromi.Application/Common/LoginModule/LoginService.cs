using Gromi.Application.Common.AuthModule;
using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule;
using Gromi.Infra.DataAccess.Shared;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Constant;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.Common.LoginModule.Dtos;
using Gromi.Infra.Entity.Common.LoginModule.Params;
using Gromi.Infra.Utils.Helpers;
using Gromi.Repository.Common.SystemModule;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Application.Common.LoginModule
{
    /// <summary>
    /// 登录服务接口
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseResult> Register(RegisterParam param);

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        Task<BaseResult<byte[]>> GetCaptcha();

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        Task<BaseResult<LoginResponse>> Login(LoginParam loginParam);
    }

    /// <summary>
    /// 登录服务实现
    /// </summary>
    [AutoInject(ServiceLifetime.Scoped)]
    public class LoginService : ILoginService
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly RedisServer<string> _redisServer;

        public LoginService(IJwtService jwtService, IUserRepository userRepository, RedisServer<string> redisServer)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
            _redisServer = redisServer;
        }

        public async Task<BaseResult> Register(RegisterParam param)
        {
            BaseResult result = new BaseResult() { Code = ResponseCodeEnum.InternalError };

            try
            {
                if (string.IsNullOrEmpty(param.Name) || string.IsNullOrEmpty(param.Account) || string.IsNullOrEmpty(param.Password))
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Msg = "注册失败，参数有误";
                    return result;
                }

                var addRes = await _userRepository.InsertAsync(param.Adapt<UserInfo>());

                result.Code = addRes != null ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Msg = addRes != null ? "注册成功" : "注册失败";

                return result;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE constraint failed"))
                {
                    result.Code = ResponseCodeEnum.Fail;
                    result.Msg = $"注册失败，当前账号已存在";
                }
                else
                {
                    result.Msg = $"注册失败,{ex.Message}";
                }
                LogHelper.Error(result.Msg);
                return await Task.FromResult(result);
            }
        }

        public async Task<BaseResult<byte[]>> GetCaptcha()
        {
            try
            {
                BaseResult<byte[]> result = new BaseResult<byte[]>();

                var captchaData = CaptchaHelper.GenerateCaptcha();

                SessionHelper.SetSession(CommonConstant.CaptchaKey, captchaData.Code);

                result.Code = ResponseCodeEnum.Success;
                result.Data = captchaData.Image;

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"获取验证码图片失败:{ex.Message}");
                return await Task.FromResult(new BaseResult<byte[]>(ResponseCodeEnum.InternalError, ex.Message));
            }
        }

        public async Task<BaseResult<LoginResponse>> Login(LoginParam loginParam)
        {
            try
            {
                BaseResult<LoginResponse> result = new BaseResult<LoginResponse>();

                if (string.IsNullOrEmpty(loginParam.Account) || string.IsNullOrEmpty(loginParam.Password))
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Msg = "账号或密码不能为空";
                    return result;
                }

                long verifyRes = -1;
                var sessionCaptcha = SessionHelper.GetSession(CommonConstant.CaptchaKey);
                if (sessionCaptcha != null && loginParam.Captcha.ToUpper() == sessionCaptcha.ToString())
                {
                    verifyRes = await _userRepository.VerifyPassword(loginParam.Account, loginParam.Password);
                }
                else
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Msg = "验证码错误,请重试";
                    return result;
                }
                if (verifyRes != -1)
                {
                    var userInfo = await _userRepository.GetModelAsync(verifyRes);
                    var tokenDto = await _jwtService.CreateToken(userInfo);
                    if (tokenDto == null || tokenDto.Data == null)
                    {
                        result.Code = ResponseCodeEnum.InternalError;
                        result.Msg = $"登录失败：创建Token失败";
                        return result;
                    }

                    result.Data = userInfo.Adapt<LoginResponse>();
                    result.Data.Token = tokenDto.Data.Token;
                    result.Msg = "登录成功";
                    return result;
                }
                else
                {
                    result.Code = ResponseCodeEnum.Fail;
                    result.Msg = "密码校验失败";
                }

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"登录失败:{ex.Message}");
                return await Task.FromResult(new BaseResult<LoginResponse>(ResponseCodeEnum.InternalError, ex.Message));
            }
        }
    }
}