angular.module('opCommonModule')
    .factory('localStorage', function () {        
       var save = function (name, data) {
        amplify.store(name, data);
    };

    var retrieve = function (name) {
        return amplify.store(name);
    };

    var clearStore = function (name) {
        return amplify.store(name, null);
    };

    return {
        save: save,
        retrieve: retrieve,
        clearStore: clearStore
    }
});
