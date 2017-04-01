using System;
using System.Collections.Generic;

namespace Harry.Toolkit.Json
{
    public static class JsonExtensions
    {
        public static string ToJson(this object value)
        {
            return JsonHelper.SerializeObject(value);
        }
    }
}
