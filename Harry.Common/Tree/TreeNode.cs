using System.Collections.Generic;

namespace Harry.Tree
{
    public class TreeNode<TKey>
    {
        public TKey Id { get; set; }

        public string Name { get; set; }

        public bool Open { get; set; }

        /// <summary>
        /// 获取是否为父节点
        /// </summary>
        public bool IsParent
        {
            get
            {
                return Children != null && Children.Count > 0;
            }
        }

        public List<TreeNode<TKey>> Children { get; set; }
    }
}
