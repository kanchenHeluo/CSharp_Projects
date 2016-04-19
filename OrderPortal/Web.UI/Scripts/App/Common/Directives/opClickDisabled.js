angular.module("opCommonModule")
.directive('opClickDisabled', function () {
    return {
        compile: function (tElement, tAttrs, transclude) {
            //handle ngClick
            tAttrs["ngClick"] = ("ng-click", "!(" + tAttrs["clickDisabled"] + ") && (" + tAttrs["ngClick"] + ")");
            //handle href
            tElement.on("click", function (e) {
                if (tElement.hasClass("disabled")) {
                    e.preventDefault();
                }
            });
            return function (scope, iElement, iAttrs) {
                scope.$watch(iAttrs["clickDisabled"], function (newValue) {
                    if (newValue !== undefined) {
                        iElement.toggleClass("disabled", newValue);
                    }
                });
            };
        }
    };
})