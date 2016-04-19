angular.module('orderApp')
.controller("orderDashboardCtrl", ['$scope', '$state', 'orderDashboardSvc', 'orderSvc', 'domainDataSvc', 'configSvc', function ($scope, $state, orderDashboardSvc, orderSvc, domainDataSvc, configSvc) {
    var edittedModel = orderSvc.getModel();

    $scope.configSvc = configSvc;

    $scope.isDraft = true;

    $scope.initPageInfo = function() {
        $scope.pageInfo = {
            savedOrderPage: {
                itemsPerPage: 10,
                currentPage: 1,
                pageInterval: 5
            },
            blockedOrderPage: {
                itemsPerPage: 10,
                currentPage: 1,
                pageInterval: 5
            },
            completedOrderPage: {
                itemsPerPage: 50,
                currentPage: 1,
                pageInterval: 5
            },
        };
    };

    $scope.allOrders = {
        savedOrder: [],
        blockedOrder: [],
        completedOrder: [],
        blockedTotalCount: 0,
        completedTotalCount: 0
    };

    $scope.init = function () {
        $scope.initPageInfo();
        if ($scope.configSvc.pcn) {
            $scope.showDraft();
            $scope.showBlocked();
            $scope.showCompleted();
            edittedModel.isEdit = false;
        }
    };

    $scope.clearCustomer = function () {
        if ($scope.configSvc.pcn) {
            $scope.configSvc.pcn = '';
            $scope.allOrders.savedOrder = [];
            $scope.allOrders.blockedOrder = [];
            $scope.allOrders.completedOrder = [];
            $scope.allOrders.completedTotalCount = 0;
            $scope.allOrders.blockedTotalCount = 0;
            $scope.getDraftPromise = orderDashboardSvc.clearOrder();
            $scope.isDraft = true;
            edittedModel.isEdit = false;
        }
    };

    $scope.showDraft = function () {
        $scope.allOrders.savedOrder = [];
        $scope.getDraftPromise = orderDashboardSvc.getDraft($scope.configSvc.pcn).then(function (data) {
            $scope.allOrders.savedOrder = data.SavedOrder;
        });
       
        $scope.isDraft = true;
    };

    $scope.showBlocked = function () {
        $scope.allOrders.blockedTotalCount = 0;
        $scope.allOrders.blockedOrder = [];
        var purchaseOrderStatusCodeTable = ["HPN"];
        $scope.getBlockedPromise = orderDashboardSvc.getBlocked($scope.configSvc.pcn, null, null, $scope.pageInfo.blockedOrderPage.currentPage, $scope.pageInfo.blockedOrderPage.itemsPerPage, purchaseOrderStatusCodeTable).then(function (data) {
            $scope.allOrders.blockedOrder = data.Order;
            $scope.allOrders.blockedTotalCount = data.TotalCount;
        });
       
        $scope.isDraft = true;
    };

    $scope.showCompleted = function () {
        $scope.allOrders.completedTotalCount = 0;
        $scope.allOrders.completedOrder = [];
        var purchaseOrderStatusCodeTable = ["HAD"];
        $scope.getCompletedPromise = orderDashboardSvc.getCompleted($scope.configSvc.pcn, null, null, $scope.pageInfo.completedOrderPage.currentPage, $scope.pageInfo.completedOrderPage.itemsPerPage, purchaseOrderStatusCodeTable).then(function (data) {
            $scope.allOrders.completedOrder = data.Order;
            $scope.allOrders.completedTotalCount = data.TotalCount;
        });
    };

    $scope.showPurchaseOrderDetail = function (order) {
        var purchaseOrderStatusCodeTable = ["HPN"];
        orderDashboardSvc.getCompleted($scope.configSvc.pcn, order.EndCustomerNumber, order.Id, 1, 1, purchaseOrderStatusCodeTable).then(function (data) {
            edittedModel.orderModel = data.Order;
            edittedModel.lineItems = data.LineItems;
            $state.go("ordereditor", { draftOrderId: null, purchaseOrderId: order.Id });
        });
        
    };

    $scope.redirectToSummary = function (order) {
        var purchaseOrderStatusCodeTable = ["HAD"];
        orderDashboardSvc.getCompleted($scope.configSvc.pcn, order.EndCustomerNumber, order.Id, 1, 1, purchaseOrderStatusCodeTable).then(function (data) {
            edittedModel.orderModel = data.Order;
            edittedModel.lineItems = data.LineItems;
            $state.go("ordersummary");
        });
    };

    $scope.blockedOrdersChanging = function () {
        $scope.allOrders.blockedOrder = [];
        var purchaseOrderStatusCodeTable = ["HPN"];
        $scope.getDraftPromise = orderDashboardSvc.getCompleted($scope.configSvc.pcn, null, null, $scope.pageInfo.blockedOrderPage.currentPage, $scope.pageInfo.blockedOrderPage.itemsPerPage, purchaseOrderStatusCodeTable).then(function (data) {
            $scope.allOrders.blockedOrder = data.Order;
            $scope.allOrders.blockedTotalCount = data.TotalCount;
        });
    };

    $scope.compeletedOrdersChanging = function () {
        var purchaseOrderStatusCodeTable = ["HAD"];
        $scope.allOrders.completedOrder = [];
        $scope.getDraftPromise = orderDashboardSvc.getCompleted($scope.configSvc.pcn, null, null, $scope.pageInfo.completedOrderPage.currentPage, $scope.pageInfo.completedOrderPage.itemsPerPage, purchaseOrderStatusCodeTable).then(function (data) {
            $scope.allOrders.completedOrder = data.Order;
            $scope.allOrders.completedTotalCount = data.TotalCount;
        });
    };

    $scope.dialogStatus = {
        deleteOrderDialogOpen: false
    };

    $scope.openDeleteDialog = function (orderList, index, e) {
        e.preventDefault();
        e.stopPropagation();
        $scope.dialogStatus.deleteOrderDialogOpen = true;
        $scope.dialogStatus.orderList = orderList;
        $scope.dialogStatus.index = index;
    };

    $scope.deleteOrder = function () {
        var orderList = $scope.dialogStatus.orderList;
        var index = $scope.dialogStatus.index;
        if(orderList && index){
            if (index >= 0 && index < orderList.length) {
                $scope.deleteOrderPromise = orderSvc.deleteOrder(orderList[index].DraftOrderId).then(function (response)
                {
                    if (response === 0) {
                        orderList.splice(index, 1);
                    }
                });
            };
        };
        $scope.dialogStatus.deleteOrderDialogOpen = false;
    };

    $scope.cancelDelete = function () {
        if ($scope.dialogStatus.orderList && $scope.dialogStatus.index) {
            delete $scope.dialogStatus.orderList;
            delete $scope.dialogStatus.index;
        };
        $scope.dialogStatus.deleteOrderDialogOpen = false;
    };

    $scope.showDraftOrderDetail = function (order) {
        $state.go("ordereditor", { draftOrderId: order.DraftOrderId, purchaseOrderId: null });
    };
}]);
