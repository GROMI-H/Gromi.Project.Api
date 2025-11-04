namespace Gromi.Infra.Entity.Common.BaseModule.Settings
{
    /// <summary>
    /// 定时任务配置
    /// </summary>
    public class JobConfig
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
}