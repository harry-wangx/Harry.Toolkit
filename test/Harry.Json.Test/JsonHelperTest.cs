using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
namespace Harry.Json.Test
{
    public class JsonHelperTest
    {
        [Fact]
        public void Test_SerializeObject()
        {
            //arrange
            var person = new Person();

            //act
            var jsonString = JsonHelper.SerializeObject(person);

            //assert
            Assert.Equal(Person.JSON_STRING, jsonString);
        }

        [Fact]
        public void Test_DeserializeObject()
        {
            //arrange
            var person = new Person();

            //act
            dynamic obj = JsonHelper.DeserializeObject(Person.JSON_STRING);

            //assert
            Assert.Equal((string)obj.Name, person.Name);
            Assert.Equal((int)obj.Age, person.Age);
            Assert.Equal((DateTime)obj.CreatedOn,person.CreatedOn);
        }

        [Fact]
        public void Test_DeserializeObject_With_Generic()
        {
            //arrange
            var person = new Person();

            //act
            var jsonPerson = JsonHelper.DeserializeObject<Person>(Person.JSON_STRING);

            //assert
            Assert.Equal(jsonPerson.Name, person.Name);
            Assert.Equal(jsonPerson.Age, person.Age);
            Assert.Equal(jsonPerson.CreatedOn, person.CreatedOn);
        }
    }
}
