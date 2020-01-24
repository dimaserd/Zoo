using Croco.Core.Utils;

namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// Параметры вызова javascript метода
    /// </summary>
    public class JsWorkerMethodCallParameters
    {
        private readonly object[] _parameters;

        private int _currentIndex = 0;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="parameters"></param>
        public JsWorkerMethodCallParameters(object[] parameters)
        {
            _parameters = parameters;
        }

        /// <summary>
        /// Получить json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        private string GetJson<T>(object param)
        {
            if (typeof(T) == typeof(int))
            {
                return param.ToString();
            }

            return Tool.JsonConverter.Serialize(param);
        }

        /// <summary>
        /// Получить параметр
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetParameter<T>()
        {
            var param = _parameters[_currentIndex];

            _currentIndex++;

            var json = GetJson<T>(param);

            return Tool.JsonConverter.Deserialize<T>(json);
        }
    }
}