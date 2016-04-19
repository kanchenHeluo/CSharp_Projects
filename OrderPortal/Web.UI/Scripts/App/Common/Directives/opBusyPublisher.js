angular.module('opCommonModule')
  .directive('opBusyPublisher', ['$rootScope', function ($rootScope) {

      function link(scope, element, attrs) {

          var startEvent = (attrs.opBusyPublisherEvent + 'Start').toLowerCase();
          var endEvent = (attrs.opBusyPublisherEvent + 'End').toLowerCase();

          var handler = function (newPromise, oldPromise) {

              if (!newPromise || newPromise.opBusyFulfilled) {
                  return;
              };

              $rootScope.$broadcast(startEvent);

              var then = newPromise.then;

              var success = function() {
                  newPromise.opBusyFulfilled = true;
                  $rootScope.$broadcast(endEvent);
              };

              var error = function () {
                  newPromise.opBusyFulfilled = true;
                  $rootScope.$broadcast(endEvent);
              };

              then.call(newPromise, success, error);
          };

          scope.$watchCollection(attrs.opBusyPublisher, handler);
      };

      return {
          restrict: 'A',
          link: link
      };
  }]);