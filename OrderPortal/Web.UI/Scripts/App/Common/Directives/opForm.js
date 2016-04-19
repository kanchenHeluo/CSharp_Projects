angular.module('opCommonModule')
.directive("opForm", function () {
    return {
        restrict: 'E',
        transclude: true,
        template: '<div ng-transclude></div>',
        controller: ['$element', '$scope', function ($element, $scope) {
            this.$pristine = true;
            var error = {};
            var validateFunctors = [];
            var parentFormCtrl = $element.parent().controller('op-form');

            this.$pushError = function (errId, errMsg, handled, element, errorClass) {
                if ($scope.$eval($element.attr('validation-enable')) == false) {
                    return;
                }

                error[errId] = {handled: handled, errMsg: errMsg};

                var messages = createMessage();
                var alias = this.alias;
                $scope.$evalAsync(function () {
                    $scope.errorMessageSource[alias] = messages;
                });

                if (!handled) {
                    if (!element.validatorFlag[errId]) {
                        element.validatorFlag[errId] = true;
                    };
                    $(element).addClass(errorClass);
                };

                if (parentFormCtrl) {
                    parentFormCtrl.$pushError(errId, errMsg, true, element, errorClass);
                }
            };

            this.$popError = function (errId, handled, element, errorClass) {
                var existFlag = error[errId];
                delete error[errId];

                var messages = createMessage();
                var alias = this.alias;
                $scope.$evalAsync(function() {
                    $scope.errorMessageSource[alias] = messages;
                });

                if (!handled && existFlag) {
                    delete element.validatorFlag[errId];
                    if (Object.keys(element.validatorFlag) == 0) {
                        $(element).removeClass(errorClass);
                    };
                };

                if (parentFormCtrl) {
                    parentFormCtrl.$popError(errId, true);
                }
            };

            var createMessage = function() {
                var messages = [];
                for (var property in error) {
                    if (!error[property].handled) {
                        messages.push(error[property].errMsg);
                    };
                };
                return messages;
            }

            this.$setPristine = function () {
                this.$pristine = false;
                if (parentFormCtrl) {
                    parentFormCtrl.$setPristine();
                }
            };

            this.$validate = function () {
                if ($scope.$eval($element.attr('validation-enable')) == false) {
                    return true;
                }
                var result = true;
                for (var i = 0; i < validateFunctors.length; i++) {
                    if (!validateFunctors[i]()) {
                        result = false;
                    }
                }
                return result;
            };

            this.$isValid = function () {
                return Object.keys(error).length == 0;
            };


            var thisValidate = this.$validate;
            $scope.$watch(function () {
                return $scope.$eval($element.attr('validation-enable'));
            }, function (newValue) {
                if (newValue) {
                    thisValidate();
                }
            });

            this.$register = function (functor) {
                validateFunctors.push(functor);
            };

            if (parentFormCtrl) {
                parentFormCtrl.$register(this.$validate);
            };
        }],
        compile: function () {
            return {
                pre: function (scope, element, attr, controller) {
                    if (!scope.validators) {
                        scope.validators = {};
                    }
                    controller.alias = attr.name;
                    if (controller.alias) {
                        scope.validators[controller.alias] = controller;
                    }

                    if (!scope.errorMessageSource) {
                        scope.errorMessageSource = {};
                    };

                    controller.errorMessages = scope.errorMessageSource;
                    scope.errorMessageSource[controller.alias] = [];

                    scope.$on('Re-Register-OpForm', function () {
                        controller.alias = attr.name;
                        if (controller.alias) {
                            scope.validators[controller.alias] = controller;
                        }
                    });
                }
            }
        }
    }
});