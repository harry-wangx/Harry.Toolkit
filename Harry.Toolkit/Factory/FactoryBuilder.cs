using Microsoft.Extensions.DependencyInjection;

namespace Harry.Factory
{
    internal sealed class FactoryBuilder : IFactoryBuilder
    {
        public FactoryBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
