using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Consts
{
    /// <summary>
    /// Заранее определенные системой атрибуты
    /// </summary>
    public static class DefinedAttributes
    {
        /// <summary>
        /// Атрибут обозначающий, что свойство является электронный адресом
        /// </summary>
        public static readonly UserInterfaceBlockAttribute Email = new UserInterfaceBlockAttribute
        {
            Name = "Email"
        };
    }
}