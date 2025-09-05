using Gromi.Infra.Utils.Helpers;
using Gromi.Template.Api.Configurations;

namespace Gromi.Template.Api
{
    public class Program
    {
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
            // 配置Swagger
            builder.Services.AddSwaggerConfiguration(enableSwagger);

            #endregion Add services to the container

            var app = builder.Build();

            #region Middleware Configuration

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerSetup(enableSwagger);
            }

            app.UseAuthorization();

            #endregion Middleware Configuration

            app.MapControllers();
            app.Run();
        }
    }
}