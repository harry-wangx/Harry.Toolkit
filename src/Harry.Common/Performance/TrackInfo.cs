
namespace Harry.Performance
{
    /// <summary>
    /// 跟踪对象信息
    /// </summary>
    public sealed class TrackInfo
    {
        public TrackInfo(string contextName, string name)
        {
            this.ContextName = contextName;
            this.Name = name;
        }
        public string ContextName { get; private set; }

        public string  Name { get;private set; }
    }
}
