using System;
using System.Collections.Generic;

namespace Harry.Tree
{
    public static class Extensions
    {
        /// <summary>
        /// 转换成树型数据
        /// </summary>
        public static TreeNode<TKey> ToTreeNode<TKey>(this ITreeNode<TKey> model, Action<ITreeNode<TKey>, TreeNode<TKey>> act = null)
        {
            var node = new TreeNode<TKey>()
            {
                Id = model.Id,
                Name = model.Name
            };
            act?.Invoke(model, node);
            return node;
        }

        /// <summary>
        /// 转换成树型数据
        /// </summary>
        public static TNode ToTreeNode<TKey, TNode>(this ITreeNode<TKey> model, Action<ITreeNode<TKey>, TNode> act = null)
            where TNode : TreeNode<TKey, TNode>, new()
        {
            var node = new TNode()
            {
                Id = model.Id,
                Name = model.Name,
            };
            act?.Invoke(model, node);
            return node;
        }

        /// <summary>
        /// 获取树形数据
        /// </summary>
        public static List<TNode> ToTreeData<TKey, TNode>(this IEnumerable<ITreeNode<TKey>> data, TNode root, Action<ITreeNode<TKey>, TNode> act = null)
            where TNode : TreeNode<TKey, TNode>, new()
        {
            Check.NotNull(data, nameof(data));

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

            List<TNode> results = new List<TNode>();
            results.Add(root);
            if (root.Children == null)
            {
                root.Children = new List<TNode>();
            }
            getTreeData(dicData, root.Id, root.Children, act);

            return results;
        }

        /// <summary>
        /// 获取树形数据
        /// </summary>
        public static List<TreeNode<TKey>> ToTreeData<TKey>(this IEnumerable<ITreeNode<TKey>> data, TreeNode<TKey> root, Action<ITreeNode<TKey>, TreeNode<TKey>> act = null)
        {
            Check.NotNull(data, nameof(data));

            return data.ToTreeData<TKey, TreeNode<TKey>>(root, act);
        }

        /// <summary>
        /// 获取树形数据(没有单一根节点)
        /// </summary>
        public static List<TNode> ToTreeData<TKey, TNode>(this IEnumerable<ITreeNode<TKey>> data, TKey parentId, Action<ITreeNode<TKey>, TNode> act = null)
            where TNode : TreeNode<TKey, TNode>, new()
        {
            Check.NotNull(data, nameof(data));

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

            List<TNode> results = new List<TNode>();
            getTreeData(dicData, parentId, results, act);
            return results;
        }

        public static List<TreeNode<TKey>> ToTreeData<TKey>(this IEnumerable<ITreeNode<TKey>> data, TKey parentId, Action<ITreeNode<TKey>, TreeNode<TKey>> act = null)
        {
            Check.NotNull(data, nameof(data));

            return data.ToTreeData<TKey, TreeNode<TKey>>(parentId, act);
        }

        /// <summary>
        /// 获取ZTree数据
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="data">所有数据</param>
        /// <param name="pid">父级ID</param>
        /// <param name="nodes">结果集合</param>
        private static void getTreeData<TKey, TNode>(Dictionary<TKey, List<ITreeNode<TKey>>> data, TKey pid, List<TNode> nodes, Action<ITreeNode<TKey>, TNode> act = null)
            where TNode : TreeNode<TKey, TNode>, new()
        {
            if (data.TryGetValue(pid, out List<ITreeNode<TKey>> list))
            {
                foreach (var item in list)
                {
                    //生成节点
                    var node = item.ToTreeNode(act: act);
                    nodes.Add(node);

                    //获取子节点
                    var children = new List<TNode>();
                    getTreeData(data, node.Id, children, act);

                    if (children.Count > 0)
                    {
                        node.Children = children;
                    }
                }
            }
        }
    }
}
