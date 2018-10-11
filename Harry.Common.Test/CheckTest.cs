using System;
using Xunit;
using Xunit.Abstractions;

namespace Harry.Common.Test
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

            try
            {
                Check.NotNull(strA, "strA");
            }
            catch (ArgumentNullException ex)
            {
                Assert.Same(ex.ParamName, "strA");
            }
        }

        [Fact]
        public void NotNullWithNullable()
        {
            Nullable<int> a = null;
            Nullable<int> b = 1;
            
            Check.NotNull(b, "b");

            Assert.Throws<ArgumentNullException>("a", () => {
                Check.NotNull(a, "a");
            });
        }

        [Fact]
        public void NotNullOrEmpty()
        {
            string a = "a";
            string b = "";
            string c = null;

            Check.NotNullOrEmpty(a, "a");


            Assert.Throws<ArgumentNullException>("b", () => {
                Check.NotNullOrEmpty(b, "b");
            });

            Assert.Throws<ArgumentNullException>("c", () => {
                Check.NotNullOrEmpty(c, "c");
            });
        }
    }
}
