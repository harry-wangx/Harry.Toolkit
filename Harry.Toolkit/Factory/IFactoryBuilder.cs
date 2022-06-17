using Microsoft.Extensions.DependencyInjection;

namespace Harry.Factory
{
    public interface IFactoryBuilder
    {
        IServiceCollection Services { get; }
    }
}
