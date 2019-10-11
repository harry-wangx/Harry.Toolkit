namespace Harry.Tree
{
    public interface ITreeNode<TKey>
    {
        TKey Id { get; set; }

        string Name { get; set; }

        TKey ParentId { get; set; }
    }
}
