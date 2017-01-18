using System;
using System.Collections.Generic;

namespace Harry.Test.Configuration
{
    public class MyConfiguration: Harry.Configuration.ConfigurationEntry<MyConfiguration>
    {
        public int Age { get; set; }

        public string Name { get; set; }
    }
}
