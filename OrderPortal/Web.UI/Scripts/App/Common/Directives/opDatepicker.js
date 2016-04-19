angular.module("opCommonModule")
  .directive('opDatepicker', function () {

    function link(scope, element, attrs) {

        var action = function (e) {
            var target = $(e.target);
            if (target != null && e.target.tagName === "INPUT" && ((e.type === 'focus') || (e.type === 'click'))) {
                oneInstance.open();
            }
        };

        var date = !scope.ngBind || (new Date(scope.ngBind)) == "Invalid Date" ? new Date() : new Date(scope.ngBind);

        $(element).kendoDatePicker({
            format: scope.dateFormat || "MM/dd/yyyy",
            max: scope.max ? new Date(scope.max) : undefined,
            min: scope.min ? new Date(scope.min) : undefined
        }).on("click focus", action);

        var oneInstance = $(element).data("kendoDatePicker");
        oneInstance.value(new Date(date));
        scope.$evalAsync(function() {
            scope.ngBind = parseDate(new Date(oneInstance.value()));
        });

        var parseDate = function(input) {
            return input.getFullYear() + '/' + (input.getMonth() + 1) + '/' + input.getDate();
        };

        oneInstance.bind("change", function () {
            scope.$evalAsync(function () {
                if (oneInstance.value()) {
                    scope.ngBind = parseDate(new Date(oneInstance.value()));
                } else {
                    scope.ngBind = '';
                }
                
            });
            oneInstance.close();
        });

        scope.$watch('isDisabled', function (value) {
            if (value != undefined) {
                if (!value) {
                    oneInstance.enable(false);
                } else {
                    oneInstance.enable();
                };
            };
        });

        scope.$watch('dateFormat', function (value) {
            if (value) {
                oneInstance.setOptions({
                    format: scope.dateFormat
                });
            };
        });

        scope.$watch('ngBind', function (value) {
            if (value) {
                oneInstance.value(new Date(value));
                scope.$evalAsync(function () {
                    scope.ngBind = parseDate(new Date(oneInstance.value()));
                });
            };
        });
    };

    return {
        restrict: 'A',
        scope: {
            isDisabled: '=',
            dateFormat: '=',
            ngBind: "=",
            max: "@",
            min: "@"
        },
        link: link
    };
});