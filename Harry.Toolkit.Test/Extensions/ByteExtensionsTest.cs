using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harry.Extensions
{
    public class ByteExtensionsTest
    {
        /// <summary>
        /// 高低位转换测试
        /// </summary>
        [Fact]
        public void ReverseHighLow()
        {
            byte v1 = 0b10110001;
            byte v2 = 0b10001101;

            Assert.Equal(v1, v2.ReverseHighLow());
            Assert.Equal(v2, v1.ReverseHighLow());
        }
    }
}
