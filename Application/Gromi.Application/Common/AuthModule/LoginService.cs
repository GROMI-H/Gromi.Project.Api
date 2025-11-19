using Gromi.Application.Validator.Common.SystemModule;
using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule;
using Gromi.Infra.DataAccess.Shared;
using Gromi.Infra.Entity.Common.AuthModule.Dtos;
using Gromi.Infra.Entity.Common.AuthModule.Params;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Constant;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.Common.SystemModule.Params;
using Gromi.Infra.Utils.Helpers;
using Gromi.Repository.Common.SystemModule;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Application.Common.AuthModule
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
        Task<BaseResult<string>> GetCaptcha();

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
                var validateRes = new RegisterValidator().Validate(param);
                if (!validateRes.IsValid)
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Message = string.Join(";", validateRes.Errors);
                    return result;
                }
                // 密码加盐
                string salt = EncryptHelper.Md5(GeneratorHelper.GenerateRandomString(5));
                param.Password = EncryptHelper.Md5(param.Password + salt);

                var userInfo = param.Adapt<UserInfo>();
                userInfo.Salt = salt;
                var addRes = await _userRepository.InsertAsync(userInfo);

                result.Code = addRes != null ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Message = addRes != null ? "注册成功" : "注册失败";

                return result;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE constraint failed"))
                {
                    result.Code = ResponseCodeEnum.Fail;
                    result.Message = $"注册失败，当前账号已存在";
                }
                else
                {
                    result.Message = $"注册失败,{ex.Message}";
                }
                LogHelper.Error(result.Message);
                return await Task.FromResult(result);
            }
        }

        public async Task<BaseResult<string>> GetCaptcha()
        {
            try
            {
                BaseResult<string> result = new BaseResult<string>();

                var captchaData = CaptchaHelper.GenerateCaptcha();
                SessionHelper.SetSession(CommonConstant.CaptchaKey, captchaData.Code);
                SessionHelper.SetSession(CommonConstant.CaptchaExpireKey, DateTime.UtcNow.AddMinutes(1).ToString("o"));
                result.Code = ResponseCodeEnum.Success;
                result.Data = $"data:image/png;base64,{Convert.ToBase64String(captchaData.Image)}";

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"获取验证码图片失败:{ex.Message}");
                return await Task.FromResult(new BaseResult<string>(ResponseCodeEnum.InternalError, ex.Message));
            }
        }

        public async Task<BaseResult<LoginResponse>> Login(LoginParam loginParam)
        {
            try
            {
                BaseResult<LoginResponse> result = new BaseResult<LoginResponse>()
                {
                    Data = new LoginResponse()
                };

                if (string.IsNullOrEmpty(loginParam.Account) || string.IsNullOrEmpty(loginParam.Password))
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Message = "账号或密码不能为空";
                    return result;
                }

                long verifyRes = -1;
                var sessionCaptcha = SessionHelper.GetSession(CommonConstant.CaptchaKey);
                var sessionCaptchaExpire = SessionHelper.GetSession(CommonConstant.CaptchaExpireKey);
                if (sessionCaptchaExpire == null || !DateTime.TryParse(sessionCaptchaExpire.ToString(), out DateTime expirationTime) || DateTime.UtcNow > expirationTime)
                {
                    result.Code = ResponseCodeEnum.Timeout;
                    result.Message = "验证码已过期,请刷新重试";
                    return result;
                }
                SessionHelper.RemoveSession(CommonConstant.CaptchaKey); // 获取后就删除指定Key
                if (sessionCaptcha != null && loginParam.Captcha.ToUpper() == sessionCaptcha.ToString())
                {
                    var userInfo = await _userRepository.GetUserInfoAsync(new QueryUserParam { Account = loginParam.Account });
                    if (userInfo == null)
                    {
                        result.Code = ResponseCodeEnum.NotFound;
                        result.Message = "未查询到用户信息";
                        return result;
                    }
                    verifyRes = await _userRepository.VerifyPassword(loginParam.Account, EncryptHelper.Md5(loginParam.Password + userInfo.Salt));
                }
                else
                {
                    result.Code = ResponseCodeEnum.InvalidParameter;
                    result.Message = "验证码错误,请重试";
                    return result;
                }
                if (verifyRes != -1)
                {
                    var userInfo = await _userRepository.GetModelAsync(verifyRes);
                    var tokenDto = await _jwtService.CreateToken(userInfo);
                    if (tokenDto == null || tokenDto.Data == null)
                    {
                        result.Code = ResponseCodeEnum.InternalError;
                        result.Message = $"登录失败：创建Token失败";
                        return result;
                    }
                    result.Code = ResponseCodeEnum.Success;
                    result.Data = userInfo.Adapt<LoginResponse>();
                    result.Data.Token = tokenDto.Data.Token;
                    result.Message = "登录成功";
                    return result;
                }
                else
                {
                    result.Code = ResponseCodeEnum.Fail;
                    result.Message = "密码校验失败";
                }

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"登录失败:{ex.Message}");
                return await Task.FromResult(new BaseResult<LoginResponse>(ResponseCodeEnum.InternalError, ex.Message, new LoginResponse()));
            }
        }
    }
}