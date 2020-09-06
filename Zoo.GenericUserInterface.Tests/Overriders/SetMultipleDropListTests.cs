using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Models.Overridings;
using Zoo.GenericUserInterface.Resources;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Tests.Overriders
{
    public class SetMultipleDropListTests
    {
        internal class SomeType
        {
            public string Prop1 { get; set; }
            public int AnotherProp1 { get; set; }
            public List<SomeEnum> Prop2 { get; set; }
            public List<SomeType2> Prop3 { get; set; }
        }

        internal class SomeType2
        {
            public string Prop1 { get; set; }
        }

        [Test]
        public void NotOnPrimitivesShouldThrowException()
        {
            NotOnPrimitivesShouldThrowExceptionGeneric(x => x.Prop2);
            NotOnPrimitivesShouldThrowExceptionGeneric(x => x.Prop3);
        }

        private void NotOnPrimitivesShouldThrowExceptionGeneric<TProp>(Expression<Func<SomeType, TProp>> expression)
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                new GenericUserInterfaceModelBuilder<SomeType>().GetBlockBuilder(expression)
                    .SetMultipleDropDownList(new List<SelectListItemData<TProp>>());
            });
            var propName = MyExpressionExtensions.GetMemberName(expression);
            var expectedMessage = string.Format(ExceptionTexts.CantSetMultipleDropListNotOnPrimitivesFormat, propName, typeof(TProp).FullName);
        }


        [Test]
        public void TestImplementingMultipleDropDownForToNotEnumerableProperty()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeType>();

            var ex = Assert.Throws<InvalidOperationException>(() => builder.GetBlockBuilder(x => x.Prop1).SetMultipleDropDownList(new List<SelectListItemData<string>>()));

            Assert.AreEqual(ex.Message, ExceptionTexts.CantImplementMultipleDropDownForToNotEnumerableProperty);
        }
    }
}