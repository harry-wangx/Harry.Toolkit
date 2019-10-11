using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


namespace Harry.Common
{
    public static class Utils
    {
        /// <summary>
        /// 深拷贝(对象应用了SerializableAttribute特性才可以用)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <returns></returns>
        public static T DeepClone<T>(T original) where T : class
        {
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Context = new StreamingContext(StreamingContextStates.Clone);
                formatter.Serialize(stream, original);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
