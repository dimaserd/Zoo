using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zoo.GenericUserInterface.Services;

namespace Tests
{
    public enum SomeType
    {
        [Display(Name = "“ип1")]
        Type1,


        [Display(Name = "“ип2")]
        Type2,
    }

    public class SomeClass
    {
        public List<string> Property2 { get; set; }

        public List<int> Property3 { get; set; }

        public List<SomeType> Property { get; set; }
    }

    public class Tests
    {
        [Test]
        public void Test1()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeClass>("prefix");

            Assert.IsTrue(true);
        }
    }
}