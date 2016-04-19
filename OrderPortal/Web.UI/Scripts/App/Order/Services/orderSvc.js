angular.module("orderApp")
.factory("orderSvc", ['$http', '$q', 'opAjaxSvc', 'urlSvc', function ($http, $q, opAjaxSvc, urlSvc) {
    var globalModel = {
        orderModel: {},
        agreementModel: null,
        lineItems: [],
        shipment: {},
        isEdit: false,
        isLockedByMe: true,
        serverSideValidationErrorMsgs: []
    }
    
    return {
        getModel: function () {
            return globalModel;
        },

        agreementSearch: function (agreementSearchParams) {
            return opAjaxSvc.post("../../ManageVLOrder/SearchAgreement", agreementSearchParams);
        },
        
        //searchProduct: function (searchProduct) {
        //    return opAjaxSvc.post("../../ManageVLOrder/SearchProduct", searchProduct);
        //},

        searchOpportunity: function (request) {
            return opAjaxSvc.post("../../ManageVLOrder/SearchOpportunity", request);
        },

        searchSku: function (item, global, outTotalCount, pageNumber, pageSize) {
            if (global.orderModel) {
                var request = {
                    PartNumber: item.PartNumber,
                    ItemName: item.ItemName,
                    UsageDate: item.POLIUsageDate,
                    //ProductTypeCode
                    ProductFamilyCode : item.ProductFamilyName,
                    PurchaseOrderTypeCode: global.orderModel.PurchaseOrderTypeCode,
                    AgreementId: global.orderModel.AgreementId,
                    AgreementNumber: global.orderModel.AgreementNumber,
                    PoolIds: global.orderHeaderAttributes.AvailablePoolIds,
                    ProgramOfferings: global.orderHeaderAttributes.ProgramOfferingIds,
                    ProgramCode: global.orderModel.ProgramCode,
                    PageSize :pageSize || 100,
                    PageNumber: pageNumber || 1
                };
                return opAjaxSvc.post("../../ManageVLOrder/SearchSku", request)
                    .then(function (data) {
                        if (data) {
                            if (outTotalCount) {
                                outTotalCount.totalCount = data.TotalCount;
                            }
                            data = data.Results;
                        }
                        if (data &&  data.length == 1) {
                            data = data[0];
                            item.PartNumber = data.PartNumber;
                            item.ItemName = data.ItemName;
                            item.ProductId = data.ProductId;
                            item.ProductFamilyCode = data.ProductFamilyCode;
                            item.ProductTypeCode = data.ProductTypeCode;
                            item.QuantityAvailable = data.QuantityAvailable;
                            item.CoverageStartDate = data.CoverageStartDate;
                            item.CoverageEndDate = data.CoverageEndDate;
                            return item;
                        }else if (data && data.length > 1) {
                            return data;
                        }
                        return null;
                    //item.searchedTerm = item.ItemName + ' ' + item.PartNumber;
                });
            }
            return { then: function() {} };
        },

        getCoverageDate: function(programType, poliUsageDate, currentAnvDt, endEffectiveDate, coverageEndDate, subscriptionMonth, billinOptionCode) {
            return opAjaxSvc.post("../../ManageVLOrder/GetCoverageDate", { programType: programType, poliUsageDate: poliUsageDate, 
                                                                                currentAnvDt: currentAnvDt, endEffectiveDate: endEffectiveDate, 
                                                                                coverageEndDate: coverageEndDate, subscriptionMonth: subscriptionMonth,
                                                                                billinOptionCode: billinOptionCode
            });
        },

        getLineItemEstimate: function (lineItem, agreement, poType) {
            return opAjaxSvc.post("../../ManageVLOrder/GetLineItemEstimate", { lineItem: lineItem, agreement: agreement, poType: poType });
        },

        submitOrder: function (agreement, order, lineItems, shipTo) {
            return opAjaxSvc.post(urlSvc.CreateOrder, { agreement: agreement, order: order, lineItems: lineItems, shipTo: shipTo });
        },

        upload: function (data) {
            return opAjaxSvc.postFormData("../../ManageVLOrder/Upload/", data);
        },

        saveDraftOrder: function (agreement, order, lineItems) {
            return opAjaxSvc.post(urlSvc.SaveDraftOrder, { agreement: agreement, order: order, lineItems: lineItems });
        },

        deleteOrder: function (orderId) {
            return opAjaxSvc.post(urlSvc.DeleteDraftOrder, { id: orderId });
        },

        getOrderDetail: function (orderId, pcn) {
            return opAjaxSvc.post("../../ManageVLOrder/GetDraftOrder/", { id: orderId,  pcnFilter: pcn}).then(function(data) {
                globalModel.orderModel = data.Header;
                globalModel.lineItems = data.LineItems;
                return data;
            });
        },

        searchOrderHistory: function (agreementId) {
            return opAjaxSvc.post("../../ManageVLOrder/SearchOrderHistory/", { agreementId: agreementId });
        },

        lockDraftOrder: function (draftOrderId) {
            return opAjaxSvc.post("../../ManageVLOrder/LockDraftOrder/", { id: draftOrderId });
        },

        unlockDraftOrder: function (draftOrderId) {
            return opAjaxSvc.post("../../ManageVLOrder/UnlockDraftOrder/", { id: draftOrderId });
        },

        saveShipmentRequest: function(shipment) {
            return opAjaxSvc.post("../../ManageVLOrder/SaveShipmentRequest/", { shipment: shipment });
        },

        validateShipmentAddress : function(shipment){
            return opAjaxSvc.post("../../ManageVLOrder/ValidateShipToAddress/", { shipment: shipment });
        },

        saveShipmentAddress: function(shipment) {
            return opAjaxSvc.post("../../ManageVLOrder/SaveShipmentAddress/", { shipment: shipment });
        },

        deleteShipmentAddress: function (shipmentId) {
            return opAjaxSvc.post("../../ManageVLOrder/DeleteShipmentAddress/", { id: shipmentId });
        },

        getAllShipmentList: function (agreementId) {
            return opAjaxSvc.post("../../ManageVLOrder/GetAllShipmentList/", { agreementId: agreementId });
        },

        checkShipTo: function(agreement, order, lineItems) {
            return opAjaxSvc.post("../../ManageVLOrder/CheckShipTo/", { agreement: agreement, order: order, lineItems: lineItems });
        },

        validateDraftOrder: function (agreement, order, lineItems) {
            return opAjaxSvc.post("../../ManageVLOrder/ValidateDraftOrder/", { agreement: agreement, order: order, lineItems: lineItems });
        }
    }
}]);