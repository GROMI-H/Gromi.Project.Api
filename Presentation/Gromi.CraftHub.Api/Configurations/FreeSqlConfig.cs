using FreeSql;
using Gromi.Infra.Entity.Common.Attributes;
using Gromi.Infra.Entity.Common.Enums;
using Gromi.Infra.Entity.Common.Settings;
using Gromi.Infra.Repository.Shared;
using Gromi.Infra.Utils.Helpers;
using System.Reflection;
using Yitter.IdGenerator;

namespace Gromi.CraftHub.Api.Configurations
{
    /// <summary>
    /// FreeSql配置
    /// </summary>
    public static class FreeSqlConfig
    {
        /// <summary>
        /// 添加FreeSql配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddFreeSqlConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            Dictionary<DbKey, DbConnectionConfig> dicDbConnections = new Dictionary<DbKey, DbConnectionConfig>
            {
                {DbKey.DbCraftHub, configuration.GetSection("FreeSqlConfig").Get<DbConnectionConfig>() ?? new DbConnectionConfig() }
            };

            var multiFreeSql = new MultiFreeSqlManager<DbKey>();

            if (dicDbConnections[DbKey.DbCraftHub] != null)
            {
                multiFreeSql.Register(DbKey.DbCraftHub, () => new Lazy<IFreeSql>(() =>
                {
                    var dbType = dicDbConnections[DbKey.DbCraftHub].DbMode switch
                    {
                        DbMode.SQLite => DataType.Sqlite,
                        DbMode.MySQL => DataType.MySql,
                        _ => throw new NotSupportedException($"不支持的数据库类型：{dicDbConnections[DbKey.DbCraftHub].DbMode}")
                    };

                    IFreeSql fsql = new FreeSqlBuilder()
                        .UseConnectionString(dbType, dicDbConnections[DbKey.DbCraftHub].ConnectionString)
                        .UseAdoConnectionPool(true)
                        .UseAutoSyncStructure(dicDbConnections[DbKey.DbCraftHub].EnableAutoSyncStructure) // 自动同步实体结构
                        .UseMonitorCommand(cmd => LogHelper.Info($"SQL:{cmd.CommandText}")) // 监听并输出SQL
                        .UseLazyLoading(dicDbConnections[DbKey.DbCraftHub].EnableLazyLoading) // 开启实体的延迟加载：如果实体有导航属性(外键),访问时才会去数据库加载；会增加SQL次数，但简化了代码
                        .Build();

                    // 审计属性值
                    fsql.Aop.AuditValue += (s, e) =>
                    {
                        // 1.ID列 自动填充雪花ID
                        if (e.Column.CsType == typeof(long) && e.Property.GetCustomAttribute<SnowflakeAttribute>(false) != null && (e.Value == null || (long)e.Value == 0))
                        {
                            e.Value = YitIdHelper.NextId();
                        }

                        // 2.时间字段处理
                        if (e.Column.CsType == typeof(DateTime))
                        {
                            if (e.Property.Name == "CreateTime")
                            {
                                if (e.AuditValueType == FreeSql.Aop.AuditValueType.Insert)
                                {
                                    e.Value = DateTime.Now;
                                }
                            }
                            if (e.Property.Name == "UpdateTime")
                            {
                                if (e.Value == null || (DateTime)e.Value == default)
                                {
                                    e.Value = DateTime.Now;
                                }
                            }
                        }
                    };

                    return fsql;
                }).Value);
            }

            services.AddSingleton<IMultiFreeSqlManager<DbKey>>(multiFreeSql);
        }
    }
}