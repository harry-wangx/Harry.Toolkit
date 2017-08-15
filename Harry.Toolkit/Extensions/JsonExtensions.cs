using Harry.Json;
using System;
using System.Collections.Generic;

namespace Harry.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson(this object value)
        {
            return JsonHelper.SerializeObject(value);
        }
    }
}
