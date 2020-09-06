using System.Collections.Generic;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Extensions
{
    /// <summary>
    /// Расширения для построения списков
    /// </summary>
    internal static class MySelectListItemExtensions
    {
        /// <summary>
        /// Получить список из булевых значений
        /// </summary>
        /// <param name="isNullable"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        internal static List<SelectListItem> GetBooleanList(bool isNullable, GenericInterfaceOptions opts)
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
    }
}