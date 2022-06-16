using System.Threading;
using System.Threading.Tasks;

namespace Harry.Services
{
    public interface IService
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 是否已正常启动
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task StartAsync(CancellationToken token);
    }
}
