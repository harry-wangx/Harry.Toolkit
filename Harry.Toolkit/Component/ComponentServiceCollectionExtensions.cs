using Harry.Component;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ComponentServiceCollectionExtensions
    {
        /// <summary>
        /// 添加组件管理器
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddComponentManager(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //注册设备管理器
            if (!services.Any(m => m.ServiceType == typeof(IComponentManager<>)))
            {
                services.Add(ServiceDescriptor.Singleton(typeof(IComponentManager<>), typeof(ComponentManager<>)));
            }
            return services;
        }
    }
}
