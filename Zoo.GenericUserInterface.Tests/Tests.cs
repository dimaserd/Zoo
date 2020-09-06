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
        public void TestListOfStringPoperty()
        {
            var result = new GenericUserInterfaceModelBuilder<SomeClass>().Result;

            var block = result.Interface.Blocks.First(t => t.PropertyName == nameof(SomeClass.Property2));

            Assert.AreEqual(nameof(SomeClass.Property2), block.LabelText);
            Assert.AreEqual(UserInterfaceType.MultipleDropDownList, block.InterfaceType);
            Assert.IsTrue(block.DropDownData.SelectList.Count == 0);
        }

        [Test]
        public void ShiftToStartForTest()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeClass>();

            builder.ShiftToStartFor(x => x.Property);

            var result = builder.Result;

            Assert.IsTrue(result.Interface.Blocks.First().PropertyName == nameof(SomeClass.Property));
        }

        [Test]
        public void ShiftToEndForTest()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeClass>();

            builder.ShiftToEndFor(x => x.Property2);

            Assert.IsTrue(builder.Result.Interface.Blocks.Last().PropertyName == nameof(SomeClass.Property2));
        }

        [Test]
        public void Test()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeClass>();

            var list = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Selected = false,
                    Text = "Text",

                }
            };

            builder.DropDownListFor(x => x.Sex, list);

            var lastProp = builder.Result.Interface.Blocks.Last();

            Assert.IsTrue(lastProp.InterfaceType == UserInterfaceType.DropDownList);
            Assert.IsTrue(lastProp.DropDownData.SelectList == list);
        }

        [Test]
        public void TestImplementingDropDownForToEnumProperty()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeClass>();

            var ex = Assert.Throws<ApplicationException>(() => builder.DropDownListFor(x => x.EnumProp, new List<SelectListItem>
            {
                new SelectListItem
                {
                    Selected = false,
                    Text = "Text"
                }
            }));

            var expectedMessage = string.Format(ExceptionTexts.CantImplementMethodNameToEnumPropertyFormat, "SetDropDownList");
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void TestImplementingMultipleDropDownForToNotEnumerableProperty()
        {
            var builder = new GenericUserInterfaceModelBuilder<SomeClass>();

            var ex = Assert.Throws<ApplicationException>(() => builder.MultipleDropDownListFor(x => x.EnumProp, new List<SelectListItem>
            {
                new SelectListItem
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
            var builder = new GenericUserInterfaceModelBuilder<SomeClass>();

            var ex = Assert.Throws<ApplicationException>(() => builder.MultipleDropDownListFor(x => x.Property, new List<SelectListItem>
            {
                new SelectListItem
                {
                    Selected = false,
                    Text = "Text"
                }
            }));

            Assert.AreEqual(ex.Message, ExceptionTexts.CantImplementMultipleDropDownForToEnumerableOfEnumerationProperty);
        }
    }
}