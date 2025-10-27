﻿using Microsoft.Extensions.DependencyInjection;

namespace Gromi.Infra.Entity.Common.BaseModule.Attributes
{
    /// <summary>
    /// 自动注入特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoInjectAttribute : Attribute
    {
        public string Key { get; set; }

        public ServiceLifetime Lifetime { get; set; }

        public AutoInjectAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, string key = "Default")
        {
            Lifetime = serviceLifetime;
            Key = key;
        }
    }
}