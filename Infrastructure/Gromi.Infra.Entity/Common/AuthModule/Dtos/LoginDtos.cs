using Gromi.Infra.Entity.Common.BaseModule.Params;

namespace Gromi.Infra.Entity.Common.AuthModule.Dtos
{
    /// <summary>
    /// 登录响应
    /// </summary>
    public class LoginResponse : BaseParam
    {
        /// <summary>
        /// JwtToken
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
    }
}