using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harry.Extensions
{

    public static class BitConvertExtensions
    {
        #region Int16,UInt16
        /// <summary>
        /// 转换为Int16
        /// </summary>
        /// <param name="buffer">原始数据</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="isLittleEndian">是否小端模式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public unsafe static Int16 ToInt16(this byte[] buffer, int startIndex, bool isLittleEndian)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (startIndex < 0 || startIndex >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (startIndex > buffer.Length - 2)
            {
                throw new ArgumentException($"当前{nameof(startIndex)}索引开始,剩余已不足2字节");
            }

            return ToInt16(new ReadOnlySpan<byte>(buffer, startIndex, 2), isLittleEndian);
        }

        /// <summary>
        /// 转换为Int16
        /// </summary>
        /// <param name="buffer">原始数据</param>
        /// <param name="isLittleEndian">是否小端模式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public unsafe static Int16 ToInt16(this in ReadOnlySpan<byte> buffer, bool isLittleEndian)
        {
            if (buffer.Length < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(buffer), $"{nameof(buffer)}长度不能小于2");
            }

            fixed (byte* ptr = &buffer[0])
            {
                if (isLittleEndian)
                {
                    return (short)(*ptr | (ptr[1] << 8));
                }
                else
                    return (short)((*ptr << 8) | ptr[1]);
            }
        }

        /// <summary>
        /// 转换为UInt16
        /// </summary>
        /// <param name="buffer">原始数据</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="isLittleEndian">是否小端模式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public unsafe static UInt16 ToUInt16(this byte[] buffer, int startIndex, bool isLittleEndian)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (startIndex < 0 || startIndex >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (startIndex > buffer.Length - 2)
            {
                throw new ArgumentException($"当前{nameof(startIndex)}索引开始,剩余已不足2字节");
            }

            return (UInt16)ToInt16(new ReadOnlySpan<byte>(buffer, startIndex, 2), isLittleEndian);
        }

        /// <summary>
        /// 转换为UInt16
        /// </summary>
        /// <param name="buffer">原始数据</param>
        /// <param name="isLittleEndian">是否小端模式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public unsafe static UInt16 ToUInt16(this in ReadOnlySpan<byte> buffer, bool isLittleEndian)
        {
            if (buffer.Length < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(buffer), $"{nameof(buffer)}长度不能小于2");
            }

            return (UInt16)ToInt16(buffer, isLittleEndian);
        }
        #endregion

        #region Int32,UInt32
        /// <summary>
        /// 转换为Int32
        /// </summary>
        /// <param name="buffer">原始数据</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="isLittleEndian">是否小端模式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public unsafe static Int32 ToInt32(this byte[] buffer, int startIndex, bool isLittleEndian)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (startIndex < 0 || startIndex >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (startIndex > buffer.Length - 4)
            {
                throw new ArgumentException($"当前{nameof(startIndex)}索引开始,剩余已不足4字节");
            }

            return ToInt32(new ReadOnlySpan<byte>(buffer, startIndex, 4), isLittleEndian);
        }

        /// <summary>
        /// 转换为Int32
        /// </summary>
        /// <param name="buffer">原始数据</param>
        /// <param name="isLittleEndian">是否小端模式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public unsafe static Int32 ToInt32(this in ReadOnlySpan<byte> buffer, bool isLittleEndian)
        {
            if (buffer.Length < 4)
            {
                throw new ArgumentOutOfRangeException(nameof(buffer), $"{nameof(buffer)}长度不能小于4");
            }

            fixed (byte* ptr = &buffer[0])
            {
                if (isLittleEndian)
                {
                    return *ptr | (ptr[1] << 8) | (ptr[2] << 16) | (ptr[3] << 24);
                }
                else
                    return (*ptr << 24) | (ptr[1] << 16) | (ptr[2] << 8) | ptr[3];
            }
        }

        /// <summary>
        /// 转换为UInt32
        /// </summary>
        /// <param name="buffer">原始数据</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="isLittleEndian">是否小端模式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public unsafe static UInt32 ToUInt32(this byte[] buffer, int startIndex, bool isLittleEndian)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (startIndex < 0 || startIndex >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (startIndex > buffer.Length - 4)
            {
                throw new ArgumentException($"当前{nameof(startIndex)}索引开始,剩余已不足4字节");
            }

            return (UInt32)ToInt32(new ReadOnlySpan<byte>(buffer, startIndex, 4), isLittleEndian);
        }

        /// <summary>
        /// 转换为UInt32
        /// </summary>
        /// <param name="buffer">原始数据</param>
        /// <param name="isLittleEndian">是否小端模式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public unsafe static UInt32 ToUInt32(this in ReadOnlySpan<byte> buffer, bool isLittleEndian)
        {
            if (buffer.Length < 4)
            {
                throw new ArgumentOutOfRangeException(nameof(buffer), $"{nameof(buffer)}长度不能小于4");
            }

            return (UInt32)ToInt32(buffer, isLittleEndian);
        }
        #endregion

        #region Int64,UInt64
        /// <summary>
        /// 转换为Int64
        /// </summary>
        /// <param name="buffer">原始数据</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="isLittleEndian">是否小端模式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public unsafe static Int64 ToInt64(this byte[] buffer, int startIndex, bool isLittleEndian)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (startIndex < 0 || startIndex >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (startIndex > buffer.Length - 8)
            {
                throw new ArgumentException($"当前{nameof(startIndex)}索引开始,剩余已不足8字节");
            }

            return ToInt64(new ReadOnlySpan<byte>(buffer, startIndex, 8), isLittleEndian);
        }

        /// <summary>
        /// 转换为Int64
        /// </summary>
        /// <param name="buffer">原始数据</param>
        /// <param name="isLittleEndian">是否小端模式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public unsafe static Int64 ToInt64(this in ReadOnlySpan<byte> buffer, bool isLittleEndian)
        {
            if (buffer.Length < 8)
            {
                throw new ArgumentOutOfRangeException(nameof(buffer), $"{nameof(buffer)}长度不能小于8");
            }

            fixed (byte* ptr = &buffer[0])
            {
                if (isLittleEndian)
                {
                    int num = *ptr | (ptr[1] << 8) | (ptr[2] << 16) | (ptr[3] << 24);
                    int num2 = ptr[4] | (ptr[5] << 8) | (ptr[6] << 16) | (ptr[7] << 24);
                    return (uint)num | ((long)num2 << 32);
                }
                else
                {
                    int num3 = (*ptr << 24) | (ptr[1] << 16) | (ptr[2] << 8) | ptr[3];
                    int num4 = (ptr[4] << 24) | (ptr[5] << 16) | (ptr[6] << 8) | ptr[7];
                    return (uint)num4 | ((long)num3 << 32);
                }
            }
        }

        /// <summary>
        /// 转换为UInt64
        /// </summary>
        /// <param name="buffer">原始数据</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="isLittleEndian">是否小端模式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public unsafe static UInt64 ToUInt64(this byte[] buffer, int startIndex, bool isLittleEndian)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (startIndex < 0 || startIndex >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (startIndex > buffer.Length - 8)
            {
                throw new ArgumentException($"当前{nameof(startIndex)}索引开始,剩余已不足8字节");
            }

            return (UInt64)ToInt64(new ReadOnlySpan<byte>(buffer, startIndex, 8), isLittleEndian);
        }

        /// <summary>
        /// 转换为UInt64
        /// </summary>
        /// <param name="buffer">原始数据</param>
        /// <param name="isLittleEndian">是否小端模式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public unsafe static UInt64 ToUInt64(this in ReadOnlySpan<byte> buffer, bool isLittleEndian)
        {
            if (buffer.Length < 8)
            {
                throw new ArgumentOutOfRangeException(nameof(buffer), $"{nameof(buffer)}长度不能小于8");
            }

            return (UInt64)ToInt64(buffer, isLittleEndian);
        }
        #endregion
    }
}
