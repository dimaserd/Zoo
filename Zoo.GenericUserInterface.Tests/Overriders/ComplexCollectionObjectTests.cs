using NUnit.Framework;
using System;
using System.Collections.Generic;
using Zoo.GenericUserInterface.Resources;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests.Overriders
{
    public class ComplexCollectionObjectTests
    {
        internal class SomeType
        {
            public List<int[]> ComplexCollectionType { get; set; }
        }


        [Test]
        public void CollectionOfComplexObjects_ShouldFail()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                new GenericUserInterfaceModelBuilder<SomeType>(TestsHelper.CreateDefaultBag());
            });

            var mes = string.Format(ExceptionTexts.ClassesWithMultiDimensionalArrayPropertiesAreNotSupported, nameof(SomeType.ComplexCollectionType), typeof(int[]).FullName);

            Assert.AreEqual(mes, ex.Message);
        }
    }
}