using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Models.Method
{
    /// <summary>
    /// Параметры вызова javascript метода
    /// </summary>
    internal class JsWorkerMethodCallParameters : IJsWorkerMethodCallParameters
    {
        private readonly object[] _parameters;

        private int _currentIndex = 0;


        internal JsWorkerMethodCallParameters(object[] parameters)
        {
            _parameters = parameters;
        }

        
        public T GetParameter<T>()
        {
            var param = _parameters[_currentIndex];

            _currentIndex++;

            var json = ZooSerializer.Serialize(param);

            return ZooSerializer.Deserialize<T>(json);
        }

        public int GetParamsLength()
        {
            return _parameters?.Length ?? 0;
        }
    }
}