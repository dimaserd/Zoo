using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        public List<string> Property2 { get; set; }

        public List<int> Property3 { get; set; }

        public List<SomeType> Property { get; set; }
    }

    public class EditApplicationUser
    {
        public string Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Дата рождения")]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        [Display(Name = "Пол")]
        public bool? Sex { get; set; }

        public string ObjectJson { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
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
            var builder = new GenericUserInterfaceModelBuilder<EditApplicationUser>("prefix");

            builder.DropDownListFor(x => x.Sex, new List<Zoo.GenericUserInterface.Models.MySelectListItem>
            {
                new Zoo.GenericUserInterface.Models.MySelectListItem
                {
                    Selected = false,
                    Text = "Text",

                }
            });

            Assert.IsTrue(true);
        }
    }
}