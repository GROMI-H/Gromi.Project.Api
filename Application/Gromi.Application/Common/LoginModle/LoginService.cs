using AutoMapper;
using Gromi.Application.Common.AuthModule;
using Gromi.Infra.DataAccess.DbEntity.Common.SystemModule;
using Gromi.Infra.DataAccess.Shared;
using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using Gromi.Infra.Entity.Common.BaseModule.Constant;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.Common.LoginModule.Dtos;
using Gromi.Infra.Entity.Common.LoginModule.Params;
using Gromi.Infra.Entity.Common.SystemModule.Dtos;
using Gromi.Infra.Utils.Helpers;
using Gromi.Repository.Common.SystemModule;
using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Application.Common.LoginModle
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
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly RedisServer<string> _redisServer;

        public LoginService(IMapper mapperr, IJwtService jwtService, IUserRepository userRepository, RedisServer<string> redisServer)
        {
            _mapper = mapperr;
            _jwtService = jwtService;
            _userRepository = userRepository;
            _redisServer = redisServer;
        }

        public async Task<BaseResult> Register(RegisterParam param)
        {
            try
            {
                BaseResult result = new BaseResult();
                var addRes = await _userRepository.InsertAsync(_mapper.Map<UserInfo>(param));

                result.Code = addRes != null ? ResponseCodeEnum.Success : ResponseCodeEnum.Fail;
                result.Msg = addRes != null ? "注册成功" : "注册失败";

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"注册失败：{ex.Message}");
                return await Task.FromResult(new BaseResult(ResponseCodeEnum.Fail, "注册失败"));
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
                    var tokenDto = await _jwtService.CreateToken(_mapper.Map<UserInfoDto>(userInfo));
                    if (tokenDto == null || tokenDto.Data == null)
                    {
                        result.Code = ResponseCodeEnum.InternalError;
                        result.Msg = $"登录失败：创建Token失败";
                        return result;
                    }

                    result.Data = _mapper.Map<LoginResponse>(userInfo);
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