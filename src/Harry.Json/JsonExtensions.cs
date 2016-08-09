#if !NET20
using System;
using System.Collections.Generic;

namespace Harry.Json
{
    public static class JsonExtensions
    {
        public static string ToJson(this object value)
        {
            return JsonHelper.SerializeObject(value);
        }
    }
}

#endif