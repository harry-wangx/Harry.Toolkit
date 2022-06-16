using Microsoft.Extensions.DependencyInjection;

namespace Harry.Device
{
    public interface IDeviceBuilder
    {
        IServiceCollection Services { get; }
    }
}
