angular.module("opCommonModule")
  .directive('opPanelcontainer', function () {

      function link(scope, iElement, iAttrs, controller, transcludeFn) {

          var placeholderleft = iElement.find('.placeholder-left');
          var replacementleft = iElement.find('.replacement-left').contents();
          if (replacementleft.length == 0) {
              replacementleft = $('<div style="display:inline-block"><span class="panel-title ms-sitetitle layout-panel-heading-left">' + scope.name + '</span></div>');
          } 
          placeholderleft.before(replacementleft);
          placeholderleft.remove();

          var placeholderright = iElement.find('.placeholder-right');
          var replacementright = iElement.find('.replacement-right').contents();

          var dropdownList = iElement.find('.replacement-right .dropdown-menu');
          if (dropdownList.length != 0) {
              placeholderright.after('<span class="more symbol" data-toggle="dropdown"></span>');
          };

          placeholderright.before(replacementright);
          placeholderright.remove();

          function collapse(s, e) {
              var $s = this;
              $s.scope = s;
              $s.element = e;

              $s.panelHeading = $s.element.children("div").children("div").first();
              $s.source = $s.element.children("div").children("div").first().find("span").last();
              $s.target = $s.source.parent().parent().parent("div[class*='panel-heading']").next("div[class*='panel-collapse']");

              //HTML Element defination
              $s.heightElement = 'height';

              //Style constant defination
              $s.collapseStyle = 'collapse';
              $s.arrowDownStyle = 'downChevron symbol';
              $s.arrowUpStyle = 'upChevron symbol';
              $s.paneleadingDisableStyel = 'panel-heading-disable';
              $s.expendStyle = 'in';
              $s.collapsingStyle = 'collapsing';

              //Event constant defination
              $s.hideCompletedEvent = 'hideCompleted';
              $s.expandCompletedEvent = 'expandCompleted';
              $s.disabledCompletedEvent = 'disabledCompleted';
              $s.enabledCompletedEvent = 'enabledCompleted';

              //Panel status defination
              $s.disableStatusd = 'Disabled';
              $s.collapseStatus = 'Collapse';
              $s.expandStatus = 'Expand';

              //The first dimension stand for XXXIsOpen flag in the controler.
              //Second dimension stand for XXXIsDisable flag in the controler.
              $s.statusAray = new Array();
              $s.statusAray[0] = new Array($s.collapseStatus, $s.expandStatus);
              $s.statusAray[1] = new Array($s.disableStatusd, $s.disableStatusd);

              $s.transitioning = 0;
          };

          //Brower compatibility for css3 transit end event
          collapse.prototype.transitionendevent = (function () {
              var el = document.createElement('orderTemp');

              var transEndEventNames = {
                  'WebkitTransition': 'webkitTransitionEnd',
                  'MozTransition': 'transitionend',
                  'OTransition': 'oTransitionEnd otransitionend',
                  'transition': 'transitionend'
              }

              for (var name in transEndEventNames) {
                  if (el.style[name] !== undefined) {
                      return { end: transEndEventNames[name] }
                  }
              }
          })();

          //trigger transitionendevent manually in case it couldn't triggered for some reason.
          collapse.prototype.emulateTransitionEnd = function(element, duration) {
              var $s = this;
              var called = false;
              $(element).one($s.transitionendevent.end, function() {
                   called = true;
              });
              var callback = function() {
                  if (!called) $(element).trigger($s.transitionendevent.end);
              };
              setTimeout(callback, duration);
          };

          collapse.prototype.checkStatus = function () {
              var $s = this;
              if ($s.scope.isDisabled) {
                  $s.x = 1;
              } else {
                  $s.x = 0; };
              if ($s.scope.isOpen) {
                  $s.y = 1;
              } else {
                  $s.y = 0; }
              $s.newTargetStatus = $s.statusAray[$s.x][$s.y];
              if (!$s.transitioning && $s.currentPanelStatus && $s.currentPanelStatus != $s.newTargetStatus) {
                  var functionName = $s.currentPanelStatus + "To" + $s.newTargetStatus;
                  var fn = $s[functionName];
                  if (typeof fn === 'function') fn.apply($s);
              } else if (!$s.currentPanelStatus) {
                  $s.initPanel();
              }
          };

          collapse.prototype.initPanel = function () {
              var $s = this;
              switch ($s.newTargetStatus) {
                  case $s.disableStatusd: {
                      $s.scope.initArrow = '';
                      $s.scope.initPanelStatus = $s.collapseStyle;
                      $s.scope.initHeadingStyle = $s.paneleadingDisableStyel;
                      $s.scope.isOpen = false;
                      $s.scope.isDisabled = true;
                  } break;
                  case $s.collapseStatus: {
                      $s.scope.initArrow = $s.arrowDownStyle;
                      $s.scope.initPanelStatus = $s.collapseStyle;
                      $s.scope.isOpen = false;
                      $s.scope.isDisabled = false;
                  } break;
                  case $s.expandStatus: {
                      $s.scope.initArrow = $s.arrowUpStyle;
                      $s.scope.initPanelStatus = $s.expendStyle;
                      $s.scope.isOpen = true;
                      $s.scope.isDisabled = false;
                  } break;
                  default:;
              };
              $s.currentPanelStatus = $s.newTargetStatus;
          };

          collapse.prototype.transitionEnd = function () {
              var $s = this;
              $s.transitioning = 0;
              $s.currentPanelStatus = $s.newTargetStatus;
              $s.checkStatus();
          };

          collapse.prototype.ExpandToCollapse = function () {
              var $s = this;
              if ($s.transitioning) return;
              $s.transitioning = 1;
              $s.source.one($s.hideCompletedEvent, $.proxy($s.transitionEnd, $s));
              $s.hide();
          };

          collapse.prototype.CollapseToExpand = function () {
              var $s = this;
              if ($s.transitioning) return;
              $s.transitioning = 1;
              $s.source.one($s.expandCompletedEvent, $.proxy($s.transitionEnd, $s));
              $s.expand();
          };

          collapse.prototype.ExpandToDisabled = function () {
              var $s = this;
              if ($s.transitioning) return;
              $s.transitioning = 1;
              $s.source.one($s.hideCompletedEvent, $.proxy($s.disabled, $s));
              $s.source.one($s.disabledCompletedEvent, $.proxy($s.transitionEnd, $s));
              $s.hide();
          };

          collapse.prototype.DisabledToExpand = function () {
              var $s = this;
              if ($s.transitioning) return;
              $s.transitioning = 1;
              $s.source.one($s.enabledCompletedEvent, $.proxy($s.expand, $s));
              $s.source.one($s.expandCompletedEvent, $.proxy($s.transitionEnd, $s));
              $s.enable();
          };

          collapse.prototype.CollapseToDisabled = function () {
              var $s = this;
              if ($s.transitioning) return;
              $s.transitioning = 1;
              $s.source.one($s.disabledCompletedEvent, $.proxy($s.transitionEnd, $s));
              $s.disabled();
          };

          collapse.prototype.DisabledToCollapse = function () {
              var $s = this;
              if ($s.transitioning) return;
              $s.transitioning = 1;
              $s.source.one($s.enabledCompletedEvent, $.proxy($s.transitionEnd, $s));
              $s.enable();
          };

          collapse.prototype.enable = function () {
              var $s = this;
              if (!$s.panelHeading.hasClass($s.paneleadingDisableStyel) || $s.source.hasClass($s.arrowDownStyle)) {
                  console.log('Error status from scope.enable function.');
                  $s.transitioning = 0;
                  return;
              };
              $s.panelHeading.removeClass($s.paneleadingDisableStyel);
              $s.source.addClass($s.arrowDownStyle);
              $s.source.trigger($s.enabledCompletedEvent);
          };

          collapse.prototype.disabled = function () {
              var $s = this;
              if (!$s.source.hasClass($s.arrowDownStyle) || $s.panelHeading.hasClass($s.paneleadingDisableStyel)) {
                  console.log('Error status from scope.disabled function.');
                  $s.transitioning = 0;
                  return;
              };
              $s.source.removeClass($s.arrowDownStyle);
              $s.panelHeading.addClass($s.paneleadingDisableStyel);
              $s.source.trigger($s.disabledCompletedEvent);
          };

          collapse.prototype.expand = function () {
              var $s = this;
              if (!$s.target.hasClass($s.collapseStyle) || $s.target.hasClass($s.collapsingStyle)) {
                  console.log('Error status from scope.expand function.');
                  $s.transitioning = 0;
                  return;
              };
              $s.target.removeClass($s.collapseStyle).addClass($s.collapsingStyle)[$s.heightElement](0);
              var scrollSize = $.camelCase(['scroll', $s.heightElement].join('-'));
              $s.target.one($s.transitionendevent.end, $.proxy($s.expandStep2, $s));
              $s.target[$s.heightElement]($s.target[0][scrollSize]);
              $s.emulateTransitionEnd($s.target, 350);
          };

          collapse.prototype.expandStep2 = function () {
              var $s = this;
              $s.target.removeClass($s.collapsingStyle).addClass($s.collapseStyle).addClass($s.expendStyle)[$s.heightElement]('auto');
              $s.source.removeClass($s.arrowDownStyle).addClass($s.arrowUpStyle);
              $s.source.trigger($s.expandCompletedEvent);
          };

          collapse.prototype.hide = function () {
              var $s = this;
              if ($s.target.hasClass($s.collapsingStyle) || !$s.target.hasClass($s.expendStyle)) {
                  console.log('Error status from scope.hide function.');
                  $s.transitioning = 0;
                  return;
              };
              $s.target[$s.heightElement]($s.target[$s.heightElement]())[0].offsetHeight;
              $s.target.addClass($s.collapsingStyle).removeClass($s.collapseStyle).removeClass($s.expendStyle);
              $s.target.one($s.transitionendevent.end, $.proxy($s.hideStep2, $s));
              $s.target[$s.heightElement](0);
              $s.emulateTransitionEnd($s.target, 350);
          };

          collapse.prototype.hideStep2 = function () {
              var $s = this;
              $s.target.removeClass($s.collapsingStyle).addClass($s.collapseStyle);
              $s.source.removeClass($s.arrowUpStyle).addClass($s.arrowDownStyle);
              $s.source.trigger($s.hideCompletedEvent);
          };

          var data = new collapse(scope, iElement);

          scope.$watch('isDisabled', function () {
              if (!data.transitioning) {
                  data.checkStatus();
              };
          });

          scope.$watch('isOpen', function () {
              if (!data.transitioning) {
                  data.checkStatus();
              };
          });

          scope.toggle = function () {
              scope.isOpen = !scope.isOpen;
          };
      }

      return {
          restrict: 'E',
          transclude: true,
          scope: {
              name: '@',
              isOpen: '=',
              isDisabled: '='
          },
          template:
               '<div class="panel panel-default">' +
                    '<div class="panel-heading layout-positionRel {{initHeadingStyle}}">' +
                        '<span class="placeholder-left"></span>' +
                        '<div class="layout-inlineBlockOnly layout-positionAbs" style="bottom: 7px;">' +
                            '<div class="layout-positionRel layout-inlineBlockOnly" >' +
                                '<span class="placeholder-right"></span>' +
                            '</div>' +
                            '<div class="layout-inlineBlockOnly">' +
                                '<span class="{{initArrow}}" style="vertical-align: bottom;" ng-click="toggle()"/>' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                    '<div class="panel-collapse {{initPanelStatus}}">' +
                        '<div class="panel-body">' +
                            '<div ng-transclude></div>' +
                        '</div>' +
                    '</div>' +
                '</div>',
          link: link
      };
  });