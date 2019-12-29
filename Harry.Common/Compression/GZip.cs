﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Harry.Compression
{
    public static class GZip
    {
        private const int _DefaultCopyBufferSize = 40960; //81920;

        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())
            using (GZipStream zipStream = new GZipStream(ms, CompressionMode.Compress))
            {
                zipStream.Write(data, 0, data.Length);//将数据压缩并写到基础流中
#if !NET40
                zipStream.FlushAsync().Wait();
#else
                zipStream.Close();
#endif
                return ms.ToArray();
            }

        }

        /// <summary>
        /// 解压数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] data)
        {
            using (MemoryStream originalStream = new MemoryStream(data))
            using (GZipStream decompressionStream = new GZipStream(originalStream, CompressionMode.Decompress))
            using (MemoryStream ms = new MemoryStream())
            {
#if !NET40
                decompressionStream.CopyTo(ms);
#else
                byte[] buffer = new byte[_DefaultCopyBufferSize];
                int read;
                while ((read = decompressionStream.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, read);
#endif
                return ms.ToArray();
            }
        }


    }
}
