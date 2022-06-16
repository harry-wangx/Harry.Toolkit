using Microsoft.Extensions.DependencyInjection;
using System;

namespace Harry.Device
{
    public class DeviceBuilder: IDeviceBuilder
    {
        public DeviceBuilder(IServiceCollection services)
        {
            this.Services = services ?? throw new ArgumentNullException(nameof(services));
        }
        public IServiceCollection Services { get; private set; }
    }
}
