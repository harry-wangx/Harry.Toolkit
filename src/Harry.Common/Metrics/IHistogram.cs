namespace Harry.Metrics
{
    public interface IHistogram : ResetableMetric
    {
        void Update(long value, string userValue = null);
    }
}
