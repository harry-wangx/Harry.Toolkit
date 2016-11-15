using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Harry.Test.NET45.Common
{
    public class UtilsTest
    {
        [Fact]
        public void TestDOTNET_VERSION()
        {
            //arrange
            
            //act
            string version = Harry.Common.Utils.DOTNET_VERSION;

            //assert
            Assert.Equal(version, Harry.Common.Utils.NET45);
        }
    }
}
