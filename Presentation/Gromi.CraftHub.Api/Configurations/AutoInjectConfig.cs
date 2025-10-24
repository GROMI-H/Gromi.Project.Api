using Gromi.Infra.Entity.Common.Attributes;
using Gromi.Infra.Repository.Shared;
using System.Reflection;

namespace Gromi.CraftHub.Api.Configurations
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

            string[] injectKeys = { "Default", "CraftHub", "Memo" };
            // 利用反射获取程序所有加载类型
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Gromi.*.dll").Select(Assembly.LoadFrom).ToList();
            if (assemblies != null && assemblies.Count > 0)
            {
                List<Type> types = assemblies.Where(d => d.FullName != null && (d.FullName.Split(",")[0].EndsWith(".Repository") || d.FullName.Split(",")[0].EndsWith(".Application")))
                    .SelectMany(x => x.GetTypes())
                    .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttributes(typeof(AutoInjectAttribute), false).Length > 0)
                    .Where(t => injectKeys.Contains(t.GetCustomAttribute<AutoInjectAttribute>()?.Key))
                    .ToList();

                // 无法自动实例化泛型类 T
                services.AddSingleton(typeof(IRepository<>), typeof(BaseRepository<>));
                types.ForEach(impl =>
                {
                    // 获取该类所有接口
                    Type[] interfaces = impl.GetInterfaces();
                    // 获取该类注入的生命周期
                    ServiceLifetime? lifetime = impl.GetCustomAttribute<AutoInjectAttribute>()?.Lifetime;
                    interfaces.ToList().ForEach(inter =>
                    {
                        switch (lifetime)
                        {
                            case ServiceLifetime.Singleton:
                                services.AddSingleton(inter, impl);
                                break;

                            case ServiceLifetime.Scoped:
                                services.AddScoped(inter, impl);
                                break;

                            case ServiceLifetime.Transient:
                                services.AddTransient(inter, impl);
                                break;
                        }
                    });
                });

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
}