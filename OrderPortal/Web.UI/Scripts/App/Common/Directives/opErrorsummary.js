angular.module("opCommonModule")
  .directive('opErrorsummary', ['$compile', function ($compile) {
      function link(scope, element, attrs) {

          var templateScope = scope.$new();
          var template = '';

          if (attrs['messageOnly'] != undefined) {
              template =
                  '<div class="validation-summary-errors" id="valSumId" data-valmsg-summary="true" ng-if="messageSource">' +
                  '<span>{{messageSource}}</span>' +
                  '</div>';
          } else if (attrs['messageSource']) {
              template =
                '<div class="validation-summary-errors" id="valSumId" data-valmsg-summary="true" ng-if="messageSource && messageSource.length > 0">' +
                '<span>{{messageTitle}}:</span>' +
                '<ul>' +
                '<li ng-repeat="message in messageSource track by $index">{{message}}</li>' +
                '</ul>' +
                '</div>';
          } else {
              var parentFormCtrl = element.parent().controller('op-form');

              if (parentFormCtrl.errorMessages && parentFormCtrl.alias) {
                  templateScope.errorMessages = parentFormCtrl.errorMessages;
                  templateScope.alias = parentFormCtrl.alias;
              };

              template =
                '<div class="validation-summary-errors" id="valSumId" data-valmsg-summary="true" ng-if="errorMessages && alias && errorMessages[alias].length > 0">' +
                '<span>{{messageTitle}}:</span>' +
                '<ul>' +
                '<li ng-repeat="message in errorMessages[alias] track by $index">{{message}}</li>' +
                '</ul>' +
                '</div>';
          };

          var compliedElement = $compile(template)(templateScope);
          element.append(compliedElement);
      }
      return {
          restrict: 'E',
          scope: {
              messageTitle : '@',
              messageSource: '='
          },
          link: link
      };
  }]);