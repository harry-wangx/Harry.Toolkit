using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harry.Device
{
    public static class DeviceManagerExtensions
    {
        public static T Get<T>(this IDeviceManager<T> manager)
            where T : class
        {
            return manager.Get(string.Empty);
        }

        public static bool Get<T>(this IDeviceManager<T> manager, ref T value)
            where T : class
        {
            value = manager.Get();
            return value != null;
        }

        public static bool Get<T>(this IDeviceManager<T> manager, string name, ref T value)
            where T : class
        {
            value = manager.Get(name);
            return value != null;
        }
    }
}
