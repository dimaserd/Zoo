namespace Zoo.ServerJs.Abstractions
{
    /// <summary>
    /// Параметры для вызова метода
    /// </summary>
    public interface IJsWorkerMethodCallParameters
    {
        /// <summary>
        /// Получить параметр
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetParameter<T>();

        /// <summary>
        /// Получить длину параметров
        /// </summary>
        /// <returns></returns>
        int GetParamsLength();
    }
}