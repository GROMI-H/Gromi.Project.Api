namespace Gromi.Infra.Entity.Common.AuthModule.Params
{
    /// <summary>
    /// 登录参数
    /// </summary>
    public class LoginParam
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Captcha { get; set; }
    }

    /// <summary>
    /// 注册参数
    /// </summary>
    public class RegisterParam
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}