using System;
using System.Collections.Generic;

namespace Harry.Tree
{
    public static class Extensions
    {
        /// <summary>
        /// 转换成树型数据
        /// </summary>
        public static TreeNode<TKey> ToTreeNode<TModel, TKey>(this TModel model, Action<TModel, TreeNode<TKey>> act = null)
            where TModel : class, ITreeNode<TKey>
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
        public static TNode ToTreeNode<TModel, TKey, TNode>(this TModel model, Action<TModel, TNode> act = null)
            where TModel : class, ITreeNode<TKey>
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
        public static List<TNode> ToTreeData<TModel, TKey, TNode>(this IEnumerable<TModel> data, TNode root, Action<TModel, TNode> act = null)
            where TModel : class, ITreeNode<TKey>
            where TNode : TreeNode<TKey, TNode>, new()
        {
            Check.NotNull(data, nameof(data));

            root.Open = true;
            var dicData = new Dictionary<TKey, List<TModel>>();
            foreach (var item in data)
            {
                if (dicData.TryGetValue(item.ParentId, out List<TModel> list))
                {
                    list.Add(item);
                }
                else
                {
                    dicData.Add(item.ParentId, new List<TModel>() { item });
                }
            }

            List<TNode> results = new List<TNode>();
            results.Add(root);
            if (root.Children == null)
            {
                root.Children = new List<TNode>();
            }
            getTreeData<TModel, TKey, TNode>(dicData, root.Id, root.Children, act);

            return results;
        }

        /// <summary>
        /// 获取树形数据
        /// </summary>
        public static List<TreeNode<TKey>> ToTreeData<TModel, TKey>(this IEnumerable<TModel> data, TreeNode<TKey> root, Action<TModel, TreeNode<TKey>> act = null)
            where TModel : class, ITreeNode<TKey>
        {
            Check.NotNull(data, nameof(data));

            return data.ToTreeData<TModel, TKey, TreeNode<TKey>>(root, act);
        }

        /// <summary>
        /// 获取树形数据(没有单一根节点)
        /// </summary>
        public static List<TNode> ToTreeData<TModel, TKey, TNode>(this IEnumerable<TModel> data, TKey parentId, Action<TModel, TNode> act = null)
            where TModel : class, ITreeNode<TKey>
            where TNode : TreeNode<TKey, TNode>, new()
        {
            Check.NotNull(data, nameof(data));

            var dicData = new Dictionary<TKey, List<TModel>>();
            foreach (var item in data)
            {
                if (dicData.TryGetValue(item.ParentId, out List<TModel> list))
                {
                    list.Add(item);
                }
                else
                {
                    dicData.Add(item.ParentId, new List<TModel>() { item });
                }
            }

            List<TNode> results = new List<TNode>();
            getTreeData(dicData, parentId, results, act);
            return results;
        }

        public static List<TreeNode<TKey>> ToTreeData<TModel, TKey>(this IEnumerable<TModel> data, TKey parentId, Action<TModel, TreeNode<TKey>> act = null)
            where TModel : class, ITreeNode<TKey>
        {
            Check.NotNull(data, nameof(data));

            return data.ToTreeData<TModel, TKey, TreeNode<TKey>>(parentId, act);
        }

        /// <summary>
        /// 获取ZTree数据
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="data">所有数据</param>
        /// <param name="pid">父级ID</param>
        /// <param name="nodes">结果集合</param>
        private static void getTreeData<TModel, TKey, TNode>(Dictionary<TKey, List<TModel>> data, TKey pid, List<TNode> nodes, Action<TModel, TNode> act = null)
            where TModel : class, ITreeNode<TKey>
            where TNode : TreeNode<TKey, TNode>, new()
        {
            if (data.TryGetValue(pid, out List<TModel> list))
            {
                foreach (var item in list)
                {
                    //生成节点
                    var node = item.ToTreeNode<TModel, TKey, TNode>(act: act);
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
