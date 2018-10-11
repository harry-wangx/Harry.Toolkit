using Harry.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Harry
{
    [DebuggerStepThrough]
    public static class Check
    {
        #region NotNull
#if COREFX || NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void NotNull<T>(T value, string paramName) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
        }

#if COREFX || NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void NotNull<T>(T? value, string paramName) where T : struct
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
        }
        #endregion

        #region NotEmpty
#if COREFX || NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void NotNullOrEmpty(string value, string paramName)
        {
            if (!value.HasValue())
                throw new ArgumentNullException(paramName);
        }
        #endregion

    }
}
