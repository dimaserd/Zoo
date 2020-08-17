using Croco.Core.Documentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Extensions
{
    /// <summary>
    /// Расширения для построения списков
    /// </summary>
    public static class MySelectListItemExtensions
    {
        /// <summary>
        /// Получить список из булевых значений
        /// </summary>
        /// <param name="isNullable"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetBooleanList(bool isNullable, GenericInterfaceOptions opts)
        {
            var list = new List<SelectListItem>();

            if(isNullable)
            {
                list.Add(new SelectListItem
                {
                    Selected = true,
                    Text =  opts.NotSelectedText,
                    Value = null
                });
            }

            list.AddRange(new List<SelectListItem>
            {
                new SelectListItem
                {
                    Selected = !isNullable,
                    Text = opts.TextOnFalse,
                    Value = false.ToString(),
                },

                new SelectListItem
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
        public static List<SelectListItem> GetEnumDropDownList(Type enumType)
        {
            var crocoType = CrocoTypeDescription.GetDescription(enumType);

            var main = crocoType.GetMainTypeDescription();

            if(!main.IsEnumeration)
            {
                throw new Exception("Данный тип не является перечислением");
            }

            return main.EnumDescription.EnumValues.Select(x => new SelectListItem
            {
                Text = x.DisplayName,
                Value = x.StringRepresentation
            }).ToList();
        }
    }
}