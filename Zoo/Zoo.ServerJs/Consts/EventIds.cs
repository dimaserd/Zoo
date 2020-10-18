using Microsoft.Extensions.Logging;

namespace Zoo.ServerJs.Consts
{
    internal static class EventIds
    {
        const int StartIndex = 10000;

        internal static class CallRemoteApi
        {
            public static readonly EventId RemoteApiNotFound = new EventId(StartIndex + 0, "CallRemoteApi.RemoteApiNotFound");
            public static readonly EventId NoWorkerWithNameFound = new EventId(StartIndex + 1, "CallRemoteApi.NoWorkerWithNameFound");
            public static readonly EventId CallNotSucceeded = new EventId(StartIndex + 2, "CallRemoteApi.CallNotSucceeded");
            public static readonly EventId CallLogged = new EventId(StartIndex + 3, "CallRemoteApi.CallLogged");
            public static readonly EventId ResponseDeserializationError = new EventId(StartIndex + 4, "CallRemoteApi.ResponseDeserializationError");
        }
    }
}