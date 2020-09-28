using Croco.Core.Documentation.Models;
using Croco.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models;
using Zoo.GenericUserInterface.Models.Overridings;

namespace Zoo.GenericUserInterface.Extensions
{
    internal static class GenericUserInterfaceModelBuilderExtensions
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
                Value = Tool.JsonConverter.Serialize(data.Value)
            };
        }

        private static string AddPropNameToPrefix(string prefix, string propName)
        {
            return prefix == string.Empty ? propName : $"{prefix}.{propName}";
        }

        private static UserInterfaceBlock GetBlockForEnumerable(string prefix, CrocoPropertyReferenceDescription prop, CrocoTypeDescriptionResult desc, GenericInterfaceOptions opts)
        {
            var type = desc.GetTypeDescription(prop.TypeDisplayFullName);

            if(!type.ArrayDescription.IsArray)
            {
                throw new InvalidOperationException("Type is not of type enumerable");
            }

            var enumeratedType = desc.GetTypeDescription(type.ArrayDescription.ElementDiplayFullTypeName);

            if(enumeratedType.IsPrimitive)
            {
                return new UserInterfaceBlock
                {
                    LabelText = prop.PropertyDescription.PropertyDisplayName,
                    InterfaceType = UserInterfaceType.MultipleDropDownList,
                    PropertyName = prop.PropertyDescription.PropertyName,
                    DropDownData = GetSelectListData(prop, desc, opts)
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
            var propTypeDescription = main.GetTypeDescription(prop.TypeDisplayFullName);

            if (!propTypeDescription.IsClass && !propTypeDescription.ArrayDescription.IsArray)
            {
                var type = GetUserInterfaceType(propTypeDescription);

                var selectList = GetSelectListData(prop, main, opts);

                return new UserInterfaceBlock
                {
                    LabelText = prop.PropertyDescription.PropertyDisplayName,
                    PropertyName = prop.PropertyDescription.PropertyName,
                    InterfaceType = type,
                    DropDownData = selectList,
                    TextBoxData = GetTextBoxDataForProperty(propTypeDescription, type)
                };
            }

            if (propTypeDescription.ArrayDescription.IsArray)
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

        private static DropDownListData GetSelectListData(CrocoPropertyReferenceDescription propDescr, CrocoTypeDescriptionResult main, GenericInterfaceOptions opts)
        {
            var emptySelectListPredicates = new List<Func<CrocoTypeDescription, bool>>
            {
                x => x.ArrayDescription.IsArray,
                x => x.FullTypeName == typeof(DateTime).FullName
            };

            var propTypeDesc = main.GetTypeDescription(propDescr.TypeDisplayFullName);

            if (propTypeDesc.ArrayDescription.IsArray)
            {
                var enumeratedType = main.GetTypeDescription(propTypeDesc.ArrayDescription.ElementDiplayFullTypeName);

                if (enumeratedType.IsEnumeration)
                {
                    return new DropDownListData
                    {
                        SelectList = enumeratedType.EnumDescription.EnumValues.Select(x => new SelectListItem
                        {
                            Text = x.DisplayName,
                            Value = x.StringRepresentation
                        }).ToList(),
                        CanAddNewItem = false
                    };
                }
            }

            if (propTypeDesc.FullTypeName == typeof(bool).FullName)
            {
                return new DropDownListData
                {
                    SelectList = MySelectListItemExtensions.GetBooleanList(propTypeDesc.IsNullable, opts),
                    CanAddNewItem = false
                };
            }

            if (emptySelectListPredicates.Any(x => x(propTypeDesc)))
            {
                return new DropDownListData 
                {
                    SelectList = new List<SelectListItem>(),
                    CanAddNewItem = true
                } ;
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

                return new DropDownListData
                {
                    CanAddNewItem = false,
                    SelectList = enumsList
                };
            }

            return null;
        }

        private static UserInterfaceType GetUserInterfaceType(CrocoTypeDescription desc)
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