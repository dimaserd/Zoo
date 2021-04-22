using Croco.Core.Documentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Options;

namespace Zoo.GenericUserInterface.Extensions
{
    internal static class ForEnumDropDownDataBuilder
    {
        public static DropDownListData GetDropDownListData(CrocoTypeDescription propTypeDesc, GenericInterfaceOptions opts)
        {
            if (!propTypeDesc.IsEnumeration)
            {
                throw new InvalidOperationException($"{propTypeDesc.FullTypeName} is not  enum");
            }

            var enumsList = new List<SelectListItem>();

            if (propTypeDesc.IsNullable)
            {
                enumsList.Add(new SelectListItem
                {
                    Value = null,
                    Selected = true,
                    Text = opts.NotSelectedText
                });
            }

            enumsList.AddRange(propTypeDesc.EnumDescription.EnumValues.Select(x => new SelectListItem
            {
                Value = x.StringRepresentation,
                Text = x.DisplayName
            }));

            if (!enumsList.Any(x => x.Selected))
            {
                var first = enumsList.FirstOrDefault();

                if (first != null)
                {
                    first.Selected = true;
                }
            }

            return new DropDownListData
            {
                CanAddNewItem = false,
                SelectList = enumsList
            };
        }
    }
}