using System.ComponentModel.DataAnnotations;

namespace Gromi.Infra.Entity.CraftHub.MemoModule.Enums
{
    /// <summary>
    /// 笔记类型
    /// </summary>
    public enum NoteType
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Display(Name = "默认")]
        Default = 0,

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        Password = 1,

        /// <summary>
        /// 流程
        /// </summary>
        [Display(Name = "流程")]
        Flow = 2
    }
}