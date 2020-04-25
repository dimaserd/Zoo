using System;
using System.Collections.Generic;
using Jint;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Zoo.ServerJs.Tests
{
    public class JIntTests
    {
        static string Call()
        {
            return JsonConvert.SerializeObject(new int[] { 1, 2, 3, 4 });
        }

        static void Log(params object[] parameters)
        {
            Logs.Add(JsonConvert.SerializeObject(parameters));
        }

        static readonly List<string> Logs = new List<string>();

        [Test]
        public void Test4()
        {
            var engine = new Engine();

            engine.SetValue("api", new
            {
                Call = new Func<string>(Call),
            });

            engine.SetValue("console", new
            {
                Log = new Action<object[]>(Log)
            });

            var script = "var t = api.Call();\n";

            script += "console.log(t);\n";
            script += "console.log(t.length);\n";
            script += "for(var i = 0; i < t.length; i++) {\n";

            script += "console.log('Result', i, t[i]); \n}";

            engine.Execute(script);
        }
    }
}