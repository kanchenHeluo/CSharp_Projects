angular.module("opCommonModule")
.directive("opRequired", function () {
    return {
        restrict: 'A',
        require: ['^opForm'],
        link: function (scope, element, attrs) {

            var errMsg = attrs['opValidationMsg']; 
            var parentFormCtrl = $(element).controller('op-form');
            var guid = jQuery.guid++;
            if (!element.validatorFlag) element.validatorFlag = {};
            var model = attrs['ngModel'] || attrs['ngBind'];

            var validate = function () {

                var ngModel = scope.$eval(model);
                var validationEnable = scope.$eval(attrs['validationEnable']);
                var errorClass = attrs['opErrorClass'] || 'error';
                
                if (validationEnable == false) {
                    return true;
                }
                if (!ngModel) {
                    if (!errMsg) {
                        errMsg = attrs['name'] + " is required.";
                    }
                    parentFormCtrl.$pushError(guid, errMsg, false, element, errorClass);
                    return false;
                } else {
                    parentFormCtrl.$setPristine();
                    parentFormCtrl.$popError(guid, false, element, errorClass);
                    return true;
                }
            };

            $(element).on('blur', validate);

            scope.$watch(model, function (newValue, oldValue) {
                //if (newValue != oldValue) {
                    validate();
                //};
            });

            scope.$watch(function () {
                return scope.$eval(element.attr('validation-enable'));
            }, function () {
                validate();
            })

            parentFormCtrl.$register(validate);
        }
    }
});