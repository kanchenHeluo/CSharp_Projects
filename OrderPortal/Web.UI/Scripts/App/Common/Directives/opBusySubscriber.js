angular.module('opCommonModule')
  .directive('opBusySubscriber', ['$compile', function ($compile) {

      function link(scope, element, attrs) {

          var templateScope = scope.$new();

          var position = element.css('position');
          if (position === 'static' || !position){
              element.css('position', 'relative');
          };

          var backdrop = '<div class="op-busy op-busy-backdrop ng-hide" ng-show="showFlag"></div>' +
                            '<div class="op-busy ng-hide" ng-show="showFlag"><img src="../../Content/Images/Spinner.gif"/ style="position:absolute"></div>';
          var backdropElement = $compile(backdrop)(templateScope);
          var spinner = backdropElement.children('img')[0];

          var zIndex = element.css('z-index') === 'auto' ? 10 : element.css('z-index') + 10;

          angular.forEach(backdropElement, function(item) {
              $(item).css('z-index', zIndex);
          });

          element.append(backdropElement);

          var counter = 0;

          var addSpinner = function () {
              counter++;
              templateScope.showFlag = true;
              angular.element(spinner)
                  .css('top', (element.height() - 40) / 2)  //40 is the height of Spinner.gif
                  .css('left', (element.width() - 40) / 2); //40 is the width of Spinner.gif
          };

          var removeSpinner = function () {
              if (--counter == 0) {
                  templateScope.showFlag = false;
              };
          };

          if (attrs.opBusySubscriber) {
              angular.forEach(attrs.opBusySubscriber.split(' '), function (event) {
                  eventStart = (event + 'Start').toLowerCase();
                  eventEnd = (event + 'End').toLowerCase();
                  templateScope.$on(eventStart, addSpinner);
                  templateScope.$on(eventEnd, removeSpinner);
              });
          };
      };

      return {
          restrict: 'A',
          link: link
      };
  }]);