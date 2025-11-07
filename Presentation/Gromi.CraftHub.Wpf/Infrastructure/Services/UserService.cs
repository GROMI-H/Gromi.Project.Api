using Gromi.CraftHub.Wpf.Infrastructure.Common;
using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.Common.SystemModule.Params;
using Gromi.Infra.Utils.Helpers;
using Newtonsoft.Json;

namespace Gromi.CraftHub.Wpf.Infrastructure.Services
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// 角色绑定
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<BaseResult> BindRole(BindRoleParam param)
        {
            string jsonParam = JsonConvert.SerializeObject(param);
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                {"Authorization", $"Bearer {GlobalManager.Token}" }
            };
            string opRes = await HttpHelper.PostAsync($"{GlobalManager.BaseUrl}/api/User/BindRole", jsonParam, headers);
            var result = JsonConvert.DeserializeObject<BaseResult>(opRes);
            return result != null ? result : new BaseResult(ResponseCodeEnum.InternalError, "数据返回错误");
        }
    }
}