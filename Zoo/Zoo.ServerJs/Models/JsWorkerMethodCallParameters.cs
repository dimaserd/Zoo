using Croco.Core.Utils;

namespace Zoo.ServerJs.Models
{
    public class JsWorkerMethodCallParameters
    {
        private readonly object[] _parameters;

        private int _currentIndex = 0;

        public JsWorkerMethodCallParameters(object[] parameters)
        {
            _parameters = parameters;
        }

        private string GetJson<T>(object param)
        {
            if (typeof(T) == typeof(int))
            {
                return param.ToString();
            }

            return Tool.JsonConverter.Serialize(param);
        }

        public T GetParameter<T>()
        {
            var param = _parameters[_currentIndex];

            _currentIndex++;

            var json = GetJson<T>(param);

            return Tool.JsonConverter.Deserialize<T>(json);
        }
    }
}