angular.module('orderApp')
.controller('orderEditorCtrl', ['$scope', '$rootScope', '$document', '$interval', '$timeout', '$state', '$location', '$stateParams', 'orderEditorSetting', 'orderSvc', 'domainDataSvc', 'configSvc', function ($scope, $rootScope, $document, $interval, $timeout, $state, $location, $stateParams, orderEditorSetting, orderSvc, domainDataSvc, configSvc) {
    // handle URL parameter 'id'
    $scope.updateAndSubmitStatus = false;
    var draftOrderId = $stateParams.draftOrderId;
    var purchaseOrderId = $stateParams.purchaseOrderId;
    var edittedModel = orderSvc.getModel();
    $scope.globalModels = edittedModel;
    if (draftOrderId) {
        edittedModel.isEdit = true;
        $scope.getOrderDetailPromise = orderSvc.getOrderDetail(draftOrderId, configSvc.pcn).then(function (orderDetails) {
            if (orderDetails) {
                edittedModel.orderModel = orderDetails.Header;
                edittedModel.lineItems = orderDetails.LineItems;
                edittedModel.agreementModel = orderDetails.Header; //order stores some of the key fields in agreement, let's see of it's sufficient
                edittedModel.agreementModel.LookUpDate = orderDetails.Header.UsageDate || new Date();

                
            }
        });
    } else if (purchaseOrderId) {
        $scope.updateAndSubmitStatus = true;
        edittedModel.agreementModel = edittedModel.orderModel;
        edittedModel.agreementModel.LookUpDate = edittedModel.orderModel.UsageDate || new Date();
        edittedModel.isEdit = true;
    } else {
        edittedModel.agreementModel = null;
        edittedModel.orderModel = null;
        edittedModel.orderHeaderAttributes = null;
        edittedModel.lineItems = [];
        edittedModel.shipment = null;
        edittedModel.isEdit = false;
    }


    var editStatus = orderSvc.getModel().isEdit;
    $scope.edit = { status: editStatus };

    $scope.sessionExpirePromptDlgInfo = {
        isOpen: false,
    }

    if (!orderSvc.getModel().isEdit) {
        return; // there is no lock/unlock/session timeout if OrderEditor is in creating new order status
    }

    orderSvc.getModel().isLockedByMe = false;
    //orderSvc.lockDraftOrder(draftOrderId).then(function (data) {
    //    if (data === 0) {
    //        // lock is successful!
    //        orderSvc.getModel().isLockedByMe = true;
    //        startMonitor();
    //    }
    //});
    
    var startMonitor = function () {
        // add keyboard, mouse event listener to track user activity
        var lastUserActivityTime = new Date(), hasNewActivity = false;
        var inputEventHandler = function () {
            lastUserActivityTime = new Date();
            hasNewActivity = true;
        };
        var inputEvents = 'keypress click mouseover mouseout';
        $document.on(inputEvents, inputEventHandler);

        // listen to location change
        var locationChangeStartListner = $rootScope.$on("$locationChangeStart", function (event, newUrl, oldUrl) {
            if (oldUrl.indexOf('OrderEditor') === -1) {
                return; //not leave from order editor page
            }

            if ($scope.sessionExpirePromptDlgInfo.isOpen) { // if the prompt dlg shows, first close it and then jump
                event.preventDefault();
                $scope.sessionExpirePromptDlgInfo.isOpen = false;
                $timeout(function () {
                    $location.url(newUrl);
                });
            }
            
            orderSvc.unlockDraftOrder(orderSvc.getModel().orderModel.DraftOrderId).then(function () {
                orderSvc.getModel().isLockedByMe = false;
            });
        });

        // start a timer to handle time-out of session
        var maxLockMinutes = orderEditorSetting.maxLockMinutes;
        var timeSpanforPrompt = (maxLockMinutes - 1) * 60 * 1000, timeSpanforExit = maxLockMinutes * 60 * 1000, timerInterval = 15 * 1000; // milliseconds
        var timer = $interval(function () {
            if (hasNewActivity) {
                orderSvc.lockDraftOrder(orderSvc.getModel().orderModel.DraftOrderId); // update latest locked date
                hasNewActivity = false;
            }

            var now = new Date();
            var timespan = now.getTime() - lastUserActivityTime.getTime();
            // session expired
            if (timespan > timeSpanforExit) {
                orderSvc.saveDraftOrder(orderSvc.getModel().agreementModel, orderSvc.getModel().orderModel, orderSvc.getModel().lineItems).then(function () {
                    orderSvc.unlockDraftOrder(orderSvc.getModel().orderModel.DraftOrderId).then(function () {
                        orderSvc.getModel().isLockedByMe = false;

                        if ($scope.sessionExpirePromptDlgInfo.isOpen) {
                            $scope.sessionExpirePromptDlgInfo.isOpen = false;
                            $timeout(function () {
                                $state.go('dashboard');
                            });
                        } else {
                            $state.go('dashboard');
                        }
                    });
                });
                return;
            }
            // it's time to reminder user
            if (timespan > timeSpanforPrompt) {
                $scope.sessionExpirePromptDlgInfo.isOpen = true;
            }
        }, timerInterval);

        $scope.$on("$destroy", function () {
            locationChangeStartListner();
            $document.off(inputEvents, inputEventHandler);
            $interval.cancel(timer);
        });
    };
}]);