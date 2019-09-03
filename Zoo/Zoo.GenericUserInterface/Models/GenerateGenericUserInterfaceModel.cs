using Croco.Core.Logic.Models.Documentation;
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
        /// <summary>
        /// Префикс для построения модели
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Описание типа данных
        /// </summary>
        public CrocoTypeDescription TypeDescription { get; set; }

        public List<UserInterfaceBlock> Blocks { get; set; }

        public GenericUserInterfaceValueProvider ValueProvider { get; set; }

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
                Blocks = GetBlocks(desc),
                Prefix = modelPrefix,
                ValueProvider = valueProvider
            };
        }

        private static UserInterfaceBlock GetBlockForEnumerable(CrocoTypeDescription prop)
        {
            return new UserInterfaceBlock
            {
                InterfaceType = UserInterfaceType.MultipleDropDownList,
                PropertyName = prop.PropertyName,
                SelectList = GetSelectList(prop),
            };
        }

        private static List<UserInterfaceBlock> GetBlocks(CrocoTypeDescription desc)
        {
            List<UserInterfaceBlock> result = new List<UserInterfaceBlock>();

            foreach (var prop in desc.Properties)
            {
                result.AddRange(GetBlocksFromProperty(prop));
            }

            return result;
        }

        private static List<UserInterfaceBlock> GetBlocksFromProperty(CrocoTypeDescription prop)
        {
            if (!prop.IsClass && !prop.IsEnumerable)
            {
                return new List<UserInterfaceBlock>
                {
                    new UserInterfaceBlock
                    {
                        PropertyName = prop.PropertyName,
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
                return GetBlocks(prop);
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

                return enumeratedType.EnumValues.Select(x => new MySelectListItem
                {
                    Text = x.DisplayName,
                    Value = x.StringRepresentation
                }).ToList();
            }

            if (prop.FullTypeName == typeof(bool).FullName)
            {
                return MySelectListItemExtensions.GetBooleanList();
            }

            if (emptySelectListPredicates.Any(x => x(prop)))
            {
                return new List<MySelectListItem>();
            }

            if (prop.IsEnumeration)
            {
                return prop.EnumValues.Select(x => new MySelectListItem
                {
                    Value = x.StringRepresentation,
                    Text = x.DisplayName
                }).ToList();
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
