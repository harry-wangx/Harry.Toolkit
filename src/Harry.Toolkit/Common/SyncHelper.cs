using System;
using System.Collections.Generic;

namespace Harry.Common
{
    public static class SyncHelper
    {
        public static void ExecuteSafely(object sync, Func<bool> canExecute, Action actiontoExecuteSafely)
        {
            if (canExecute())
            {
                lock (sync)
                {
                    if (canExecute())
                    {
                        actiontoExecuteSafely();
                    }
                }
            }
        }
    }
}
