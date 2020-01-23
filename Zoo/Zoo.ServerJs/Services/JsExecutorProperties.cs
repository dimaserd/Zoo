using System;
using System.Collections.Generic;
using Jint;
using Zoo.ServerJs.Abstractions;

namespace Zoo.ServerJs.Services
{
    public class JsExecutorProperties
    {
        public Action<Engine> EngineProperties { get; set; }

        public List<IJsWorker> JsWorkers { get; set; }
    }
}