using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Models.Definition;
using Zoo.GenericUserInterface.Utils;

namespace Zoo.GenericUserInterface.Extensions
{
    internal static class GenericUserInterfaceModelBuilderMappings
    {
        internal static List<SelectListItem> ToSelectListItems<TProp>(this List<SelectListItemData<TProp>> selectListItems)
        {
            return selectListItems.Select(ToSelectListItem).ToList();
        }

        internal static SelectListItem ToSelectListItem<TProp>(this SelectListItemData<TProp> data)
        {
            return new SelectListItem
            {
                DataJson = data.DataJson,
                Text = data.Text,
                Selected = data.Selected,
                Value = ExtractStringValue(data.Value)
            };
        }

        private static string ExtractStringValue<TProp>(TProp value)
        {
            if(value == null)
            {
                return null;
            }

            return typeof(TProp) == typeof(string)
                ? value.ToString()
                : Tool.JsonConverter.Serialize(value);
        }
    }
}