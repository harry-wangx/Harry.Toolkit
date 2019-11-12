namespace Harry.Tree
{
    public interface ITreeNode<TKey>
    {
        TKey GetId();

        string GetName();

        TKey GetParentId();
    }
}
