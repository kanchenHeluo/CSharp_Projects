angular.module("opCommonModule")
.factory("channelPartnerSvc", ['opAjaxSvc', function (opAjaxSvc) {
    return {
        selectPartner: function (pcnNo) {
            return opAjaxSvc.post(RootUrl + "dashboard/DashboardHome/UpdatePartner", pcnNo);
        }
    }
}]);