namespace Zoo.GenericUserInterface.Models
{
    /// <summary>
    /// Моделья для генерации пользовательского интерфейса на удаленной машине
    /// </summary>
    public class GenerateOnRemoteUserInterfaceModel
    {
        /// <summary>
        /// Хост адрес для приложения
        /// </summary>
        public string ApplicationHostUrl { get; set; }

        /// <summary>
        /// Модель описывающая пользовательский интерфейс
        /// </summary>
        public GenerateGenericUserInterfaceModel GemerateInterfaceModel { get; set; }
    }
}