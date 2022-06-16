using Harry.Device;
using Harry.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DeviceServiceCollectionExtensions
    {
        /// <summary>
        /// 添加设备管理器
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddDeviceManager(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //注册设备管理器
            if (!services.Any(m => m.ServiceType == typeof(IDeviceManager<>)))
            {
                services.Add(ServiceDescriptor.Singleton(typeof(IDeviceManager<>), typeof(DeviceManager<>)));
            }

            //注册设备服务
            if (!services.Any(m => m.ServiceType == typeof(IService) && m.ImplementationType == typeof(DeviceService)))
            {
                services.Add(ServiceDescriptor.Singleton<IService, DeviceService>());
            }
            return services;
        }

        public static IServiceCollection AddDeviceManager(this IServiceCollection services, Action<IDeviceBuilder> builder)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            AddDeviceManager(services);

            DeviceBuilder deviceBuilder = new DeviceBuilder(services);
            builder?.Invoke(deviceBuilder);

            return services;
        }

        /// <summary>
        /// 注册设备
        /// </summary>
        /// <typeparam name="TDevice"></typeparam>
        /// <param name="builder"></param>
        /// <param name="implementationFactory"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IDeviceBuilder AddDevice<TDevice>(this IDeviceBuilder builder, Func<IServiceProvider, TDevice> implementationFactory)
            where TDevice : class, IDevice
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));


            if (!builder.Services.Any(m => m.ServiceType == typeof(IDevice) && m.ImplementationType == typeof(TDevice)))
            {
                builder.Services.Add(ServiceDescriptor.Singleton<IDevice, TDevice>(implementationFactory));
            }
            return builder;
        }
    }
}
