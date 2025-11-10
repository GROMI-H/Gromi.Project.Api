using Gromi.Application.Common.SystemModule;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Utils.Helpers;
using Quartz;

namespace Gromi.CraftHub.Api.Jobs
{
    /// <summary>
    /// 数据库清理任务
    /// </summary>
    public class DatabaseClearJob : IJob
    {
        private readonly ICleanService _cleanService;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="cleanService"></param>
        public DatabaseClearJob(ICleanService cleanService)
        {
            _cleanService = cleanService;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Execute(IJobExecutionContext context)
        {
            var cleanRes = await _cleanService.ClearSoftDelete();
            if (cleanRes.Code != ResponseCodeEnum.Success)
            {
                LogHelper.Error(cleanRes.Message);
            }
        }
    }
}