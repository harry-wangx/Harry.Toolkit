using System;

namespace Harry.Json.Test
{
    public class Person
    {
        public const string JSON_STRING = "{\"Name\":\"Harry\",\"Age\":20,\"CreatedOn\":\"2016-01-01 00:00:00\"}";

        public string Name { get; set; } = "Harry";

        public int Age { get; set; } = 20;

        public DateTime CreatedOn { get; set; } = new DateTime(2016, 1, 1);
    }
}
