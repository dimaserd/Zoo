using Croco.Core.Documentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Extensions
{
    public static class MySelectListItemExtensions
    {
        /// <summary>
        /// Получить список из булевых значений
        /// </summary>
        /// <param name="falseText"></param>
        /// <param name="trueText"></param>
        /// <returns></returns>
        public static List<MySelectListItem> GetBooleanList(string falseText = "Нет", string trueText = "Да")
        {
            return new List<MySelectListItem>
            {
                new MySelectListItem
                {
                    Selected = true,
                    Text = falseText,
                    Value = false.ToString(),
                },

                new MySelectListItem
                {
                    Text = trueText,
                    Value = true.ToString()
                }
            };
        }

        /// <summary>
        /// Получить список из енамов
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<MySelectListItem> GetEnumDropDownList(Type enumType)
        {
            var crocoType = CrocoTypeDescription.GetDescription(enumType);

            if(!crocoType.IsEnumeration)
            {
                throw new Exception("Данный тип не является перечислением");
            }

            return crocoType.EnumDescription.EnumValues.Select(x => new MySelectListItem
            {
                Text = x.DisplayName,
                Value = x.StringRepresentation
            }).ToList();
        }
    }
}
