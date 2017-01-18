
using System;
using System.Collections.Generic;
#if NUNIT
using NUnit.Framework;
#else
using Xunit;
#endif


namespace Harry.Test.Configuration
{
    public class ConfigurationEntryTest
    {
        [Fact]
        public void GetRealPath()
        {
            //arrange
            var fileName = "~/app_data/config.xml";
            var contentPath = @"c:\www";
            var fullFileName = @"c:\www\app_data\config.xml";

            //act
            string result = MyConfiguration.GetRealPath(fileName, contentPath);

            //assert
            Assert.Equal(result, fullFileName);
        }
    }
}
