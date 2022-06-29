using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harry.Extensions
{
    public class BitConvertExtensionsTest
    {
        #region 字节数组转Int16测试
        /// <summary>
        /// 转换测试
        /// </summary>
        /// <param name="value"></param>
        [Theory]
        [InlineData(Int16.MinValue)]
        [InlineData(Int16.MaxValue)]
        [InlineData(27837)]
        [InlineData(-1)]
        [InlineData(-21443)]
        public void ArrayToInt16(Int16 value)
        {
            var buf = BitConverter.GetBytes(value);
            var buf2 = buf.Reverse().ToArray();

            var v1 = buf.ToInt16(0, true);
            var v2 = buf2.ToInt16(0, false);

            Assert.Equal(value, v1);
            Assert.Equal(v1, v2);
        }

        /// <summary>
        /// 异常测试
        /// </summary>
        [Fact]
        public void ArrayToInt16_Exception()
        {
            _ = Assert.Throws<ArgumentNullException>(() => ((byte[])null).ToInt16(0, true));
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => (new byte[] { 0x11, 0x22 }).ToInt16(2, true));
            _ = Assert.Throws<ArgumentException>(() => (new byte[] { 0x11, 0x22 }).ToInt16(1, true));
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlySpan<byte>(new byte[] { 0x11, 0x22 }, 0, 1).ToInt16(true));
        }

        /// <summary>
        /// 转换测试
        /// </summary>
        /// <param name="value"></param>
        [Theory]
        [InlineData(UInt16.MinValue)]
        [InlineData(UInt16.MaxValue)]
        [InlineData(27837)]
        public void ArrayToUInt16(UInt16 value)
        {
            var buf = BitConverter.GetBytes(value);
            var buf2 = buf.Reverse().ToArray();

            var v1 = buf.ToUInt16(0, true);
            var v2 = buf2.ToUInt16(0, false);

            Assert.Equal(value, v1);
            Assert.Equal(v1, v2);
        }

        /// <summary>
        /// 异常测试
        /// </summary>
        [Fact]
        public void ArrayToUInt16_Exception()
        {
            _ = Assert.Throws<ArgumentNullException>(() => ((byte[])null).ToUInt16(0, true));
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => (new byte[] { 0x11, 0x22 }).ToUInt16(2, true));
            _ = Assert.Throws<ArgumentException>(() => (new byte[] { 0x11, 0x22 }).ToUInt16(1, true));
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlySpan<byte>(new byte[] { 0x11, 0x22 }, 0, 1).ToUInt16(true));
        }

        /// <summary>
        /// 起始索引测试
        /// </summary>
        [Fact]
        public void ArrayToInt16_StartIndex()
        {
            var lstBuf = BitConverter.GetBytes(Int16.MaxValue).ToList();
            lstBuf.Insert(0, 0);
            lstBuf.Add(0);
            var buf = lstBuf.ToArray();
            var value = buf.ToInt16(1, BitConverter.IsLittleEndian);

            Assert.Equal(Int16.MaxValue, value);
        }
        #endregion

        #region 字节数组转Int32测试
        /// <summary>
        /// 转换测试
        /// </summary>
        /// <param name="value"></param>
        [Theory]
        [InlineData(Int32.MinValue)]
        [InlineData(Int32.MaxValue)]
        [InlineData(27837)]
        [InlineData(-1)]
        [InlineData(-21443)]
        public void ArrayToInt32(Int32 value)
        {
            var buf = BitConverter.GetBytes(value);
            var buf2 = buf.Reverse().ToArray();

            var v1 = buf.ToInt32(0, true);
            var v2 = buf2.ToInt32(0, false);

            Assert.Equal(value, v1);
            Assert.Equal(v1, v2);
        }

        /// <summary>
        /// 异常测试
        /// </summary>
        [Fact]
        public void ArrayToInt32_Exception()
        {
            _ = Assert.Throws<ArgumentNullException>(() => ((byte[])null).ToInt32(0, true));
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => (new byte[] { 0x11, 0x22 }).ToInt32(2, true));
            _ = Assert.Throws<ArgumentException>(() => (new byte[] { 0x11, 0x22 }).ToInt32(1, true));
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlySpan<byte>(new byte[] { 0x11, 0x22 }, 0, 1).ToInt32(true));
        }

        /// <summary>
        /// 转换测试
        /// </summary>
        /// <param name="value"></param>
        [Theory]
        [InlineData(UInt32.MinValue)]
        [InlineData(UInt32.MaxValue)]
        [InlineData(27837)]
        public void ArrayToUInt32(UInt32 value)
        {
            var buf = BitConverter.GetBytes(value);
            var buf2 = buf.Reverse().ToArray();

            var v1 = buf.ToUInt32(0, true);
            var v2 = buf2.ToUInt32(0, false);

            Assert.Equal(value, v1);
            Assert.Equal(v1, v2);
        }

        /// <summary>
        /// 异常测试
        /// </summary>
        [Fact]
        public void ArrayToUInt32_Exception()
        {
            _ = Assert.Throws<ArgumentNullException>(() => ((byte[])null).ToUInt32(0, true));
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => (new byte[] { 0x11, 0x22 }).ToUInt32(2, true));
            _ = Assert.Throws<ArgumentException>(() => (new byte[] { 0x11, 0x22 }).ToUInt32(1, true));
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlySpan<byte>(new byte[] { 0x11, 0x22 }, 0, 1).ToUInt32(true));
        }

        /// <summary>
        /// 起始索引测试
        /// </summary>
        [Fact]
        public void ArrayToInt32_StartIndex()
        {
            var lstBuf = BitConverter.GetBytes(0x11223344).ToList();
            lstBuf.Insert(0, 0);
            lstBuf.Add(0);
            var buf = lstBuf.ToArray();
            var value = buf.ToInt32(1, BitConverter.IsLittleEndian);

            Assert.Equal(0x11223344, value);
        }
        #endregion

        #region 字节数组转Int64测试
        /// <summary>
        /// 转换测试
        /// </summary>
        /// <param name="value"></param>
        [Theory]
        [InlineData(Int64.MinValue)]
        [InlineData(Int64.MaxValue)]
        [InlineData(27837)]
        [InlineData(-1)]
        [InlineData(-21443)]
        public void ArrayToInt64(Int64 value)
        {
            var buf = BitConverter.GetBytes(value);
            var buf2 = buf.Reverse().ToArray();

            var v1 = buf.ToInt64(0, true);
            var v2 = buf2.ToInt64(0, false);

            Assert.Equal(value, v1);
            Assert.Equal(v1, v2);
        }

        /// <summary>
        /// 异常测试
        /// </summary>
        [Fact]
        public void ArrayToInt64_Exception()
        {
            _ = Assert.Throws<ArgumentNullException>(() => ((byte[])null).ToInt64(0, true));
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => (new byte[] { 0x11, 0x22 }).ToInt64(2, true));
            _ = Assert.Throws<ArgumentException>(() => (new byte[] { 0x11, 0x22 }).ToInt64(1, true));
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlySpan<byte>(new byte[] { 0x11, 0x22 }, 0, 1).ToInt64(true));
        }

        /// <summary>
        /// 转换测试
        /// </summary>
        /// <param name="value"></param>
        [Theory]
        [InlineData(UInt64.MinValue)]
        [InlineData(UInt64.MaxValue)]
        [InlineData(27837)]
        public void ArrayToUInt64(UInt64 value)
        {
            var buf = BitConverter.GetBytes(value);
            var buf2 = buf.Reverse().ToArray();

            var v1 = buf.ToUInt64(0, true);
            var v2 = buf2.ToUInt64(0, false);

            Assert.Equal(value, v1);
            Assert.Equal(v1, v2);
        }

        /// <summary>
        /// 异常测试
        /// </summary>
        [Fact]
        public void ArrayToUInt64_Exception()
        {
            _ = Assert.Throws<ArgumentNullException>(() => ((byte[])null).ToUInt64(0, true));
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => (new byte[] { 0x11, 0x22 }).ToUInt64(2, true));
            _ = Assert.Throws<ArgumentException>(() => (new byte[] { 0x11, 0x22 }).ToUInt64(1, true));
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlySpan<byte>(new byte[] { 0x11, 0x22 }, 0, 1).ToUInt64(true));
        }

        /// <summary>
        /// 起始索引测试
        /// </summary>
        [Fact]
        public void ArrayToInt64_StartIndex()
        {
            var lstBuf = BitConverter.GetBytes(0x1122334455667788).ToList();
            lstBuf.Insert(0, 0);
            lstBuf.Add(0);
            var buf = lstBuf.ToArray();
            var value = buf.ToInt64(1, BitConverter.IsLittleEndian);

            Assert.Equal(0x1122334455667788, value);
        }
        #endregion
    }
}
