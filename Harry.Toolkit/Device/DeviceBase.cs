using Harry.Services;
using Microsoft.Extensions.Logging;
using System;

namespace Harry.Device
{
    public abstract class DeviceBase : ServiceBase, IDevice
    {
        public DeviceBase(ILoggerFactory loggerFactory) : base(loggerFactory, String.Empty)
        {

        }

        public DeviceBase(ILoggerFactory loggerFactory, string name) : base(loggerFactory, name)
        {
        }
    }
}
