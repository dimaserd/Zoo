var api = {};
api.PrepareResult = function (result) {
    if (result !== undefined) {
        return JSON.parse(result);
    }
}
api.Call = function (workerName, methodName, p1, p2, p3, p4) {
    var result = inner_js_api.Call(workerName, methodName, p1, p2, p3, p4);

    return this.PrepareResult(result);
}
api.CallExternal = function (componentName, methodName, methodPayLoad) {
    var result = inner_js_api.CallExternal(componentName, methodName, methodPayLoad);

    return this.PrepareResult(result);
}
api.CallRemoteWorkerMethod = function (remoteName, workerName, methodName, p1, p2, p3, p4) {
    var result = inner_js_api.CallRemoteWorkerMethod(remoteName, workerName, methodName, p1, p2, p3, p4);

    return this.PrepareResult(result);
}