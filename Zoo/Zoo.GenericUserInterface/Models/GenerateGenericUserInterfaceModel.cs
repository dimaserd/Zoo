using Croco.Core.Documentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Extensions;

namespace Zoo.GenericUserInterface.Models
{
    public class GenerateGenericUserInterfaceModel
    {
        public static readonly string NotSelectedText = "Не выбрано";

        /// <summary>
        /// Префикс для построения модели
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Описание типа данных
        /// </summary>
        public CrocoTypeDescription TypeDescription { get; set; }

        /// <summary>
        /// Блоки для свойств
        /// </summary>
        public List<UserInterfaceBlock> Blocks { get; set; }

        /// <summary>
        /// Провайдер значений
        /// </summary>
        public GenericUserInterfaceValueProvider ValueProvider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="valueProvider"></param>
        /// <param name="modelPrefix"></param>
        /// <returns></returns>
        public static GenerateGenericUserInterfaceModel Create(IHaveGenericUserInterface model, GenericUserInterfaceValueProvider valueProvider, string modelPrefix)
        {
            var typeDesc = CrocoTypeDescription.GetDescription(model.GetType());

            if(!typeDesc.IsClass)
            {
                throw new Exception("Обобщенный интерфейс поддерживается только у типов, являющихся классами");
            }

            return new GenerateGenericUserInterfaceModel
            {
                TypeDescription = typeDesc,
                Blocks = model.GetUserInterfaceBuildModel(),
                Prefix = modelPrefix,
                ValueProvider = valueProvider
            };
        }

        public static GenerateGenericUserInterfaceModel CreateFromObject(object model, string modelPrefix)
        {
            var prov = GenericUserInterfaceValueProvider.Create(model);

            return CreateFromType(model.GetType(), modelPrefix, prov);
        }

        public static GenerateGenericUserInterfaceModel CreateFromType(Type type, string modelPrefix, GenericUserInterfaceValueProvider valueProvider)
        {
            var desc = CrocoTypeDescription.GetDescription(type);

            return new GenerateGenericUserInterfaceModel
            {
                TypeDescription = desc,
                Blocks = GetBlocks("", desc),
                Prefix = modelPrefix,
                ValueProvider = valueProvider
            };
        }

        private static UserInterfaceBlock GetBlockForEnumerable(CrocoTypeDescription prop)
        {
            return new UserInterfaceBlock
            {
                InterfaceType = UserInterfaceType.MultipleDropDownList,
                PropertyName = prop.PropertyDescription.PropertyName,
                SelectList = GetSelectList(prop),
            };
        }

        private static List<UserInterfaceBlock> GetBlocks(string prefix, CrocoTypeDescription desc)
        {
            List<UserInterfaceBlock> result = new List<UserInterfaceBlock>();

            foreach (var prop in desc.Properties)
            {
                result.AddRange(GetBlocksFromProperty(prefix, prop));
            }

            return result;
        }

        private static List<UserInterfaceBlock> GetBlocksFromProperty(string prefix, CrocoTypeDescription prop)
        {
            if (!prop.IsClass && !prop.IsEnumerable)
            {
                return new List<UserInterfaceBlock>
                {
                    new UserInterfaceBlock
                    {
                        PropertyName = $"{prefix}{prop.PropertyDescription.PropertyName}",
                        InterfaceType = GetUserInterfaceType(prop),
                        SelectList = GetSelectList(prop)
                    }
                };
            }

            if(prop.IsEnumerable)
            {
                return new List<UserInterfaceBlock>()
                {
                    GetBlockForEnumerable(prop)
                };
            }

            if (prop.IsClass)
            {
                return GetBlocks($"{prefix}{prop.PropertyDescription.PropertyName}.", prop);
            }

            throw new Exception("не продумано");
        }

        private static List<MySelectListItem> GetSelectList(CrocoTypeDescription prop)
        {
            List<Func<CrocoTypeDescription, bool>> emptySelectListPredicates = new List<Func<CrocoTypeDescription, bool>>
            {
                x => x.IsEnumerable, 
                x => x.FullTypeName == typeof(DateTime).FullName
            };
            
            if(prop.IsEnumerable && prop.EnumeratedType.IsEnumeration)
            {
                var enumeratedType = prop.EnumeratedType;

                return enumeratedType.EnumDescription.EnumValues.Select(x => new MySelectListItem
                {
                    Text = x.DisplayName,
                    Value = x.StringRepresentation
                }).ToList();
            }

            if (prop.FullTypeName == typeof(bool).FullName)
            {
                return MySelectListItemExtensions.GetBooleanList(prop.IsNullable);
            }

            if (emptySelectListPredicates.Any(x => x(prop)))
            {
                return new List<MySelectListItem>();
            }

            if (prop.IsEnumeration)
            {
                var enumsList = new List<MySelectListItem>();

                if(prop.IsNullable)
                {
                    enumsList.Add(new MySelectListItem
                    {
                        Value = null,
                        Selected = true,
                        Text = NotSelectedText
                    });
                }

                enumsList.AddRange(prop.EnumDescription.EnumValues.Select(x => new MySelectListItem
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