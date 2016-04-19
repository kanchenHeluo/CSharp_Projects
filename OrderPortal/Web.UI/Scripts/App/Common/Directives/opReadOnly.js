angular.module("opCommonModule")
.directive("opReadOnly", ["$rootScope", function($rootScope) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            scope.$watch(function () {
                with (scope) {
                    return eval(attrs["opReadOnly"]);
                }
            }, function (newValue) {
                checkItem(newValue);
            });

            var checkItem = function( readonly ) {
                $(element).find("*").andSelf().each(function() {
                    var item = $(this);
                    //hold for data-readonly
                    if (item.is('[data-readonly="true"]  *,[data-readonly="true"]')) {
                        return;
                    }
                    if (readonly) {
                        //styles changed 
                        item.addClass("disabled");
                        if (item.is('input,select,button')) {
                            item.attr('disabled', 'disabled');
                        }
                        if (item.is('button, input[type=button], input[type=submit]')) {
                            item.hide();
                        }
                        if (item.is('.calendar')) {
                            item.hide();
                        }
                        // hold for write-only
                        if (item.is('[data-writeonly="true"]')) {
                            item.hide();
                        }
                        if (item.is('[data-write="true"]')) {
                            item.off();
                        }
                    } else {
                        item.removeClass("disabled");

                        if (item.is('input,select,button')) {
                            item.removeAttr('disabled');
                        }
                        if (item.is('button, input[type=button], input[type=submit]')) {
                            item.show();
                        }
                        if (item.is('.calendar')) {
                            item.show();
                        }
                        if (item.is('[data-writeonly="true"]')) {
                            item.show();
                        }
                        if (item.is('[data-write="true"]')) {
                            item.on();
                        }
                    }
                });
            };

            $rootScope.$on('$includeContentLoaded', function(event) {
                with (scope) { // TODO: and if event scope under current scope with read-only tag
                    var data = eval(attrs["opReadOnly"]);
                    checkItem(data);
                }
            });
        }
    }
}]);