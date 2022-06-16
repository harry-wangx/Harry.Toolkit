using Harry.Services;
using System.Collections.Generic;

namespace Harry.Device
{
    public interface IDeviceManager<T> where T : class
    {
        T Get(string name);

        IEnumerable<T> GetAll();
    }
}
