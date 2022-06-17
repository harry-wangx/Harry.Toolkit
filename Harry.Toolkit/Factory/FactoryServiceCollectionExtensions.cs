using Harry.Factory;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FactoryServiceCollectionExtensions
    {
        public static IServiceCollection AddFactory(this IServiceCollection services, Action<IFactoryBuilder> configure)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));

            //注册工厂
            if (!services.Any(m => m.ServiceType == typeof(IFactory<>)))
            {
                services.Add(ServiceDescriptor.Singleton(typeof(IFactory<>), typeof(DefaultFactory<>)));
            }

            configure?.Invoke(new FactoryBuilder(services));
            return services;
        }
    }
}
