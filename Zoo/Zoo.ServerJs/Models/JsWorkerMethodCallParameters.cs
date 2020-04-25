using Croco.Core.Utils;
using Zoo.ServerJs.Statics;

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
        /// Получить параметр
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetParameter<T>()
        {
            var param = _parameters[_currentIndex];

            _currentIndex++;

            var json = ZooSerializer.Serialize(param);

            return ZooSerializer.Deserialize<T>(json);
        }
    }
}