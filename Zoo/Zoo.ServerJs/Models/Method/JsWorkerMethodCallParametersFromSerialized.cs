using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Models.Method
{
    internal class JsWorkerMethodCallParametersFromSerialized : IJsWorkerMethodCallParameters
    {
        private readonly object[] _parameterJsons;

        private int _currentIndex = 0;

        internal JsWorkerMethodCallParametersFromSerialized(string[] parameterJsons)
        {
            _parameterJsons = parameterJsons;
        }

        public T GetParameter<T>()
        {
            var param = _parameterJsons[_currentIndex];

            _currentIndex++;

            var json = ZooSerializer.Serialize(param);

            return ZooSerializer.Deserialize<T>(json);
        }

        public int GetParamsLength()
        {
            return _parameterJsons?.Length ?? 0;
        }
    }
}