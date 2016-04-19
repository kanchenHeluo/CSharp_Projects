angular.module("opCommonModule")
.controller("choosepartnerCtrl", ['$scope', '$window', 'channelPartnerSvc', function ($scope, $window, channelPartnerSvc) {
   
    $scope.submit = function () {
        if($("#SelectPartner").val() != "")
        {
            $scope.pcnNo = { pcnNo: $("#SelectPartner").val() };          
            channelPartnerSvc.selectPartner($scope.pcnNo).then(function (data) {                
                if(ModalDialogObject !=null)
                {
                    ModalDialogObject.Create({ source: "#myDialog", open: false });
                    /*hide the modal window*/                            
                    if ($window.location.href.toLowerCase().indexOf("/dashboardHome/index")<0)
                        $window.location.href = RootUrl+"Dashboard/DashboardHome/Index";
                }
            });
        }
    };

    $scope.getmodal = function () {
        if (ModalDialogObject != null) {
                ModalDialogObject.Create({ source: "#myDialog", open: true });
               
            } 
    };

}]);