using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Harry.Services
{
    public abstract class ServiceBase : IService
    {
        private readonly ILogger _logger;

        public ServiceBase(ILoggerFactory loggerFactory) : this(loggerFactory, String.Empty)
        {

        }

        public ServiceBase(ILoggerFactory loggerFactory, string name)
        {
            _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());

            if (name != null)
                this.Name = name;
        }
        public string Name { get; set; } = string.Empty;

        private volatile bool _isStarted = false;
        public bool IsStarted => _isStarted;

        protected ILogger Logger => _logger;

        public async Task StartAsync(CancellationToken token)
        {
            if (_isStarted)
            {
                _logger.LogDebug($"[{this.Name}]已启动,退出重复启动");
                return;
            }

            try
            {
                _isStarted = await OnStartAsync(token);

                if (_isStarted)
                {

                    //注册取消回调
                    if (token != CancellationToken.None)
                    {
                        token.Register(Stop);
                    }

                    if (token.IsCancellationRequested)
                    {
                        Stop();
                        return;
                    }

                    _logger.LogInformation($"[{this.Name}]已启动");
                }
            }
            catch (Exception ex)
            {
                _isStarted = false;
                _logger.LogError(ex, $"启动[{this.Name}]失败");
            }
        }

        protected virtual Task<bool> OnStartAsync(CancellationToken token)
        {
            return Task.FromResult(true);
        }

        protected virtual void OnStop()
        {

        }

        private void Stop()
        {
            if (!_isStarted) return;
            try
            {
                this.OnStop();
            }
            catch { }

            _isStarted = false;
            _logger.LogInformation($"[{this.Name}]已停止");
        }
    }
}
