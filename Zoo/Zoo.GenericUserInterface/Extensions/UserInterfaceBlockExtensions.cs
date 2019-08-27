using System;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Extensions
{
    public static class UserInterfaceBlockExtensions
    {
        public static UserInterfaceBlock GetBooleanDropDownListBlock(string propertyName)
        {
            return new UserInterfaceBlock
            {
                PropertyName = propertyName,
                InterfaceType = UserInterfaceType.DropDownList,
                SelectList = MySelectListItemExtensions.GetBooleanList()
            };
        }

        public static UserInterfaceBlock GetTextBoxBlock(string propertyName)
        {
            return new UserInterfaceBlock
            {
                PropertyName = propertyName,
                InterfaceType = UserInterfaceType.TextBox
            };
        }

        public static UserInterfaceBlock GetTexAreaBlock(string propertyName)
        {
            return new UserInterfaceBlock
            {
                PropertyName = propertyName,
                InterfaceType = UserInterfaceType.TextArea
            };
        }

        public static UserInterfaceBlock GetEnumDropDownList(string propertyName, Type enumType)
        {
            return new UserInterfaceBlock
            {
                PropertyName = propertyName,
                InterfaceType = UserInterfaceType.DropDownList,
                SelectList = MySelectListItemExtensions.GetEnumDropDownList(enumType)
            };
        }
    }
}
