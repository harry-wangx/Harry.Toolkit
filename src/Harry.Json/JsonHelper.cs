using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Harry.Json
{
    public static class JsonHelper
    {
        //设置日期时间的格式，与DataTime类型的ToString格式相同
        static IsoDateTimeConverter iso = new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };

        /// <summary>
        /// 序列化对像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SerializeObject(object input)
        {
            return JsonConvert.SerializeObject(input,iso);
        }

        /// <summary>
        /// 反序列化对像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static object DeserializeObject(string input)
        {
            return JsonConvert.DeserializeObject(input);
        }

        /// <summary>
        /// 反序列化对像
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }

        //public static string GetJsonValue(string input,string key)
        //{

        //}
    }
}
