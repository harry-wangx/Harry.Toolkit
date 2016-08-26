namespace Harry.Metrics
{
    public interface ICounter : ResetableMetric
    {
        void Increment();

        void Increment(string item);

        void Increment(long amount);

        void Increment(string item, long amount);

        void Decrement();

        void Decrement(string item);

        void Decrement(long amount);

        void Decrement(string item, long amount);
    }
}
