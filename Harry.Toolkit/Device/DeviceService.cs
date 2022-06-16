using Harry.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Harry.Device
{
    internal class DeviceService : IService
    {
        private IEnumerable<IDevice> _devices;
        public DeviceService(ILoggerFactory loggerFactory, IEnumerable<IDevice> devices)
        {
            _devices = devices ?? throw new ArgumentNullException(nameof(devices));
        }

        public string Name => "Device服务";

        public bool IsStarted { get; private set; }

        public Task StartAsync(CancellationToken token)
        {
            Task.WaitAll(_devices.Select(m =>
            {
                var task = m.StartAsync(token);
                task.ConfigureAwait(false);
                return task;
            }).ToArray(), token);
            return Task.CompletedTask;
        }
    }
}
