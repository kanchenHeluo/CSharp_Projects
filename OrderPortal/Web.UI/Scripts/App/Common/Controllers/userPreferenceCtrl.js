angular.module('opCommonModule')
.controller('userPreferenceCtrl', ['$scope', '$window', 'opAjaxSvc', function ($scope, $window, opAjaxSvc) {
    $scope.userPreferenceDialogStatus = {
        isOpen: false
    };

    $scope.userPreference = {};
    
    $scope.getUserPreference = function () {
        opAjaxSvc.get('/OrderPortal/Order/UserMgmt/GetUserPreference').then(function (data) {
            angular.copy(data, $scope.userPreference);
        });
    };

    var settingDlgOpened = false;
    $scope.openUserPreference = function () {
        settingDlgOpened = true;
        $scope.userPreferenceDialogStatus.isOpen = true;
    };

    $scope.savePreference = function () {
        if (settingDlgOpened) {
            opAjaxSvc.post('/OrderPortal/Order/UserMgmt/SetUserPreference',  $scope.userPreference).then(function (data) {
                if (data) {
                    $window.location.reload();
                }
            });
            settingDlgOpened = false;
        }
    };
}])