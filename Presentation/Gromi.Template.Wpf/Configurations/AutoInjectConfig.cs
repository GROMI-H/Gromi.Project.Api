using Gromi.Infra.Entity.CommonModule.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;

namespace Gromi.Template.Wpf.Configurations
{
    /// <summary>
    /// 领域服务配置
    /// </summary>
    public static class AutoInjectConfig
    {
        /// <summary>
        /// 自动注入
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddAutoInjectConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            string[] injectKeys = { "Default" };
            // 利用反射获取程序所有加载类型
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Gromi.*.dll").Select(Assembly.LoadFrom).ToList();
            if (assemblies != null && assemblies.Count > 0)
            {
                List<Type> types = assemblies.Where(d => d.FullName != null && d.FullName.Split(",")[0].EndsWith(".Wpf"))
                    .SelectMany(x => x.GetTypes())
                    .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttributes(typeof(AutoInjectAttribute), false).Length > 0)
                    .Where(t => injectKeys.Contains(t.GetCustomAttribute<AutoInjectAttribute>()?.Key))
                    .ToList();
                types.ForEach(impl =>
                {
                    // 获取该类注入的生命周期
                    ServiceLifetime? lifetime = impl.GetCustomAttribute<AutoInjectAttribute>()?.Lifetime;

                    switch (lifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(impl);
                            break;

                        case ServiceLifetime.Scoped:
                            services.AddScoped(impl);
                            break;

                        case ServiceLifetime.Transient:
                            services.AddTransient(impl);
                            break;
                    }
                });
            }
        }
    }
}