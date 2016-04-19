angular.module('orderApp')
.factory("autocompleteSvc", ['opAjaxSvc', function (opAjaxSvc) {
    return {
        selectPartner: function (patnerPCN, customerPCN, agreementNumber) {
            return opAjaxSvc.get(RootUrl + "Autocomplete/GetData", patnerPCN, customerPCN, agreementNumber);
        }

    }
}]);