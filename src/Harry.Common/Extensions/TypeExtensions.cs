#if !NET20
using System;
using System.Linq;
using System.Reflection;

namespace Harry.Extensions
{
    public static class TypeExtensions
    {
        public static Type BaseType(this Type type)
        {
            Throws.IfNull(type, nameof(type));
#if COREFX
            return type.GetTypeInfo().BaseType;
#else
            return type.BaseType;
#endif
        }

        public static bool IsGenericType(this Type type)
        {
            Harry.Throws.IfNull(type, nameof(type));
#if COREFX
            return type.GetTypeInfo().IsGenericType;
#else
            return type.IsGenericType;
#endif
        }

        public static Type DeclaringType(this Type type)
        {
            Harry.Throws.IfNull(type, nameof(type));
#if COREFX
            return type.GetTypeInfo().DeclaringType;
#else
            return type.DeclaringType;
#endif
        }


        public static bool IsDefined(this Type type, Type attributeType, bool inherit)
        {
            Throws.IfNull(type, nameof(type));
            Throws.IfNull(attributeType, nameof(attributeType));
#if COREFX
            return type.GetTypeInfo().IsDefined(attributeType, inherit);
#else
            return type.IsDefined(attributeType,inherit);
#endif
        }
    }
}

#endif