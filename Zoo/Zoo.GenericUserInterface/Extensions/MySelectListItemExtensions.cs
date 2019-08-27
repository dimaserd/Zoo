using Croco.Core.Logic.Models.Documentation;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Extensions
{
    public static class MySelectListItemExtensions
    {
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

        public static List<MySelectListItem> GetEnumDropDownList(Type enumType)
        {
            var crocoType = CrocoTypeDescription.GetDescription(enumType);

            if(!crocoType.IsEnumeration)
            {
                throw new Exception("Данный тип не является перечислением");
            }

            return crocoType.EnumValues.Select(x => new MySelectListItem
            {
                Text = x.DisplayName,
                Value = x.StringRepresentation
            }).ToList();
        }
    }
}
