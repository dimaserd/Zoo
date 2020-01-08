using Croco.Core.Documentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Extensions
{
    internal static class MySelectListItemExtensions
    {
        /// <summary>
        /// Получить список из булевых значений
        /// </summary>
        /// <param name="isNullable"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        internal static List<MySelectListItem> GetBooleanList(bool isNullable, GenericInterfaceOptions opts)
        {
            var list = new List<MySelectListItem>();

            if(isNullable)
            {
                list.Add(new MySelectListItem
                {
                    Selected = true,
                    Text =  opts.NotSelectedText,
                    Value = null
                });
            }

            list.AddRange(new List<MySelectListItem>
            {
                new MySelectListItem
                {
                    Selected = !isNullable,
                    Text = opts.TextOnFalse,
                    Value = false.ToString(),
                },

                new MySelectListItem
                {
                    Text = opts.TextOnTrue,
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
        internal static List<MySelectListItem> GetEnumDropDownList(Type enumType)
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