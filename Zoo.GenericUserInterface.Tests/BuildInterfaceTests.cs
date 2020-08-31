using NUnit.Framework;
using System;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Resources;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests
{
    public class BuildInterfaceTests
    {
        [TestCase(typeof(string))]
        [TestCase(typeof(int))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(bool))]
        public void CheckInterfaceGenerationOnPrimitives(Type type)
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                var res = new GenericUserInterfaceModelBuilder(type).Result;
            });

            Assert.AreEqual(ExceptionTexts.NonComplexTypesAreNotSupported, ex.Message);
        }

        public class SimpleRecursiveType
        {
            public string Prop1 { get; set; }
            public SimpleRecursiveType Prop2 { get; set; }
        }

        public class RecursiveType1
        {
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
            public RecursiveType1SubType Prop3 { get; set; }
        }

        public class RecursiveType1SubType
        {
            public int Prop2 { get; set; }
            public RecursiveType1 Prop1 { get; set; }
        }

        public class RecursiveType2
        {
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
            public RecursiveType2SubType1 Prop3 { get; set; }
        }

        public class RecursiveType2SubType1
        {
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
            public RecursiveType2SubType1SubType Prop3 { get; set; }
        }

        public class RecursiveType2SubType1SubType
        {
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
            public RecursiveType2 Prop3 { get; set; }
        }

        [TestCase(typeof(SimpleRecursiveType))]
        [TestCase(typeof(RecursiveType1))]
        [TestCase(typeof(RecursiveType2))]
        [TestCase(typeof(GenerateGenericUserInterfaceModel))]
        public void CheckInterfaceGenerationOnRecursiveTypes(Type type)
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                var res = new GenericUserInterfaceModelBuilder(type).Result;
            });

            Assert.AreEqual(ExceptionTexts.RecursiveTypesAreNotSupported, ex.Message);
        }
    }
}
