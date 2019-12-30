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
        /// <param name="isNullable"></param>
        /// <param name="nullText"></param>
        /// <param name="falseText"></param>
        /// <param name="trueText"></param>
        /// <returns></returns>
        public static List<MySelectListItem> GetBooleanList(bool isNullable, string nullText = "Не выбрано", string falseText = "Нет", string trueText = "Да")
        {
            var list = new List<MySelectListItem>();

            if(isNullable)
            {
                list.Add(new MySelectListItem
                {
                    Selected = true,
                    Text = nullText,
                    Value = null
                });
            }

            list.AddRange(new List<MySelectListItem>
            {
                new MySelectListItem
                {
                    Selected = !isNullable,
                    Text = falseText,
                    Value = false.ToString(),
                },

                new MySelectListItem
                {
                    Text = trueText,
                    Value = true.ToString()
                }
            });

            return list;
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
