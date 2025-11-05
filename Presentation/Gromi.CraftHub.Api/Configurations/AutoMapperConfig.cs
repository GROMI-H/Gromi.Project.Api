using Gromi.Infra.Entity.Common.BaseModule.Attributes;
using System.Reflection;

namespace Gromi.CraftHub.Api.Configurations
{
    /// <summary>
    /// AutoMapper配置
    /// </summary>
    [Obsolete]
    public static class AutoMapperConfig
    {
        /// <summary>
        /// 添加AutoMapper配置
        /// </summary>
        /// <param name="services"></param>
        [Obsolete]
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            string[] injectKeys = { "Default", "CraftHub", "Memo" };

            // 利用反射获取程序所有加载类型
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Gromi.*.dll").Select(Assembly.LoadFrom).ToList();

            // 注册 Profile
            List<Type> profiles = assemblies.Where(d => d.FullName != null && d.FullName.Split(",")[0].EndsWith(".AutoMapper"))
                .SelectMany(x => x.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttributes(typeof(AutoInjectAttribute), false).Length > 0)
                .Where(t => injectKeys.Contains(t.GetCustomAttribute<AutoInjectAttribute>()?.Key))
                .ToList();
            profiles.ForEach(profile =>
            {
                services.AddAutoMapper(cfg => cfg.AddProfile(profile));
            });
        }
    }
}