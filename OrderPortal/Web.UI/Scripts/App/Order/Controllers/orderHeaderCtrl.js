angular.module('orderApp')
.controller("orderHeaderCtrl", ['$scope', 'orderSvc', 'domainDataSvc', function ($scope, orderSvc, domainDataSvc) {

    $scope.globalModels = orderSvc.getModel();
    $scope.globalModels.serverSideValidationErrorMsgs = [];
    $scope.assignedToCollection = ["Customer", "Partner"];
    //$scope.assignTo = "Select";
    

    $scope.status = {
        open: $scope.globalModels.isEdit
    };

    $scope.$watch('globalModels.agreementModel', function () {
        if ($scope.globalModels.agreementModel) {
            initOrderHeader();
            $scope.status.open = true;
        }
        else {
            $scope.status.open = false;
        }
    }, true);

    var initOrderHeader = function () {
        if (!$scope.globalModels.isEdit) {
            $scope.globalModels.orderModel = {
                AgreementId: $scope.globalModels.agreementModel.AgreementId,
                AgreementNumber: $scope.globalModels.agreementModel.AgreementNumber,
                ProgramCode: $scope.globalModels.agreementModel.ProgramCode,
                PricingCurrencyCode: $scope.globalModels.agreementModel.CurrencyCode,
                OrderName: $scope.globalModels.orderModel == null ? "" : $scope.globalModels.orderModel.OrderName
            };
        }
        $scope.OrderHeaderAttributesPromise = domainDataSvc.OrderHeaderAttributes($scope.globalModels.agreementModel.AgreementId, $scope.globalModels.agreementModel.LookUpDate).then(function (data) {
            if (data) {
                $scope.globalModels.orderHeaderAttributes = data;

                //$scope.globalModels.orderModel.DirectCustomerNumber = data.DirectPartners[0].Code;
                if (data.DirectPartners.length === 1) {
                    $scope.globalModels.orderModel.DirectCustomerNumber = data.DirectPartners[0].Code;
                } else {
                    $scope.globalModels.orderModel.DirectCustomerNumber = null;
                }
                
                $scope.globalModels.orderModel.IndirectCustomerNumber = data.IndirectPartners && data.IndirectPartners.length > 0 ? data.IndirectPartners[0].Code : null;
                //TODO. the following look should have a batch method in the server to handle later
                if ($scope.globalModels.lineItems) {
                    angular.forEach($scope.globalModels.lineItems, function (item) {
                        item.UsageDate = item.UsageDate || new Date();
                        item.panelDisabled = false;
                        item.isExample = false;
                        item.panelExpanded = true;
                        $scope.LineItemAttributesPromise = domainDataSvc.LineItemAttributes($scope.globalModels, item);
                    });
                }
            }
        });
    };

}]);