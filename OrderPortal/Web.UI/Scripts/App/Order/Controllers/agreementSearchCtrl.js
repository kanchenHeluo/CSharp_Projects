angular.module('orderApp')
.controller("agreementSearchCtrl", ['$scope', 'orderSvc', 'domainDataSvc', 'opNotificationSvc', function ($scope, orderSvc, domainDataSvc, opNotificationSvc) {

    $scope.globalModels = orderSvc.getModel();
    $scope.salesLocationsPromise = domainDataSvc.SalesLocations().then(function (data) {
        $scope.SalesLocations = data;
    });

    $scope.agreementSearchParams = {
        AgreementNumber: "",
        EndCustomerName: "",
        EndCustomerNumber: "",
        SalesLocation: ""
    };

    $scope.agreementSearchAction = function () {
        if (!$scope.globalModels.isEdit && $scope.agreementSearchParams.LookUpDate && ($scope.agreementSearchParams.AgreementNumber || $scope.agreementSearchParams.EndCustomerNumber || $scope.agreementSearchParams.EndCustomerName)) {
            $scope.globalModels.agreementModel = null;
            $scope.agreementSearchPromise = orderSvc.agreementSearch($scope.agreementSearchParams).then(function (data) {
                if (data && $scope.SalesLocations) {
                    for (var i = 0; i < data.length; i++) {
                        var agg = data[i];
                        if (agg.CustomerLocationCode) {
                            for (var j = 0; j < $scope.SalesLocations.length; j++) {
                                if ($scope.SalesLocations[j].Code.trim() == agg.CustomerLocationCode.trim()) {
                                    agg.CountryName = $scope.SalesLocations[j].Name;
                                    break;
                                }
                            }
                        }
                    }
                }
                $scope.agreementSearchResults = data;
            });    
        } else {
            opNotificationSvc.error('Please enter a search criteria to search');
        }
    };

    $scope.agreementClearAction = function () {
        $scope.agreementSearchParams.AgreementNumber = null;
        $scope.agreementSearchParams.EndCustomerName = null;
        $scope.agreementSearchParams.EndCustomerNumber = null;
        $scope.agreementSearchParams.SalesLocation = null;
        $scope.agreementSearchParams.LookUpDate = new Date().getFullYear() + '/' + (new Date().getMonth() + 1) + '/' + new Date().getDate();
    };

    $scope.dialogStatus = {
        selectNewAgreementDialogOpen: false
    };

    $scope.agreementValidateAction = function (event, agreement, selector) {
        var selected = $scope.globalModels.agreementModel;
        if (selected && agreement.AgreementNumber == selected.AgreementNumber) {
            return;
        };
        if (!selected || !selected.AgreementNumber) {
            $scope.agreementValidateActionStep2(event, agreement, selector, selected);
            return;
        };
        //open select new agreement dialog
        $scope.dialogStatus.event = event;
        $scope.dialogStatus.agreement = agreement;
        $scope.dialogStatus.selector = selector;
        $scope.dialogStatus.selected = selected;
        $scope.dialogStatus.selectNewAgreementDialogOpen = true;
    };

    $scope.selectNewAgreement = function () {
        var event = $scope.dialogStatus.event;
        var agreement = $scope.dialogStatus.agreement;
        var selector = $scope.dialogStatus.selector;
        var selected = $scope.dialogStatus.selected;
        if (event && agreement && selected) {
            $scope.agreementValidateActionStep2(event, agreement, selector, selected);
        };
        $scope.dialogStatus.selectNewAgreementDialogOpen = false;
    }

    $scope.cancelSelection = function () {
        delete $scope.dialogStatus.event;
        delete $scope.dialogStatus.agreement;
        delete $scope.dialogStatus.selector;
        delete $scope.dialogStatus.selected;
        $scope.dialogStatus.selectNewAgreementDialogOpen = false;
    }

    $scope.agreementValidateActionStep2 = function (event, agreement, selector, selected) {
        $scope.globalModels.agreementModel = agreement;
        $scope.globalModels.agreementModel.LookUpDate = $scope.agreementSearchParams.LookUpDate;
        $scope.globalModels.lineItems = [];
        $scope.globalModels.shipment = {};
        if ((selector && $(event.toElement || event.target).is(selector)) || !selector) {
            if (selected) {
                selected.selected = false;
            }
            agreement.selected = true;
        }
    };

    //$scope.$watch('agreementSearchParams', function () {
    //    $scope.agreementSearchAction();
    //}, false);
}]);