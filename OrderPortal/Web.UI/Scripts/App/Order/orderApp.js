angular.module('orderApp', ['opCommonModule', 'ui.router'])
.config(['$stateProvider', '$urlRouterProvider', '$locationProvider', '$httpProvider', function ($stateProvider, $urlRouterProvider, $locationProvider, $httpProvider) {
    $stateProvider
        .state("dashboard", {
            url: "/",
            templateUrl: "../Resource/Dashboard",
            controller: "orderDashboardCtrl"
        })
        .state("ordereditor", {
            url: "/OrderEditor/:draftOrderId/:purchaseOrderId",
            templateUrl: "../Resource/OrderEditor",
            controller: "orderEditorCtrl"
        })
        .state("ordersummary", {
            url: "/OrderSummary",
            templateUrl: "../Resource/OrderSummary",
            controller: "orderSummaryCtrl"
        })
        .state("batchupload", {
            url: "/BatchUpload",
            templateUrl: "../Resource/BatchUpload",
            controller: "batchUploadCtrl"
        })
        .state("mockupui", {
            url: "/MockUpUI",
            templateUrl: "../Resource/MockUpUI",
            controller: "mockuiCtrl"
        })
        .state("orderrenewal", {
            url: "/OrderRenewal",
            templateUrl: "../Resource/OrderRenewal",
            controller: "orderRenewalCtrl"
        });
    $urlRouterProvider.otherwise("/");
    $locationProvider.html5Mode(false);

    $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    $httpProvider.interceptors.push('opHttpInterceptor');

}])
.factory('opHttpInterceptor', ['$q', function ($q) {
    return {
        'response': function (response) {
            var reqAuth = response.headers('REQUIRES_AUTH');
            if (response.status === 200 && reqAuth === '1') {
                window.location.reload();
            }
            return response;
        },

        'responseError': function (rejection) {
            if (rejection.status === 401) {
                window.location.reload();
            }
            return $q.reject(rejection);
        }
    };
}]);