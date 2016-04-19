angular.module("opCommonModule")
  .directive('opFileuploader', function () {

      function link(scope, iElement, iAttrs, controller, transcludeFn) {

          var inputBox = iElement.siblings('div').first().find('input').first();
          var browserButton = inputBox.next();
          var anchor = iElement.siblings('div').first();
          var inputFileType = iElement;
          var template = '<input type="file" style="display:none" op-fileuploader>';

          var reregisiter = function () {
              inputBox.off('click focus', handleEvent);
              browserButton.off('click focus', handleEvent);
              inputFileType.remove();

              anchor.before(template);
              inputFileType = anchor.prev();
              inputFileType.on('change', handleFileSelect);
              inputBox.on('click focus', { element: inputFileType }, handleEvent);
              browserButton.on('click focus', { element: inputFileType }, handleEvent);
          }

          var handleFileSelect = function (event) {
              if (event.target.files[0]) {

                  var file = event.target.files[0];

                  scope.setfilename(file.name);

                  var reader = new FileReader();

                  reader.onload = function (e) {
                      scope.setfilecontent(e.target.result);
                      scope.disableUploadFlag(false);
                      scope.cleanErrorMessage();
                      reregisiter();
                  };

                  reader.onerror = function (e) {
                      scope.setErrorMessage(["Failed to read the file."]);
                      scope.disableUploadFlag(true);
                      scope.cleanErrorMessage();
                      reregisiter();
                  };

                  reader.readAsDataURL(file); 
              };
          };

          var handleEvent = function (event) {
              if (event && event.data && event.data.element) {
                  event.data.element.click();
              };
              event.preventDefault();
          };

          if (window.File && window.FileReader && window.FileList && window.Blob) {

              inputFileType.on('change', handleFileSelect);
              inputBox.on('click focus', { element: inputFileType }, handleEvent);
              browserButton.on('click focus', { element: inputFileType }, handleEvent);

          } else {
              scope.setErrorMessage(['The File APIs are not fully supported in this browser.']);
              return;
          };
      }

      return {
          restrict: 'A',
          link: link
      };
  });