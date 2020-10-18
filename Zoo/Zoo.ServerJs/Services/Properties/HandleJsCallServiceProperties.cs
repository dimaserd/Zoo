using Jint;
using System;

namespace Zoo.ServerJs.Services.Properties
{
    internal class JsExecutionContextProperties
    {
        internal JsExecutorComponents Components { get; set; }
        internal Action<Engine> EngineAction { get; set; }
    }
}