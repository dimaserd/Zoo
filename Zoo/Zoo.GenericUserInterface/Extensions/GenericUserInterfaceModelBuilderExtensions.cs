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
            var type = desc.GetTypeDescription(prop.TypeDisplayFullName);

            if(!type.ArrayDescription.IsArray)
            {
                throw new InvalidOperationException("Type is not of type enumerable");
            }

            var enumeratedType = desc.GetTypeDescription(type.ArrayDescription.ElementDiplayFullTypeName);

            if(enumeratedType.IsPrimitive)
            {
                return new UserInterfaceBlock(prop)
                {
                    InterfaceType = UserInterfaceType.MultipleDropDownList,
                    DropDownData = GetSelectListData(prop, desc, opts)
                };
            }

            var newPrefix = AddPropNameToPrefix(prefix, prop.PropertyDescription.PropertyName);

            return new UserInterfaceBlock(prop)
            {
                InterfaceType = UserInterfaceType.GenericInterfaceForArray,
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

            var integerTypes = new[]
            {
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(ushort),
                typeof(short),
                typeof(byte)
            };

            var type = prop.ExtractType();

            var isInteger = integerTypes.Contains(type);

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

                return new UserInterfaceBlock(prop)
                {   
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

                return new UserInterfaceBlock(prop)
                {
                    InterfaceType = UserInterfaceType.GenericInterfaceForClass,
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
                x => x.ExtractType() == typeof(DateTime) || x.ExtractType() == typeof(DateTime?)
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

            if (propTypeDesc.ExtractType() == typeof(bool) || propTypeDesc.ExtractType() == typeof(bool?))
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
                return ForEnumDropDownDataBuilder.GetDropDownListData(propTypeDesc, opts);
            }

            return null;
        }

        private static UserInterfaceType GetUserInterfaceType(CrocoTypeDescription desc)
        {
            var type = desc.ExtractType();

            if (type == typeof(bool) || type == typeof(bool?))
            {
                return UserInterfaceType.DropDownList;
            }

            if (type == typeof(DateTime) || type == typeof(DateTime?))
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