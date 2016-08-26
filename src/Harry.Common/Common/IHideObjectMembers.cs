using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Harry.Common
{
    /// <summary>
    /// 用来隐藏Object对像的默认方法
    /// </summary>
    public interface IHideObjectMembers
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();
    }
}
