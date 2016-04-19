angular.module('orderApp')
.controller("orderSummaryCtrl", ['$scope', 'orderSvc', function ($scope, orderSvc) {
    $scope.globalModels = orderSvc.getModel();
}]);