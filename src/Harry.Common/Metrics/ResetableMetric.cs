
using Harry.Common;

namespace Harry.Metrics
{
    public interface ResetableMetric:IHideObjectMembers
    {
        void Reset();
    }
}
