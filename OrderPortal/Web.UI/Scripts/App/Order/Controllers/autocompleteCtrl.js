angular.module('orderApp')
.controller("actrl", ['$scope', 'autocompleteSvc', '$http',
    function actrl($scope, autocompleteSvc, $http) {
         $scope.patnerPCN = { patnerPCN: "9D370B2F" };
        $scope.customerPCN = { customerPCN: "15759383" };
        $scope.agreementNumber = { agreementNumber: "01E60004" };
        //autocompleteSvc.selectPartner($scope.patnerPCN, $scope.customerPCN, $scope.agreementNumber).then(function (data) {
        //    $scope.resultset = data.Data;
        //    });
          
   
}]);
