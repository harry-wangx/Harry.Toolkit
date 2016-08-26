namespace Harry.Metrics
{
    public interface IMeter: ResetableMetric
    {

        void Mark();

        void Mark(string item);

        void Mark(long count);

        void Mark(string item, long count);
    }
}
