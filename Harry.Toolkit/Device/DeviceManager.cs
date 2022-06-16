using System;
using System.Collections.Generic;
using System.Linq;

namespace Harry.Device
{
    public class DeviceManager<T> : IDeviceManager<T>
        where T : class
    {
        private IEnumerable<IDevice> _devices;
        public DeviceManager(IEnumerable<IDevice> devices)
        {
            if (devices == null) throw new ArgumentNullException(nameof(devices));

            _devices = devices.Where(m => m is T);

            var repeat = _devices.GroupBy(m => m.Name).Where(m => m.Count() > 1);
            if (repeat.Any())
            {
                throw new ArgumentException($"{typeof(T)}类型设备,名称[{string.Join(",", repeat.Select(m => m.Key))}]有重复（忽略大小写）");
            }
            _devices = _devices.ToArray();
        }

        public T Get(string name)
        {
            return (T)_devices.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<T> GetAll()
        {
            return _devices.Select(m => (T)m);
        }
    }
}
