using Microsoft.Extensions.Logging;

namespace Zoo.ServerJs.Consts
{
    internal static class EventIds
    {
        internal static class CallRemoteApi
        {
            public static readonly string RemoteApiNotFound = "CallRemoteApi.RemoteApiNotFound";
            public static readonly string NoWorkerWithNameFound = "CallRemoteApi.NoWorkerWithNameFound";
            public static readonly string CallNotSucceeded = "CallRemoteApi.CallNotSucceeded";
            public static readonly string CallLogged = "CallRemoteApi.CallLogged";
            public static readonly string ResponseDeserializationError = "CallRemoteApi.ResponseDeserializationError";
        }

        internal static class JsExecutorComponents
        {
            public static readonly string GetExternalComponentNotFound = "JsExecutorComponents.GetExternalComponent.NotFound";
            public static readonly string GetExternalComponentFound = "JsExecutorComponents.GetExternalComponent.Found";


            public static readonly string GetJsWorkerNotFound = "JsExecutorComponents.GetExternalComponent.NotFound";
            public static readonly string GetJsWorkerFound = "JsExecutorComponents.GetExternalComponent.Found";

            public static readonly string CallExternalComponentOnResult = "JsExecutor.CallExternalComponent.OnResult";
        }
    }
}