﻿#if !NET20
using System;
using System.Collections.Generic;
using System.Linq;

namespace Harry.Domain
{
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        protected abstract IEnumerable<object> GetAttributesToIncludeInEqualityCheck();

        public override bool Equals(object obj)
        {
            return Equals(obj as T);
        }

        public bool Equals(T other)
        {
            if (other == null)
            {
                return false;
            }
            return GetAttributesToIncludeInEqualityCheck().SequenceEqual(other.GetAttributesToIncludeInEqualityCheck());
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (var obj in this.GetAttributesToIncludeInEqualityCheck())
            {
                hash = hash * 31 + (obj == null ? 0 : obj.GetHashCode());
            }
            return hash;
        }

        public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
        {
            return !(left == right);
        }
    }
}

#endif