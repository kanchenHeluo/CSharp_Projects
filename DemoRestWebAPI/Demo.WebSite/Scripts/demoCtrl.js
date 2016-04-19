demoApp.controller('demoCtrl', ['$scope', '$http', demoCtrl]);

function demoCtrl($scope, $http) {

    $scope.searchPo_PoId = '';
    $scope.searchPoLi_PoId = '';
    $scope.searchPoLi_LiId = '';

    $scope.result = [];

    $scope.getPurchaseOrder = function () {
        $http.get(rootUrl + "Home/GetPurchaseOrderAsync?poId=" + $scope.searchPo_PoId).then(function (data) {
            if (data) {
                $scope.results = data.data;
            }
        });
        //todo: why $http.get(url, params) not work?
    }

    $scope.getPurchaseOrderLineItem = function () {
        $http.get(rootUrl + "Home/GetPurchaseOrderLineItemAsync?poId=" + $scope.searchPoLi_PoId + "&poLiId=" + $scope.searchPoLi_LiId).then(function (data) {
            if (data) {
                $scope.results = data.data;
            }
        });
    }
}