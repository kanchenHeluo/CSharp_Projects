angular.module('orderApp')
.factory('orderDashboardSvc', ['opAjaxSvc', function (opAjaxSvc) {
    return {
        //get draft orders
        getDraft: function (pcn) {
            return opAjaxSvc.post('../../ManageVLOrder/GetDraftOrder/', {pcnFilter: pcn});
        },
        //clear orders
        clearOrder: function () {
            return opAjaxSvc.post("../../ManageVLOrder/GetDraftOrder/", { pcnFilter: null });
        },
        //get completed orders
        getCompleted: function (pcn, customerNumber, id, pageNumber, pageSize, purchaseOrderStatusCodeTable) {
            var request = {
                pcnFilter: pcn,
                customerNumber: customerNumber,
                id: id,
                pageNumber: pageNumber || 1,
                pageSize: pageSize || 50,
                purchaseOrderStatusCodeTable: purchaseOrderStatusCodeTable
            };
            return opAjaxSvc.get('GetOrdersWithStatus', request);
        },
        // get blocked orders
        getBlocked: function (pcn, customerNumber, id, pageNumber, pageSize, purchaseOrderStatusCodeTable) {
            var request = {
                pcnFilter: pcn,
                customerNumber: customerNumber,
                id: id,
                pageNumber: pageNumber || 1,
                pageSize: pageSize || 50,
                purchaseOrderStatusCodeTable: purchaseOrderStatusCodeTable
            };
            return opAjaxSvc.get('GetOrdersWithStatus', request);
        },
    };
}]);

