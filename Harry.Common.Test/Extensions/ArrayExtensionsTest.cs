﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Harry.Extensions;
using System.Linq;

namespace Harry.Common.Extensions
{
    public class ArrayExtensionsTest
    {
        ITestOutputHelper outputHelper;

        public ArrayExtensionsTest(ITestOutputHelper output)
        {
            this.outputHelper = output;
        }

        [Fact]
        public void EqualsTest()
        {
            byte[] a = new byte[] { 1, 2, 3 };
            byte[] b = new byte[] { 1, 2, 3 };
            byte[] c = new byte[] { 1, 2, 3, 4 };
            byte[] d = null;

            Assert.True(a.Equals<byte>(b));
            Assert.False(a.Equals<byte>(c));
            Assert.False(a.Equals<byte>(d));
            Assert.True(d.Equals<byte>(null));

            //测试带元素比较器的
            Assert.False((new string[] { "a", "B" }).Equals<string>(new string[] { "A", "B" }));
            Assert.True((new string[] { "a", "B" }).Equals<string>(new string[] { "A", "B" }, StringComparer.OrdinalIgnoreCase));
        }

        [Fact]
        public void EqualsRangeTest()
        {
            byte[] a = new byte[] { 1, 2, 3, 4, 5, 6 };
            byte[] b = new byte[] { 1, 2 };
            byte[] c = new byte[] { 5, 6 };
            byte[] d = new byte[] { 5, 7 };

            Assert.True(b.Equals<byte>(a, 0, 1));
            Assert.True(c.Equals<byte>(a, 4, 5));

            Assert.False(c.Equals<byte>(a, 5, 5));//数量不对
            Assert.False(d.Equals<byte>(a, 4, 5));//元素不对

            //测试带元素比较器的
            Assert.False((new string[] { "a" }).Equals(new string[] { "A", "B" }, 0, 0));
            Assert.True((new string[] { "a" }).Equals(new string[] { "A", "B" }, 0, 0, StringComparer.OrdinalIgnoreCase));

        }

        [Fact]
        public void StartsFor()
        {
            byte[] a = new byte[] { 1, 2, 3, 4, 5, 6 };
            byte[] b = new byte[] { 1, 2 };
            byte[] c = new byte[] { 5, 6 };

            Assert.True(b.StartsFor(a));
            Assert.False(c.StartsFor(a));
        }

        [Fact]
        public void EndsFor()
        {
            byte[] a = new byte[] { 1, 2, 3, 4, 5, 6 };
            byte[] b = new byte[] { 1, 2 };
            byte[] c = new byte[] { 5, 6 };

            Assert.False(b.EndsFor(a));
            Assert.True(c.EndsFor(a));
        }

        [Fact]
        public void Distinct()
        {
            byte[] a = new byte[] { 1, 2, 3 };
            byte[] b = new byte[] { 1, 2, 3 };

            List<byte[]> data = new List<byte[]>();
            data.Add(a);
            data.Add(a);
            data.Add(b);

            var data2 = data.Distinct(ArrayComparer<byte>.Default).ToList();

            Assert.Single(data2);
            Assert.True((new byte[] { 1, 2, 3 }).Equals<byte>(data2.Single()));

        }
    }
}
