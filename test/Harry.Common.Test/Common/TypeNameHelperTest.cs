using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Harry.Common.Test
{
    public class TypeNameHelperTest
    {
        [Fact]
        public void Test_GetTypeDisplayName()
        {
            //arrange

            //act
            string name = Harry.Common.TypeNameHelper.GetTypeDisplayName(this.GetType());

            //assert
            Assert.Equal(name, "Harry.Common.Test.TypeNameHelperTest");

        }

        [Fact]
        public void Test_GetTypeDisplayName_With_Generic()
        {
            //arrange

            //act
            string name = Harry.Common.TypeNameHelper.GetTypeDisplayName(new List<int>().GetType());

            //assert
            Assert.Equal(name, "System.Collections.Generic.List");

        }
    }
}
