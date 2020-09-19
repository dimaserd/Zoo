using Croco.Core.Documentation.Models;
using NUnit.Framework;
using System;
using Zoo.ServerJs.Models;

namespace Zoo.ServerJs.Tests
{
    public class TypeDescriptionTests
    {
        [Test]
        public void Test()
        {
            var result = CrocoTypeDescription.GetDescription(typeof(Exception));
        }
    }
}
