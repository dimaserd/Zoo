using Croco.Core.Documentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Extensions
{
    internal static class GenericUserInterfaceModelBuilderExtensions
    {
        private static UserInterfaceBlock GetBlockForEnumerable(CrocoPropertyReferenceDescription prop, CrocoTypeDescriptionResult desc, GenericInterfaceOptions opts)
        {
            return new UserInterfaceBlock
            {
                LabelText = prop.PropertyDescription?.PropertyDisplayName,
                InterfaceType = UserInterfaceType.MultipleDropDownList,
                PropertyName = prop.PropertyDescription.PropertyName,
                SelectList = GetSelectList(prop, desc, opts),
            };
        }

        internal static List<UserInterfaceBlock> GetBlocks(string prefix, CrocoTypeDescription currentType, CrocoTypeDescriptionResult desc, GenericInterfaceOptions opts)
        {
            List<UserInterfaceBlock> result = new List<UserInterfaceBlock>();

            foreach (var prop in currentType.Properties)
            {
                result.AddRange(GetBlocksFromProperty(prefix, prop, desc, opts));
            }

            return result;
        }

        private static UserInterfaceTextBoxData GetTextBoxDataForProperty(CrocoTypeDescription prop, UserInterfaceType interfaceType)
        {
            if (interfaceType != UserInterfaceType.TextBox)
            {
                return null;
            }

            var integerTypeNames = new[]
            {
                typeof(int).FullName,
                typeof(uint).FullName,
                typeof(long).FullName,
                typeof(ulong).FullName,
                typeof(ushort).FullName,
                typeof(short).FullName,
                typeof(byte).FullName
            };

            var isInteger = integerTypeNames.Contains(prop.FullTypeName);

            return new UserInterfaceTextBoxData
            {
                IsInteger = isInteger,
                IntStep = 1,
            };
        }

        private static List<UserInterfaceBlock> GetBlocksFromProperty(string prefix, CrocoPropertyReferenceDescription prop, CrocoTypeDescriptionResult main, GenericInterfaceOptions opts)
        {
            var propTypeDescription = main.GetTypeDescription(prop.DisplayFullTypeName);

            if (!propTypeDescription.IsClass && !propTypeDescription.IsEnumerable)
            {
                var type = GetUserInterfaceType(propTypeDescription);

                return new List<UserInterfaceBlock>
                {
                    new UserInterfaceBlock
                    {
                        LabelText = prop.PropertyDescription?.PropertyDisplayName,
                        PropertyName = $"{prefix}{prop.PropertyDescription.PropertyName}",
                        InterfaceType = type,
                        SelectList = GetSelectList(prop, main, opts),
                        TextBoxData = GetTextBoxDataForProperty(propTypeDescription, type)
                    }
                };
            }

            if (propTypeDescription.IsEnumerable)
            {
                return new List<UserInterfaceBlock>()
                {
                    GetBlockForEnumerable(prop, main, opts)
                };
            }

            if (propTypeDescription.IsClass)
            {
                var propDesc = main.GetTypeDescription(propTypeDescription.TypeDisplayFullName);

                return GetBlocks($"{prefix}{prop.PropertyDescription.PropertyName}.", propDesc, main, opts);
            }

            throw new Exception("не продумано");
        }

        private static List<SelectListItem> GetSelectList(CrocoPropertyReferenceDescription propDescr, CrocoTypeDescriptionResult main, GenericInterfaceOptions opts)
        {
            var emptySelectListPredicates = new List<Func<CrocoTypeDescription, bool>>
            {
                x => x.IsEnumerable,
                x => x.FullTypeName == typeof(DateTime).FullName
            };

            var propTypeDesc = main.GetTypeDescription(propDescr.DisplayFullTypeName);

            if (propTypeDesc.IsEnumerable)
            {
                var enumeratedType = main.GetTypeDescription(propTypeDesc.EnumeratedDiplayFullTypeName);

                if (enumeratedType.IsEnumeration)
                {
                    return enumeratedType.EnumDescription.EnumValues.Select(x => new SelectListItem
                    {
                        Text = x.DisplayName,
                        Value = x.StringRepresentation
                    }).ToList();
                }
            }

            if (propTypeDesc.FullTypeName == typeof(bool).FullName)
            {
                return MySelectListItemExtensions.GetBooleanList(propTypeDesc.IsNullable, opts);
            }

            if (emptySelectListPredicates.Any(x => x(propTypeDesc)))
            {
                return new List<SelectListItem>();
            }

            if (propTypeDesc.IsEnumeration)
            {
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

                return enumsList;
            }

            return null;
        }

        internal static UserInterfaceType GetUserInterfaceType(CrocoTypeDescription desc)
        {
            if (desc.FullTypeName == typeof(bool).FullName)
            {
                return UserInterfaceType.DropDownList;
            }

            if (desc.FullTypeName == typeof(DateTime).FullName)
            {
                return UserInterfaceType.DatePicker;
            }
            if (desc.IsEnumeration)
            {
                return UserInterfaceType.DropDownList;
            }

            return UserInterfaceType.TextBox;
        }
    }
}