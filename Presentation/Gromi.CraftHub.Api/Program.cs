using Gromi.CraftHub.Api.Configurations;
using Gromi.CraftHub.Api.Middlewares;
using Gromi.Infra.Utils.Helpers;
using Gromi.Infra.Utils.Utils;

namespace Gromi.CraftHub.Api
{
    /// <summary>
    /// 主程序
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 程序入口
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Load Configuration File

            // 使用内置机制，根据环境自动导入配置文件
            //builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

            #endregion Load Configuration File

            #region Add services to the container

            bool enableSwagger = Convert.ToBoolean(builder.Configuration["EnableSwagger"]);

            LogHelper.Initialize(builder.Configuration["LogPath"] ?? AppDomain.CurrentDomain.BaseDirectory);

            // 配置跨域
            builder.Services.AddCorsConfiguration(builder.Configuration["DefaultCorePolicyName"] ?? "TempCors");

            // 添加控制器服务
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();

            // 配置自动注入
            builder.Services.AddAutoInjectConfiguration();
            // 配置AutoMapper映射
            //builder.Services.AddAutoMapperConfiguration(); // 因转为收费，2025-11-05弃用
            // 配置Mapster映射
            builder.Services.AddMapsterConfiguration();
            // 配置Swagger
            builder.Services.AddSwaggerConfiguration(enableSwagger);
            // 配置FreeSql
            builder.Services.AddFreeSqlConfiguration(builder.Configuration);
            // 配置 其他工具
            builder.Services.AddOtherConfiguration();
            // 配置Redis
            builder.Services.AddEasyCacheConfiguration(builder.Configuration);
            // 配置Jwt
            builder.Services.AddJwtConfiguration(builder.Configuration);
            // 配置Quartz.NET
            builder.Services.AddQuartzConfiguration(builder.Configuration);

            #endregion Add services to the container

            var app = builder.Build();

            #region Middleware Configuration

            var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
            SessionHelper.Init(httpContextAccessor);

            app.UseMiddleware<ExceptionHandleMiddleware>();

            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwaggerSetup(enableSwagger);
            //}
            app.UseSession();

            app.UseRouting();
            app.UseAuthentication(); // 身份认证
            app.UseAuthorization(); // 授权检查

            #endregion Middleware Configuration

            // 全局缓冲，解决request中获取body为空情况
            app.Use(next => context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            });
            app.MapControllers();
            app.Run();
        }
    }
}