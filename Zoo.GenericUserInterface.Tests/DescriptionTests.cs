using Croco.Core.Documentation.Models;
using Croco.Core.Documentation.Services;
using NUnit.Framework;
using System;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Tests
{
    public class DescriptionTests
    {
        [TestCase(typeof(GenerateGenericUserInterfaceModel), true)]
        public void TestTypes(Type type, bool isRecursiveExpected)
        {
            var doc = CrocoTypeDescription.GetDescription(type);

            var main = doc.GetMainTypeDescription();

            Assert.IsNotNull(doc);
            Assert.AreEqual(type, main.ExtractType());

            var isRecursive = new CrocoClassDescriptionChecker().IsRecursiveType(doc);

            Assert.AreEqual(isRecursiveExpected, isRecursive);
        }
    }
}