namespace Zoo.GenericUserInterface.Models.Values
{
    public class GenericUserInterfacePropertyGenericValue<T>
    {
        public string PropertyName { get; set; }

        public T Value { get; set; }
    }
}
