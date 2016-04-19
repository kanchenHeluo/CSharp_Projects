angular.module("opCommonModule")
.directive("opPagination", ['$timeout', function ($timeout) {
    return {
        restrict: 'E',
        scope: {
            opCurPage: '=',
            opTotalItem: '=',
            opPageInterval: '=',
            opItemPerPage: '=',
            opPageChange: '&'
        },

        template: '<div class="fll"><ul class="pagination pagination-sm">'
                    + '<li ng-class="preDisabled"><a ng-click="clickPrev()">Prev</a></li>'
                    + '<li ng-class="curActive(pageNo)" ng-repeat="pageNo in pages" ng-if="pageNo>=opCurPage && pageNo<opCurPage+opPageInterval"><a ng-click="clickPage(pageNo)">{{pageNo}}</a></li>'
                    + '<li ng-class="nextDisabled"><a ng-click="clickNext()">Next</a></li>'
                    + '</ul></div><div class="ms-stdtext layout-paging flr">Displaying items  {{(opCurPage-1)*opItemPerPage+1}}-{{(opCurPage*opItemPerPage) > opTotalItem? opTotalItem: (opCurPage*opItemPerPage)}} of {{opTotalItem}}</div>',

        link: function (scope) {
            scope.pageCnt = 0;
            scope.pages = [];
            scope.preDisabled = "";
            scope.nextDisabled = "";
            scope.$watch("opTotalItem", function (value) {
                scope.pages =[];
                scope.pageCnt = parseInt((value + scope.opItemPerPage - 1) / scope.opItemPerPage);

                for (var i = 1; i <= scope.pageCnt; ++i) {
                    scope.pages.push(i);
                }
                scope.disablePreNext();
            });

            scope.clickPage = function(pno) {
                scope.opCurPage = pno;
                pageChange();
                scope.disablePreNext();
            };
            scope.clickPrev = function() {
                if (scope.opCurPage > 1) {
                    scope.opCurPage--;
                    pageChange();
                }
                scope.disablePreNext();
            };
            scope.clickNext = function() {
                if (scope.opCurPage < scope.pageCnt) {
                    scope.opCurPage++;
                    pageChange();
                }
                scope.disablePreNext();
            };
            scope.curActive = function(pno) {
                if (scope.opCurPage == pno) {
                    return "active";
                }
                return "";
            };
            scope.disablePreNext = function() {
                scope.preDisabled = scope.opCurPage == 1 ? "disabled" : "";
                scope.nextDisabled = scope.opCurPage == scope.pageCnt ? "disabled" : "";
            };

            function pageChange() {
                if (scope.opPageChange) {
                    $timeout(scope.opPageChange);
                }
            }
        }
    };
}]);