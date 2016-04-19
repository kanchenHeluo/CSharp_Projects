angular.module("opCommonModule")
.directive("opSlideIn", function () {
    var dialog, isInWatchListProcessing = false;
   
    return {
        restrict: 'E',
        transclude: true,
        scope: {
            Title: "@title",
            ngOpen: '=',
            ngOnClose: '&',
        },
        template: '<div style="transform: translateZ(0);-webkit-transform: translateZ(0);position: fixed; top:0; left:0; width: 100%; background-color: #000; opacity:0.8; bottom:0;"></div>' +
                  '<div style="transform: translateZ(0);-webkit-transform: translateZ(0);position: fixed; width:70%; bottom:0; overflow-y:auto; z-index: 1000; top:0; right: 0; background-color:#FFF; padding:20px;">' +
                        '<header>' +
                            '<h2 style="width:100%;">{{Title}}<span class="symbol remove flr" tabindex="0"></span></h2>' +
                        '</header>' +
                        '<section class="content" ng-transclude></section>' +
                    '</div>',
        link: function (scope, element) {
            $(element).hide();
            function show() {
                $(element).show();
                $('body').css('overflow', 'hidden');
                $('body').css('height', '100%');
            }
            function hide() {
                //$(element).hide("slide", { direction: "right" }, 1000);
                $(element).hide();
                $('body').css('overflow', 'auto');
                $('body').css('height', 'auto');
                if (!isInWatchListProcessing) {
                    scope.$apply(function () {
                        scope.ngOpen = false;
                    });
                } else {
                    scope.ngOpen = false;
                    isInWatchListProcessing = false;
                }
                if (scope.ngOnClose) {
                    scope.ngOnClose();
                }
            }
            $(element).find('.remove').click(hide);
            scope.hide = hide;
           scope.$watch('ngOpen', function (value) {
                if (value) {
                    show();
                } else {
                    isInWatchListProcessing = true;
                    hide();
                }
            });
        }
    };
});