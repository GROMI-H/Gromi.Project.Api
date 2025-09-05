using Gromi.Infra.Utils.Helpers;
using Microsoft.OpenApi.Models;

namespace Gromi.Template.Api.Configurations
{
    /// <summary>
    /// Swagger 配置
    /// </summary>
    public static class SwaggerConfig
    {
        /// <summary>
        /// Swagger 注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="enableSwagger"></param>
        public static void AddSwaggerConfiguration(this IServiceCollection services, bool enableSwagger = false)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (enableSwagger)
            {
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Gromi Template API",
                        Version = "v1",
                        Description = "API for Gromi Template application",
                    });
                });
            }
        }

        /// <summary>
        /// Swagger 安装
        /// </summary>
        /// <param name="app"></param>
        /// <param name="enableSwagger"></param>
        public static void UseSwaggerSetup(this IApplicationBuilder app, bool enableSwagger = false)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (enableSwagger)
            {
                app.UseSwagger(); // 启用 Swagger 中间件
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gromi Template API v1");
                    c.RoutePrefix = string.Empty; // 使 Swagger UI 在根路径可用
                });
            }
        }
    }
}