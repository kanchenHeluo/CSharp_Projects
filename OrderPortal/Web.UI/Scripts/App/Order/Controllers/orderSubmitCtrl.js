angular.module('orderApp')
.controller("orderSubmitCtrl", ['$scope', 'orderSvc', '$state', '$location', function ($scope, orderSvc, $state, $location) {
    $scope.submitDisabled = function () {
        var globalModels = orderSvc.getModel();
        if (globalModels.orderModel) {
            if ($scope.validators['orderEditForm'].$isValid() == false) {
                globalModels.orderModel.ValidateFlag = false;
            }
            return !globalModels.orderModel.ValidateFlag;
        }
        return true;
    };

    $scope.updateOrder = function () {
        if ($scope.validators['orderEditForm'].$validate()) {
            var globalModels = orderSvc.getModel();
            if (globalModels && globalModels.orderModel && globalModels.orderModel.Id) {
                //TODO: call update and submit action
            }
        }
    };

    $scope.saveAndValidateOrder = function () {
        var ret = $scope.validators['orderEditForm'].$validate();
        var globalModels = orderSvc.getModel();
        
        orderSvc.saveDraftOrder(globalModels.agreementModel, globalModels.orderModel, globalModels.lineItems).then(function (data) {
            if (data) {
                orderSvc.getModel().orderModel.DraftOrderId = data;
            }
            // do server-side validation
            orderSvc.validateDraftOrder(globalModels.agreementModel, globalModels.orderModel, globalModels.lineItems).then(function (data) {
                orderSvc.getModel().serverSideValidationErrorMsgs = data;
                globalModels.orderModel.ValidateFlag = ret && (data && Object.keys(data).length <= 0);
            });
        });
        
        
    };

    $scope.submitOrder = function () {
        if ($scope.validators['orderEditForm'].$validate()) {
            var globalModels = orderSvc.getModel();
           $scope.submitOrderPromise = orderSvc.submitOrder(globalModels.agreementModel, globalModels.orderModel, globalModels.lineItems, globalModels.shipment).then(function(data) {
                if (data) {
                    globalModels.orderModel = data;
                    $state.go("dashboard");
                }
            });
        }
    };
}]);