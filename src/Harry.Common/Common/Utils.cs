﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


#if !COREFX
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
#endif

namespace Harry.Common
{
    public static class Utils
    {
        public const string NET35 = "NET35";
        public const string NET40 = "NET40";
        public const string NET45 = "NET45";
        public const string COREFX = "COREFX";

#if NET35
        public static readonly string DOTNET_VERSION = NET35;
#endif
#if NET40
        public static readonly string DOTNET_VERSION = NET40;
#endif
#if NET45
        public static readonly string DOTNET_VERSION = NET45;
#endif
#if COREFX
        public static readonly string DOTNET_VERSION = COREFX;
#endif


#if !COREFX
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
#endif
    }
}
