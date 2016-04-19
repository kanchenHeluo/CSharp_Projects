angular.module("opCommonModule")
.factory("opAjaxSvc", ['$http', '$q', 'opNotificationSvc', function ($http, $q, opNotificationSvc) {

    function beginRequest(supressGlobal) {
        opNotificationSvc.status.isProcessing = !supressGlobal;
        opNotificationSvc.status.message = '';
        opNotificationSvc.status.error = '';
    }

    function endRequest(deferred, data) {
        opNotificationSvc.status.isProcessing = false;
        if (data.IsRedirect === true) {
            if (data.RedirectLocation) {
                window.location = data.RedirectLocation;
            } else {
                failed(deferred, '404', data);
            }
            return;
        }
        if (data.HasBase) {
            if (data.Message) {
                opNotificationSvc.message(data.Message);
            }
            if (data.Error) {
                opNotificationSvc.error(data.Error);
                deferred.reject(data.Data);
                return;
            }
            deferred.resolve(data.Data);
        } else {
            deferred.resolve(data);
        }
    }

    function failed(deferred, status, data) {
        opNotificationSvc.status.isProcessing = false;
        opNotificationSvc.error('Unexpected Error');//TODO: we should redirect here
        console.log(data);
        deferred.reject(status);
    }

    function get(url, params, supressGlobal) {
        var deferred = $q.defer();
        beginRequest(supressGlobal);
        var request = $http.get(url, { params: params })
            .success(function (data) {
            endRequest(deferred, data);
        }).error(function (data, status) {
            failed(deferred, status, data);
        });
        return deferred.promise;
    }

    function post(url, params, supressGlobal) {
        var deferred = $q.defer();
        beginRequest(supressGlobal);
        $http.post(url, params).success(function (data) {
            endRequest(deferred, data);
        }).error(function (data, status) {
            failed(deferred, status, data);
        });
        return deferred.promise;
    }

    function postFormData(url, params, supressGlobal) {
        var deferred = $q.defer();
        beginRequest(supressGlobal);
        $http.post(url, params, {
            headers: { 'Content-Type': undefined },
            transformRequest: function(data) { return data; }
        }).success(function (data) {
            endRequest(deferred, data);
        }).error(function (data, status) {
            failed(deferred, status, data);
        });
        return deferred.promise;
    }

    return {
        ajaxStatus: opNotificationSvc,
        get: get,
        post: post,
        postFormData: postFormData
};
}]);