angular.module("orderApp")
.controller("batchUploadCtrl", ['$scope', 'orderSvc', function ($scope, orderSvc) {

    $scope.filecontent = "";
    $scope.setfilecontent = function (content) {
        $scope.filecontent = content;
    };

    $scope.filename = "";
    $scope.setfilename = function (filename) {
        $scope.$evalAsync(function () {
            $scope.filename = filename;
        });
    };

    $scope.disableUpload = true;
    $scope.disableUploadFlag = function (flag) {
        $scope.$evalAsync(function () {
            $scope.disableUpload = flag;
        });
    };

    $scope.browseflag = false;

    $scope.placeholderstring = "No file chosen...";
    $scope.placeholder = $scope.placeholderstring;

    $scope.showUploadButton = true;
    $scope.showFixButton = false;
    $scope.errorMessages = [];

    $scope.setErrorMessageForExcel = function (message) {
        for (index = 0; index < message.length; index++) {
            $scope.errorMessages.push(message[index].Message);
        }
    };

    $scope.setErrorMessageForAgreement = function (message) {
        for (index = 0; index < message.length; index++) {
            $scope.errorMessages.push('Agreement: ' + message[index] + ' is not found.');
        }
    };

    $scope.setErrorMessageForBatch = function (message) {
        for (index = 0; index < message.length; index++) {
            $scope.errorMessages.push(message[index]);
        }
    };

    $scope.cleanErrorMessage = function () {
        if ($scope.errorMessages.length > 0) {
            $scope.errorMessages.splice(0, $scope.errorMessages.length);
        }
    };

    $scope.unsubmitflag = false;

    $scope.Upload = function () {

        var formData = new FormData();
        formData.append('filename', $scope.filename);
        formData.append('unsubmitflag', $scope.unsubmitflag);
        formData.append('filecontent', $scope.filecontent);

        $scope.uploadPromise = orderSvc.upload(formData).then(function (data) {
            if (data.source && data.source == 'excel' && data.errors) {
                $scope.setErrorMessageForExcel(data.errors);
                $scope.Reload();
            }
            else if (data.source && data.source == 'agreement' && data.errors) {
                $scope.setErrorMessageForAgreement(data.errors);
                $scope.Reload();
            }
            else if (data.source && data.source == 'batch' && data.orders) {
                var errorFlag = false;
                for (i = 0; i < data.orders.length; i++) {
                    for (j = 0; j < data.orders[i].Comments.length; j++) {
                        $scope.setErrorMessageForBatch([data.orders[i].PurchaseOrderNumber + ': ' + data.orders[i].Comments[j].Comment]);
                        errorFlag = true;
                    }
                }
                if (errorFlag) {
                    $scope.browseflag = true;
                    $scope.disableUpload = true;
                    $scope.showUploadButton = false;
                    $scope.showFixButton = true;
                } else {
                    $scope.Clear();
                }
            }
        });
    };

    $scope.Clear = function () {
        $scope.placeholder = $scope.placeholderstring;
        $scope.filecontent = "";
        $scope.filename = "";
        $scope.disableUpload = true;
        $scope.showUploadButton = true;
        $scope.showFixButton = false;
        $scope.cleanErrorMessage();
    };

    $scope.Reload = function () {
        $scope.placeholder = $scope.placeholderstring;
        $scope.filecontent = "";
        $scope.filename = "";
        $scope.disableUpload = true;
        $scope.showUploadButton = true;
        $scope.showFixButton = false;
    };
}]);