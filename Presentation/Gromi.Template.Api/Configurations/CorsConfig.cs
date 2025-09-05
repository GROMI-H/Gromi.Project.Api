namespace Gromi.Template.Api.Configurations
{
    /// <summary>
    /// 跨域配置
    /// </summary>
    public static class CorsConfig
    {
        /// <summary>
        /// Cors跨域注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="_defaultCorsPolicyName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddCorsConfiguration(this IServiceCollection services, string _defaultCorsPolicyName)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddCors(options =>
            {
                options.AddPolicy(_defaultCorsPolicyName, builder =>
                {
                    builder.WithOrigins("*")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
        }
    }
}