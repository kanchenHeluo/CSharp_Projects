angular.module("orderApp")
.controller("orderShipToCtrl", ["$scope", "orderSvc", "domainDataSvc", function ($scope, orderSvc, domainDataSvc) {
    var id = 0;
    var createObj = function() {
        return { id: id++, AgreementID: $scope.globalModels.agreementModel.AgreementId, AgreementNumber: $scope.globalModels.agreementModel.AgreementNumber, ShipToPartnerNumber: $scope.globalModels.orderModel.DirectCustomerNumber, EndCustomName: $scope.globalModels.agreementModel.EndCustomerName, LicenseProgramCode: $scope.globalModels.agreementModel.ProgramCode };
    };

    $scope.globalModels = orderSvc.getModel();

    $scope.Shipment = null; // for shipment address -- slide in
    $scope.shipmentAddressSelected = {};

    $scope.shipmentAddressList = []; 
    $scope.showShipmentAddressDetails = []; 
    $scope.showShipmentAddressList = true;

    //dialogs status and flags
    $scope.dialogStatus = {
        addRequestDialogOpen: false,
        addAddressDialogOpen: false,
        loadTag: false // used for prevent first time dialog open
    };
    //$scope.justSaveTag = false;
    $scope.addrFormDirty = false;

    $scope.errorMessage = [];

    //init
    $scope.checkShipTo = function () {
        var len = $scope.globalModels.lineItems.length;
        var isLotus = $scope.globalModels.agreementModel.IsLotus;
        for (var i = 0; i < len; i++) {
            var attrs = $scope.globalModels.lineItems[i].attributes;
            if (attrs && Object.keys(attrs).length > 0) {
                if ((isLotus) || (attrs.IsShipmentEnabled)) {
                    $scope.checkShipToFlag = true;
                    return true;
                }
            }
        }
        return false;
    };

    $scope.$watch('checkShipToFlag', function (value) {
        if (value) {
            $scope.GetSupportedLanguagesPromise = domainDataSvc.GetSupportedLanguages().then(function (data) {
                $scope.Languages = data;
            });
            $scope.ShipToUsageCountriesPromise = domainDataSvc.UsageCountries().then(function (data) {
                $scope.UsageCountries = data;
            });
        }
    });

    $scope.initShipToRequestForm = function () {
        if ($scope.globalModels.orderModel.PurchaseOrderShipToId) {
            var len = $scope.shipmentAddressList.length;
            for (var i = 0; i < len; i++) {
                if ($scope.shipmentAddressList[i].PurchaseOrderShipToId == $scope.globalModels.orderModel.PurchaseOrderShipToId) {
                    $scope.shipmentAddressSelected = $scope.shipmentAddressList[i];
                    break;
                }
            }
        }
    };

    $scope.getShipmentInitInfo = function () {
        var agreementId = !$scope.globalModels.agreementModel ? 2048621 : $scope.globalModels.agreementModel.AgreementId;
        $scope.getAllShipmentListPromise = orderSvc.getAllShipmentList(agreementId).then(function (data) {
            $scope.shipmentAddressList = data;
            $scope.showshipmentAddressList = false;
            var len = $scope.shipmentAddressList.length;
            for (var i = 0; i < len; i++) {
                $scope.showShipmentAddressDetails[i] = false;
            }
            $scope.initShipToRequestForm();
        });
    };

    $scope.initShipmentDropdownListDisplay = function () {
        $scope.showshipmentAddressList = true;
        var len = $scope.shipmentAddressList.length;
        for (var i = 0; i < len; ++i) {
            $scope.showShipmentAddressDetails[i] = false;
        }
    };

    //request dialog
    $scope.selectShipmentAddress = function (index) {
        $scope.shipmentAddressSelected = $scope.shipmentAddressList[index];
        $scope.globalModels.orderModel.PurchaseOrderShipToId = $scope.shipmentAddressSelected.PurchaseOrderShipToId;
    };

    $scope.displayShipmentAddressDetails = function (index, event) {
        $scope.showShipmentAddressDetails[index] = true;
        $scope.showshipmentAddressList = false;
        
        event.stopPropagation();
    };

    $scope.openAddRequestDialog = function (item) {
        if (item) {
            $scope.globalModels.orderModel.PurchaseOrderShipToId = item.PurchaseOrderShipToId;
            $scope.shipmentAddressSelected = item;
        } else {
            $scope.initShipToRequestForm();
        }
        $scope.dialogStatus.addRequestDialogOpen = true;
        $scope.dialogStatus.addAddressDialogOpen = false;
        $scope.dialogStatus.loadTag = true;
        $scope.Shipment = null;
    };
    
    $scope.shipmentAddressCloseCallback = function () {
        $scope.errorMessage = null;
        if ($scope.dialogStatus.loadTag) {
            $scope.dialogStatus.addRequestDialogOpen = true;
        }
    };

    $scope.openAddAddressDialog = function (item) {
        if (item && Object.keys(item).length > 0) {
            $scope.Shipment = angular.copy(item, createObj()); 
        } else {
            $scope.Shipment = createObj();
        }
        $scope.dialogStatus.addAddressDialogOpen = true;
        $scope.dialogStatus.addRequestDialogOpen = false;
    };

    $scope.newShipmentAddress = function () {
        if (!$scope.addrFormDirty || confirm("are you sure to give up the existing changes?")) {
            $scope.Shipment = createObj();
            $scope.addrFormDirty = false;
        }
    };

    $scope.triggerBlur = function (event) {
        if (event) {
            $(event.target).parent().find('input[op-required]').blur();
        }
    };

    $scope.saveShipmentRequest = function () {
        $scope.globalModels.shipment = $scope.shipmentAddressSelected;
        $scope.saveDraftOrderPromise = orderSvc.saveDraftOrder($scope.globalModels.agreementModel, $scope.globalModels.orderModel, $scope.globalModels.lineItems, $scope.globalModels.shipment).then(function(data) {
            if (data) {
                orderSvc.getModel().orderModel.DraftOrderId = data;
            }
        });
    };

    $scope.saveShipmentAddress = function () {
        $scope.Shipment.OrganizationName = $scope.globalModels.agreementModel.EndCustomerName;
        $scope.saveShipmentAddressPromise = orderSvc.saveShipmentAddress($scope.Shipment).then(function (data) {
            if (data) {
                if ($scope.Shipment.PurchaseOrderShipToId != data.ShipToId) {
                    $scope.Shipment.PurchaseOrderShipToId = data.ShipToId;
                    $scope.shipmentAddressList.push($scope.Shipment);
                } else {
                    var len = $scope.shipmentAddressList.length;
                    for (var i = 0; i < len; i++) {
                            if ($scope.shipmentAddressList[i].PurchaseOrderShipToId == data.ShipToId) {
                            $scope.shipmentAddressList[i] = $scope.Shipment;
                            break;
                        }
                    }
                }
                $scope.addrFormDirty = false;
            }
        });
    };

    $scope.validateShipmentAddress = function () {
        $scope.errorMessage = [];
        $scope.Shipment.OrganizationName = $scope.globalModels.agreementModel.EndCustomerName;
        $scope.validateShipmentAddressPromise = orderSvc.validateShipmentAddress($scope.Shipment).then(function (data) {
            $scope.Shipment.LastValidatedDate = data.Key;
            $scope.errorMessage = data.Value;
        });
    };

    $scope.showAddingShipToOrder = function() {
        if ($scope.Shipment && $scope.Shipment.PurchaseOrderShipToId) {
            return !$scope.addrFormDirty;
        }
        return false;
    };
    
    $scope.deleteShipmentAddress = function (shipment) {
        if (shipment && Object.keys(shipment).length > 1) {
            if (shipment.PurchaseOrderShipToId) {
                if (confirm("You are removing a existing shipping address")) {
                    $scope.deleteShipmentAddressPromise = orderSvc.deleteShipmentAddress(shipment.PurchaseOrderShipToId).then(function(data) {
                        if (data) {
                            $scope.Shipment = createObj();
                            if ($scope.Shipment.PurchaseOrderShipToId == shipment.PurchaseOrderShipToId) {
                                $scope.Shipment.PurchaseOrderShipToId = undefined;
                            }
                            if ($scope.shipmentAddressSelected.PurchaseOrderShipToId == shipment.PurchaseOrderShipToId) {
                                $scope.shipmentAddressSelected = undefined;
                            }
                            var len = $scope.shipmentAddressList.length;
                            for (var i = 0; i < len; i++) {
                                if ($scope.shipmentAddressList[i].PurchaseOrderShipToId == shipment.PurchaseOrderShipToId) {
                                    $scope.shipmentAddressList.splice(i, 1);
                                    break;
                                }
                            }
                        }
                    });
                }
            } else {
                if (confirm("are you giving up the changes?")) {
                    $scope.Shipment = createObj();
                }
            }
        } else {
            alert("No Ship To Address To Delete Here.");
        }
    };

    $scope.$watch("Shipment", function(newValue, oldValue) {
        if (newValue && oldValue && newValue.id == oldValue.id) {
            //dirty
            $scope.addrFormDirty = true;
            if (newValue.PurchaseOrderShipToId != oldValue.PurchaseOrderShipToId) { // save new created scenario
                $scope.addrFormDirty = false;
            }

        } else {
            $scope.addrFormDirty = false;
        }
    }, true);

    $scope.$watch('globalModels.agreementModel.AgreementId', function (value) {
        if (value) {
            $scope.getShipmentInitInfo();
        };
    });
}]);