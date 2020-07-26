﻿using Croco.Core.Documentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Extensions;

namespace Zoo.GenericUserInterface.Models
{
    /// <summary>
    /// Модель для создания обощенного интерфейса
    /// </summary>
    public class GenerateGenericUserInterfaceModel
    {
        /// <summary>
        /// Префикс для построения модели
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Описание типа данных
        /// </summary>
        public CrocoTypeDescriptionResult TypeDescription { get; set; }

        /// <summary>
        /// Блоки для свойств
        /// </summary>
        public List<UserInterfaceBlock> Blocks { get; set; }

        /// <summary>
        /// Провайдер значений
        /// </summary>
        public GenericUserInterfaceValueProvider ValueProvider { get; set; }

        /// <summary>
        /// Сериализованные кастомные данные
        /// </summary>
        public string CustomDataJson { get; set; }

        /// <summary>
        /// Кастомно переопределить
        /// </summary>
        /// <param name="overridings"></param>
        /// <returns></returns>
        public Task OverrideAsync(GenericUserInterfaceOverridings overridings)
        {
            var overriding = overridings.GetOverriding(TypeDescription.GetMainTypeDescription().FullTypeName);
            
            if(overriding == null)
            {
                return Task.CompletedTask;
            }

            return overriding(this);
        }

        /// <summary>
        /// Создать из объекта используя провайдер значений
        /// </summary>
        /// <param name="model"></param>
        /// <param name="modelPrefix"></param>
        /// <returns></returns>
        public static GenerateGenericUserInterfaceModel CreateFromObject(object model, string modelPrefix)
        {
            var prov = GenericUserInterfaceValueProvider.Create(model);

            return CreateFromType(model.GetType(), modelPrefix, prov, null);
        }

        /// <summary>
        /// Создать из типа
        /// </summary>
        /// <param name="type"></param>
        /// <param name="modelPrefix"></param>
        /// <param name="valueProvider"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static GenerateGenericUserInterfaceModel CreateFromType(Type type, string modelPrefix, GenericUserInterfaceValueProvider valueProvider, GenericInterfaceOptions opts)
        {
            var desc = CrocoTypeDescription.GetDescription(type);

            if(opts == null)
            {
                opts = GenericInterfaceOptions.Default();
            }

            return new GenerateGenericUserInterfaceModel
            {
                TypeDescription = desc,
                Blocks = GetBlocks("", desc.GetMainTypeDescription(), desc, opts),
                Prefix = modelPrefix,
                ValueProvider = valueProvider
            };
        }

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

        private static List<UserInterfaceBlock> GetBlocks(string prefix, CrocoTypeDescription currentType, CrocoTypeDescriptionResult desc, GenericInterfaceOptions opts)
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
            if(interfaceType != UserInterfaceType.TextBox)
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
                typeof(short).FullName
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

            if(propTypeDescription.IsEnumerable)
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

        private static List<MySelectListItem> GetSelectList(CrocoPropertyReferenceDescription propDescr, CrocoTypeDescriptionResult main, GenericInterfaceOptions opts)
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
                    return enumeratedType.EnumDescription.EnumValues.Select(x => new MySelectListItem
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
                return new List<MySelectListItem>();
            }

            if (propTypeDesc.IsEnumeration)
            {
                var enumsList = new List<MySelectListItem>();

                if(propTypeDesc.IsNullable)
                {
                    enumsList.Add(new MySelectListItem
                    {
                        Value = null,
                        Selected = true,
                        Text = opts.NotSelectedText
                    });
                }

                enumsList.AddRange(propTypeDesc.EnumDescription.EnumValues.Select(x => new MySelectListItem
                {
                    Value = x.StringRepresentation,
                    Text = x.DisplayName
                }));

                return enumsList;
            }

            return null;
        }

        private static UserInterfaceType GetUserInterfaceType(CrocoTypeDescription desc)
        {
            if (desc.FullTypeName == typeof(bool).FullName)
            {
                return UserInterfaceType.DropDownList;
            }

            if(desc.FullTypeName == typeof(DateTime).FullName)
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