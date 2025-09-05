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

            // ʹ�����û��ƣ����ݻ����Զ����������ļ�
            //builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

            #endregion Load Configuration File

            #region Add services to the container

            bool enableSwagger = Convert.ToBoolean(builder.Configuration["EnableSwagger"]);

            LogHelper.Initialize(builder.Configuration["LogPath"] ?? AppDomain.CurrentDomain.BaseDirectory);

            // ���ÿ���
            builder.Services.AddCorsConfiguration(builder.Configuration["DefaultCorePolicyName"] ?? "TempCors");

            // ��ӿ���������
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();

            // �����Զ�ע��
            builder.Services.AddAutoInjectConfiguration();
            // ����Swagger
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