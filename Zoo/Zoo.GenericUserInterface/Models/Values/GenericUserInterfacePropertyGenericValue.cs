namespace Zoo.GenericUserInterface.Models.Values
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericUserInterfacePropertyGenericValue<T>
    {
        /// <summary>
        /// Название свойства
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public T Value { get; set; }
    }
}