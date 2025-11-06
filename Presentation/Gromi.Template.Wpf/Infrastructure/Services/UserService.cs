using Gromi.Infra.Entity.Common.BaseModule.Dtos;
using Gromi.Infra.Entity.Common.LoginModule.Params;
using Gromi.Infra.Utils.Helpers;
using Newtonsoft.Json;

namespace Gromi.Template.Wpf.Infrastructure.Services
{
    public class UserService
    {
        public async Task<BaseResult> Register(RegisterParam param)
        {
            var result = await HttpHelper.PostAsync("http://localhost:5093/api/Login/Register", JsonConvert.SerializeObject(param));
            return JsonConvert.DeserializeObject<BaseResult>(result);
        }
    }
}