using Gromi.Application.Common.SystemModule;
using Gromi.Infra.Entity.Common.BaseModule.Enums;
using Gromi.Infra.Entity.Common.SystemModule.Dtos;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using System.Reflection;

namespace Gromi.CraftHub.Api.Jobs
{
    /// <summary>
    /// 注册Api路由定时任务
    /// </summary>
    public class RegisterApiRouteJob : IJob
    {
        private readonly IApiRouteService _apiRouteService;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="apiRouteService"></param>
        public RegisterApiRouteJob(IApiRouteService apiRouteService)
        {
            _apiRouteService = apiRouteService;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Execute(IJobExecutionContext context)
        {
            var controllers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract);

            var apiRoutes = new List<ApiRouteDto>();

            foreach (var controller in controllers)
            {
                // 获取控制器中的所有方法
                var actionMethods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(m => typeof(IActionResult).IsAssignableFrom(m.ReturnType) || m.ReturnType == typeof(Task<IActionResult>));

                foreach (var method in actionMethods)
                {
                    // 获取所有 HTTP 动作属性
                    var httpAttributes = method.GetCustomAttributes()
                        .Where(attr => attr is HttpGetAttribute
                            || attr is HttpPostAttribute
                            || attr is HttpPutAttribute
                            || attr is HttpDeleteAttribute
                            || attr is HttpPatchAttribute)
                        .ToList();

                    foreach (var httpAttribute in httpAttributes)
                    {
                        string template = string.Empty;
                        RouteTypeEnum routeType = RouteTypeEnum.GET;

                        // 根据属性类型获取路由模板
                        switch (httpAttribute)
                        {
                            case HttpGetAttribute getAttr:
                                template = getAttr.Template ?? "";
                                routeType |= RouteTypeEnum.GET;
                                break;

                            case HttpPostAttribute postAttr:
                                template = postAttr.Template ?? "";
                                routeType |= RouteTypeEnum.POST;
                                break;

                            case HttpPutAttribute putAttr:
                                template = putAttr.Template ?? "";
                                routeType |= RouteTypeEnum.PUT;
                                break;

                            case HttpDeleteAttribute deleteAttr:
                                template = deleteAttr.Template ?? "";
                                routeType |= RouteTypeEnum.DELETE;
                                break;

                            case HttpPatchAttribute patchAttr:
                                template = patchAttr.Template ?? "";
                                routeType |= RouteTypeEnum.PATCH;
                                break;
                        }

                        var summary = method
                            .GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true)
                            .FirstOrDefault() as System.ComponentModel.DescriptionAttribute;

                        apiRoutes.Add(new ApiRouteDto()
                        {
                            Name = controller.Name,
                            Route = template,
                            Description = summary?.Description,
                            RouteType = routeType
                        });
                    }
                }
            }

            var res = await _apiRouteService.UpsertApiRoute(apiRoutes);
            return;
        }
    }
}