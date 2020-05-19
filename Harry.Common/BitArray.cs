/*
 来源于<NET CLR via c# 4> 第10章 10.2有参数性
 */
using System;

namespace Harry
{
    public sealed class BitArray
    {
        //容纳了二进制位的私有字节数组
        private byte[] m_byteArray;
        private int m_numBits;

        //下面的构造器用于分配字节数组,并将所有位设为0
        public BitArray(int numBits)
        {
            //先验证实参
            if (numBits <= 0)
                throw new ArgumentOutOfRangeException(nameof(numBits));

            //保存位的个数
            m_numBits = numBits;
            //为位数组分配字节
            m_byteArray = new byte[(numBits + 7) / 8];
        }

        //下面是索引器(有参属性)
        public bool this[int bitPos]
        {
            //下面是索引器的get访问器方法
            get
            {
                //先验证实参
                if ((bitPos < 0) || (bitPos >= m_numBits))
                    throw new ArgumentOutOfRangeException(nameof(bitPos));

                //返回指定索引处的位的状态(如果要与1比较,位还得右移回去)
                return (m_byteArray[bitPos / 8] & (1 << (bitPos % 8))) != 0;
            }

            //下面是索引器的set访问器方法
            set
            {
                if ((bitPos < 0) || (bitPos >= m_numBits))
                    throw new ArgumentOutOfRangeException(nameof(bitPos), bitPos.ToString());

                if (value)
                {
                    //将指定索引处的位设为true
                    m_byteArray[bitPos / 8] = (byte)(m_byteArray[bitPos / 8] | (1 << (bitPos % 8)));
                }
                else
                {
                    //将指定索引处的位设为false
                    m_byteArray[bitPos / 8] = (byte)(m_byteArray[bitPos / 8] & ~(1 << (bitPos % 8)));
                }
            }
        }
    }
}
