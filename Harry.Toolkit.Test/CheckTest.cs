using System;
using Xunit;
using Xunit.Abstractions;

namespace Harry
{
    public class CheckTest
    {
        ITestOutputHelper outputHelper;

        public CheckTest(ITestOutputHelper output)
        {
            this.outputHelper = output;
        }

        [Fact]
        public void NotNull()
        {
            string strA = null;
            string strB = "b";

            Check.NotNull(strB, "strB");

            var ex = Assert.Throws<ArgumentNullException>(() => Check.NotNull(strA, nameof(strA)));
            Assert.Equal(nameof(strA), ex.ParamName);
        }

        [Fact]
        public void NotNullWithStruct()
        {
            Nullable<int> a = null;
            Nullable<int> b = 1;

            Check.NotNull(b, "b");

            var ex = Assert.Throws<ArgumentNullException>("a", () => Check.NotNull(a, "a"));
            Assert.Equal(nameof(a), ex.ParamName);
        }

        [Fact]
        public void NotNullOrEmpty()
        {
            string a = "a";
            string b = "";
            string c = null;

            Check.NotNullOrEmpty(a, "a");

            Assert.Throws<ArgumentNullException>("b", () => Check.NotNullOrEmpty(b, "b"));

            Assert.Throws<ArgumentNullException>("c", () => Check.NotNullOrEmpty(c, "c"));
        }
    }
}
