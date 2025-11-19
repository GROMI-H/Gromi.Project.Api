using Gromi.Infra.Entity.Common.BaseModule.Params;

namespace Gromi.Infra.Entity.Common.SystemModule.Params
{
    /// <summary>
    /// 角色绑定参数
    /// </summary>
    public class BindRoleParam
    {
        /// <summary>
        /// 待绑用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 待绑角色ID集合
        /// </summary>
        public List<long> RoleIds { get; set; }
    }

    /// <summary>
    /// 用户信息查询参数
    /// </summary>
    public class QueryUserParam : BaseParam
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户名-模糊查询
        /// </summary>
        public string UserName { get; set; }
    }
}