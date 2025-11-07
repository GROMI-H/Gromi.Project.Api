using Gromi.CraftHub.Wpf.Infrastructure.Common;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.Common.LoginModule.Dtos;
using Gromi.Infra.Entity.Common.LoginModule.Params;
using Gromi.Infra.Utils.Helpers;
using Newtonsoft.Json;

namespace Gromi.CraftHub.Wpf.Infrastructure.Services
{
    /// <summary>
    /// 登录服务
    /// </summary>
    public class LoginService
    {
        /// <summary>
        /// 获取验证码图片
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResult> GetCaptcha()
        {
            string queryRes = await HttpHelper.GetAsync($"{GlobalManager.BaseUrl}api/Login/GetCaptcha");
            byte[] imageBytes = Convert.FromBase64String(queryRes);
            return new BaseResult<byte[]>
            {
                Code = ResponseCodeEnum.Success,
                Data = imageBytes
            };
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<BaseResult> Register(RegisterParam param)
        {
            string jsonParam = JsonConvert.SerializeObject(param);
            string registerRes = await HttpHelper.PostAsync($"{GlobalManager.BaseUrl}api/Login/Register", jsonParam);
            var result = JsonConvert.DeserializeObject<BaseResult>(registerRes);
            return result != null ? result : new BaseResult(ResponseCodeEnum.InternalError, "返回错误，请重试");
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<BaseResult<LoginResponse>> Login(LoginParam param)
        {
            string jsonParam = JsonConvert.SerializeObject(param);
            string loginRes = await HttpHelper.PostAsync($"{GlobalManager.BaseUrl}api/Login/Login", jsonParam);
            var result = JsonConvert.DeserializeObject<BaseResult<LoginResponse>>(loginRes);
            if (result != null)
            {
                if (result.Code == ResponseCodeEnum.Success)
                {
                    GlobalManager.Token = result.Data.Token;
                }

                return result;
            }
            return new BaseResult<LoginResponse>(ResponseCodeEnum.InternalError, "返回错误，请重试");
        }
    }
}