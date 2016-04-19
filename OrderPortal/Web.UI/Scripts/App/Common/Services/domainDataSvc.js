angular.module("opCommonModule")
.factory("domainDataSvc", ['opAjaxSvc', '$q', 'localStorage', function (opAjaxSvc, $q, localStorage) {

    var _getDomainData = function (name, refresh) {
        var deferred = $q.defer();
    //    localStorage.clearStore(name);
        var ucs = localStorage.retrieve(name);
        if (ucs == undefined || refresh) {
            opAjaxSvc.get("../../DomainData/" + name).then(function (data) {
                localStorage.save(name, data);
                deferred.resolve(data);
            });
        } else {
            deferred.resolve(ucs);
        }

        return deferred.promise;
    };

   
    function usageCountries() {
        return _getDomainData("UsageCountries");
    }

    function salesLocations() {
        return _getDomainData("SalesLocations");
    }
    function flagReasons() {
        return _getDomainData("FlagReasons");
    }

    function lineItemAttributes(global, lineItem) {
        var request = {
            AgreementId: global.orderModel.AgreementId,
            PartNumber: lineItem.PartNumber,
            LookupDate: lineItem.POLIUsageDate,
            ItemId:lineItem.ProductId,
            ProgramCodes: [global.orderModel.ProgramCode],
            ProductFamilyCodes: [],
            ProductTypeCodes: [],
            PurchaseOrderTypes: []
        };
        if (global.orderModel.PurchaseOrderTypeCode) {
            request.PurchaseOrderTypes.push(global.orderModel.PurchaseOrderTypeCode);
        }
        if (lineItem.ProductFamilyCode) {
            request.ProductFamilyCodes.push(lineItem.ProductFamilyCode);
        }
        if (lineItem.ProductTypeCode) {
            request.ProductTypeCodes.push(lineItem.ProductTypeCode);
        }
        return opAjaxSvc.get("../../DomainData/LineItemAttributes", request).then(function (data) {
            lineItem.attributes = data;
            if (data) {
                lineItem.IsOlsItem = data.IsOls;
            }
            return data;
        });
    }

    function orderHeaderAttributes(agreementId, lookupDate) {
        return opAjaxSvc.get("../../DomainData/OrderHeaderAttributes", {
            agreementId: agreementId,
            poolIds: lookupDate,
            offerings: lookupDate
        });
    }

    function GetSupportedLanguages() {
        return _getDomainData("GetLanguages");
    }
    
    return {
        UsageCountries: usageCountries,
        SalesLocations:salesLocations,
        FlagReasons: flagReasons,
        LineItemAttributes: lineItemAttributes,
        OrderHeaderAttributes: orderHeaderAttributes,
        GetSupportedLanguages : GetSupportedLanguages 
    };

}]);