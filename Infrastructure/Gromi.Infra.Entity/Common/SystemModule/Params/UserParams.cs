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
}