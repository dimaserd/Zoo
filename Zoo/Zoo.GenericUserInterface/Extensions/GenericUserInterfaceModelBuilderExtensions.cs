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
        private static string AddPropNameToPrefix(string prefix, string propName)
        {
            return prefix == string.Empty ? propName : $"{prefix}.{propName}";
        }

        private static UserInterfaceBlock GetBlockForEnumerable(string prefix, CrocoPropertyReferenceDescription prop, CrocoTypeDescriptionResult desc, GenericInterfaceOptions opts)
        {
            var type = desc.GetTypeDescription(prop.DisplayFullTypeName);

            if(!type.IsEnumerable)
            {
                throw new InvalidOperationException("Type is not of type enumerable");
            }

            var enumeratedType = desc.GetTypeDescription(type.EnumeratedDiplayFullTypeName);

            if(enumeratedType.IsPrimitive)
            {
                return new UserInterfaceBlock
                {
                    LabelText = prop.PropertyDescription.PropertyDisplayName,
                    InterfaceType = UserInterfaceType.MultipleDropDownList,
                    PropertyName = prop.PropertyDescription.PropertyName,
                    SelectList = GetSelectList(prop, desc, opts)
                };
            }

            var newPrefix = AddPropNameToPrefix(prefix, prop.PropertyDescription.PropertyName);

            return new UserInterfaceBlock
            {
                LabelText = prop.PropertyDescription.PropertyDisplayName,
                InterfaceType = UserInterfaceType.GenericInterfaceForArray,
                PropertyName = prop.PropertyDescription.PropertyName,
                InnerGenericInterface = new GenericInterfaceModel
                {
                    Prefix = newPrefix,
                    Blocks = GetBlocks(newPrefix, enumeratedType, desc, opts)
                }
            };
        }

        internal static List<UserInterfaceBlock> GetBlocks(string prefix, CrocoTypeDescription currentType, CrocoTypeDescriptionResult desc, GenericInterfaceOptions opts)
        {
            List<UserInterfaceBlock> result = new List<UserInterfaceBlock>();

            foreach (var prop in currentType.Properties)
            {
                result.Add(GetBlockFromProperty(prefix, prop, desc, opts));
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

        private static UserInterfaceBlock GetBlockFromProperty(string prefix, CrocoPropertyReferenceDescription prop, CrocoTypeDescriptionResult main, GenericInterfaceOptions opts)
        {
            var propTypeDescription = main.GetTypeDescription(prop.DisplayFullTypeName);

            if (!propTypeDescription.IsClass && !propTypeDescription.IsEnumerable)
            {
                var type = GetUserInterfaceType(propTypeDescription);

                return new UserInterfaceBlock
                {
                    LabelText = prop.PropertyDescription.PropertyDisplayName,
                    PropertyName = prop.PropertyDescription.PropertyName,
                    InterfaceType = type,
                    SelectList = GetSelectList(prop, main, opts),
                    TextBoxData = GetTextBoxDataForProperty(propTypeDescription, type)
                };
            }

            if (propTypeDescription.IsEnumerable)
            {
                return GetBlockForEnumerable(prefix, prop, main, opts);
            }

            if (propTypeDescription.IsClass)
            {
                var propDesc = main.GetTypeDescription(propTypeDescription.TypeDisplayFullName);

                var newPrefix = AddPropNameToPrefix(prefix, prop.PropertyDescription.PropertyName);

                return new UserInterfaceBlock
                {
                    InterfaceType = UserInterfaceType.GenericInterfaceForClass,
                    LabelText = prop.PropertyDescription.PropertyDisplayName,
                    PropertyName = prop.PropertyDescription.PropertyName,
                    InnerGenericInterface = new GenericInterfaceModel
                    {
                        Prefix = newPrefix,
                        Blocks = GetBlocks(newPrefix, propDesc, main, opts)
                    }
                };
            }

            throw new Exception("Not supported");
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