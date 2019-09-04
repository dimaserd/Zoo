using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Resources;
using Zoo.GenericUserInterface.Services;

namespace Tests
{
    public enum SomeType
    {
        [Display(Name = "Тип1")]
        Type1,


        [Display(Name = "Тип2")]
        Type2,
    }

    public class SomeClass
    {
        public SomeType EnumProp { get; set; }

        public List<string> Property2 { get; set; }

        public List<int> Property3 { get; set; }

        public List<SomeType> Property { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        [Display(Name = "Пол")]
        public bool? Sex { get; set; }
    }

    public class Tests
    {
        [Test]
        public void ShiftToStartForTest()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeClass>("prefix");

            builder.ShiftToStartFor(x => x.Property);

            Assert.IsTrue(builder.Result.Blocks.First().PropertyName == nameof(SomeClass.Property));
        }

        [Test]
        public void ShiftToEndForTest()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeClass>("prefix");

            builder.ShiftToEndFor(x => x.Property2);

            Assert.IsTrue(builder.Result.Blocks.Last().PropertyName == nameof(SomeClass.Property2));
        }

        [Test]
        public void Test()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeClass>("prefix");

            var list = new List<MySelectListItem>
            {
                new MySelectListItem
                {
                    Selected = false,
                    Text = "Text",

                }
            };

            builder.DropDownListFor(x => x.Sex, list);

            var lastProp = builder.Result.Blocks.Last();

            Assert.IsTrue(lastProp.InterfaceType == UserInterfaceType.DropDownList);
            Assert.IsTrue(lastProp.SelectList == list);
        }

        [Test]
        public void TestImplementingDropDownForToEnumProperty()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeClass>("prefix");

            var ex = Assert.Throws<ApplicationException>(() => builder.DropDownListFor(x => x.EnumProp, new List<Zoo.GenericUserInterface.Models.MySelectListItem>
            {
                new MySelectListItem
                {
                    Selected = false,
                    Text = "Text"
                }
            }));

            Assert.AreEqual(ex.Message, string.Format(ExceptionTexts.CantImplementMethodNameToEnumPropertyFormat, nameof(builder.DropDownListFor)));
        }

        [Test]
        public void TestImplementingMultipleDropDownForToNotEnumerableProperty()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeClass>("prefix");

            var ex = Assert.Throws<ApplicationException>(() => builder.MultipleDropDownListFor(x => x.EnumProp, new List<MySelectListItem>
            {
                new MySelectListItem
                {
                    Selected = false,
                    Text = "Text"
                }
            }));

            Assert.AreEqual(ex.Message, string.Format(ExceptionTexts.CantImplementMultipleDropDownForToNotEnumerableProperty, nameof(builder.MultipleDropDownListFor)));
        }


        [Test]
        public void TestImplementingMultipleDropDownForToEnumProperty()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeClass>("prefix");

            var ex = Assert.Throws<ApplicationException>(() => builder.MultipleDropDownListFor(x => x.Property, new List<MySelectListItem>
            {
                new MySelectListItem
                {
                    Selected = false,
                    Text = "Text"
                }
            }));

            Assert.AreEqual(ex.Message, ExceptionTexts.CantImplementMultipleDropDownForToEnumerableOfEnumerationProperty);
        }
    }
}