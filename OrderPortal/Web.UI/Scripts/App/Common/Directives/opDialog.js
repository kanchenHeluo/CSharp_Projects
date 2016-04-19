angular.module("opCommonModule")
.directive("opDialog", function () {
    var dialog, isInWatchListProcessing = false;
   
    return {
        restrict: 'E',
        transclude: true,
        scope: {
            Title: "@title",
            ngOpen: '=',
            ngOnClose: '&',
        },
        template: '<div class="ModalDialog" style="z-index:1000"><header><h2>{{Title}}</h2><span class="symbol remove" tabindex="0" data-item="dialogClose"></span></header><section class="content" ng-transclude></section></div>',
        link: function (scope, element) {
            dialog = ModalDialogObject.Create({
                container: $(element),
                onClose: function () {
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
            });
            scope.$watch('ngOpen', function (value) {
                if (dialog) {
                    if (value) {
                        dialog.Show();
                    } else {
                        isInWatchListProcessing = true;
                        dialog.Close();
                    }
                }
            });
        }
    };
});