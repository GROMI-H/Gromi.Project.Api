using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.LoginModule.Params;
using Gromi.Infra.Utils.Helpers;
using Newtonsoft.Json;
using System.Configuration;

namespace Gromi.CraftHub.Wpf.Infrastructure.Services
{
    /// <summary>
    /// 登录服务
    /// </summary>
    public class LoginService
    {
        private readonly string BaseUrl = ConfigurationManager.AppSettings["BaseUrl"] ?? "http://localhost:5093";

        public async Task<BaseResult> Register(RegisterParam param)
        {
            string jsonParam = JsonConvert.SerializeObject(param);
            var registerRes = await HttpHelper.PostAsync($"{BaseUrl}api/Login/Register", jsonParam);
            return JsonConvert.DeserializeObject<BaseResult>(registerRes);
        }
    }
}