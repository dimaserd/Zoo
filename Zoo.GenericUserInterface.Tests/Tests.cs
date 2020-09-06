using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Extensions;
using Zoo.GenericUserInterface.Services;
using Zoo.GenericUserInterface.Models.Overridings;

namespace Tests
{
    public enum SomeEnumType
    {
        [Display(Name = "Тип1")]
        Type1,


        [Display(Name = "Тип2")]
        Type2,
    }

    public class SomeClass
    {
        public int Prop1 { get; set; }
        public SomeEnumType EnumProp { get; set; }

        public List<string> Property2 { get; set; }

        public List<int> Property3 { get; set; }

        public List<SomeEnumType> Property { get; set; }

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
    }
}