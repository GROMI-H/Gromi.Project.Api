using Gromi.Infra.Entity.Common.BaseModule.Settings;
using Quartz;
using System.Reflection;

namespace Gromi.CraftHub.Api.Configurations
{
    /// <summary>
    /// Quartz调度配置
    /// </summary>
    public static class QuartzConfig
    {
        /// <summary>
        /// 添加任务调度配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddQuartzConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var jobConfigs = configuration.GetSection("Jobs").Get<List<JobConfig>>();
            if (jobConfigs == null || !jobConfigs.Any()) return;

            // 注册 Quartz
            services.AddQuartz(options =>
            {
                var currentAssembly = Assembly.GetExecutingAssembly();
                var jobTypes = currentAssembly
                    .GetTypes()
                    .Where(t => typeof(IJob).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                    .ToList();

                foreach (var config in jobConfigs.Where(j => j.Enable))
                {
                    var jobType = jobTypes.FirstOrDefault(t => t.Name.Equals(config.Name, StringComparison.OrdinalIgnoreCase));
                    if (jobType == null)
                    {
                        throw new Exception($"未找到名为 {config.Name} 的任务类");
                    }

                    var jobKey = new JobKey(config.Name);

                    options.AddJob(jobType, jobKey, opts => { });

                    // 为空则只执行一次
                    if (string.IsNullOrEmpty(config.CronExpression))
                    {
                        options.AddTrigger(triggerOptions =>
                        {
                            triggerOptions
                                .ForJob(jobKey)
                                .WithIdentity($"{config.Name}.trigger")
                                .StartNow();
                        });
                    }
                    else
                    {
                        options.AddTrigger(triggerOptions =>
                        {
                            triggerOptions
                                .ForJob(jobKey)
                                .WithIdentity($"{config.Name}.trigger")
                                .WithCronSchedule(config.CronExpression);
                        });
                    }
                }
            });

            services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);
        }
    }
}