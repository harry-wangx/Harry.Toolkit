using System.Collections.Generic;

namespace Harry.Tree
{
    public static class Extensions
    {
        /// <summary>
        /// 转换成树型数据
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="model"></param>
        /// <param name="open"></param>
        /// <returns></returns>
        public static TreeNode<TKey> ToTreeNode<TKey>(this ITreeNode<TKey> model, bool? open = null)
        {
            return new TreeNode<TKey>()
            {
                Id = model.Id,
                Name = model.Name,
                Open = open == null ? false : open.Value
            };
        }

        /// <summary>
        /// 获取树形数据
        /// </summary>
        public static List<TreeNode<TKey>> ToTreeData<TKey>(this IEnumerable<ITreeNode<TKey>> data, TreeNode<TKey> root)
        {
            root.Open = true;
            var dicData = new Dictionary<TKey, List<ITreeNode<TKey>>>();
            foreach (var item in data)
            {
                if (dicData.TryGetValue(item.ParentId, out List<ITreeNode<TKey>> list))
                {
                    list.Add(item);
                }
                else
                {
                    dicData.Add(item.ParentId, new List<ITreeNode<TKey>>() { item });
                }
            }

            List<TreeNode<TKey>> results = new List<TreeNode<TKey>>();
            results.Add(root);
            if (root.Children == null)
            {
                root.Children = new List<TreeNode<TKey>>();
            }
            getTreeData(dicData, root.Id, root.Children);

            return results;
        }

        /// <summary>
        /// 获取树形数据
        /// </summary>
        public static List<TreeNode<TKey>> ToTreeData<TKey>(this IEnumerable<ITreeNode<TKey>> data, string rootName, TKey rootId = default)
        {
            return ToTreeData(data, new TreeNode<TKey>() { Name = rootName, Id = rootId });
        }

        /// <summary>
        /// 获取树形数据(没有单一根节点)
        /// </summary>
        public static List<TreeNode<TKey>> ToTreeData<TKey>(this IEnumerable<ITreeNode<TKey>> data, TKey parentId)
        {
            var dicData = new Dictionary<TKey, List<ITreeNode<TKey>>>();
            foreach (var item in data)
            {
                if (dicData.TryGetValue(item.ParentId, out List<ITreeNode<TKey>> list))
                {
                    list.Add(item);
                }
                else
                {
                    dicData.Add(item.ParentId, new List<ITreeNode<TKey>>() { item });
                }
            }

            List<TreeNode<TKey>> results = new List<TreeNode<TKey>>();
            getTreeData(dicData, parentId, results);
            return results;
        }

        /// <summary>
        /// 获取ZTree数据
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="data">所有数据</param>
        /// <param name="pid">父级ID</param>
        /// <param name="nodes">结果集合</param>
        private static void getTreeData<TKey>(Dictionary<TKey, List<ITreeNode<TKey>>> data, TKey pid, List<TreeNode<TKey>> nodes)
        {
            if (data.TryGetValue(pid, out List<ITreeNode<TKey>> list))
            {
                foreach (var item in list)
                {
                    //生成节点
                    var node = item.ToTreeNode();
                    nodes.Add(node);

                    //获取子节点
                    var children = new List<TreeNode<TKey>>();
                    getTreeData(data, node.Id, children);

                    if (children.Count > 0)
                    {
                        node.Children = children;
                    }
                }
            }
        }
    }
}
