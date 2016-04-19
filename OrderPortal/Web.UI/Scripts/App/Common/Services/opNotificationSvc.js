angular.module("opCommonModule")
.factory("opNotificationSvc", function () {

    var baseStatus = {
        isProcessing: false,
        message: '',
        error: ''
    };
    var rootUrl = '/';

    function getRootUrl() {
        return rootUrl;
    };

    function message(msg) {
        baseStatus.message = msg;
        if (msg && window.toastMessage) {
            window.toastMessage.Show({ Color: 'Green', Tilte: 'Success', Message: msg });
        }
    };

    function error(err) {
        baseStatus.error = error;
        if (err && window.toastMessage) {
            window.toastMessage.Show({ Color: 'Yellow', Tilte: 'Error', Message: err });
        }
    };

    function inlineMessage(msg, element) {
        var t = new MiniToastObject({ message: msg, appendto: element });
    }

    function inlineError(err, element) {
        var t = new MiniToastObject({ message: err, appendto: element });
    }

    return {
        status: baseStatus,
        message: message,
        error: error,
        inlineMessage: inlineMessage,
        inlineError: inlineError
    };
});