namespace Gromi.Infra.Entity.Common.SystemModule.Params
{
    /// <summary>
    /// 接口绑定参数
    /// </summary>
    public class BindApiParam
    {
        /// <summary>
        /// 待绑角色ID
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 待绑接口ID集合
        /// </summary>
        public List<long> ApiIds { get; set; }
    }
}