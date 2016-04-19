
requirejs.config({
    //To get timely, correct error triggers in IE, force a define/shim exports
    // check.
    enforceDefine: true,
    paths: {
        jquery: [
            '//ajax.aspnetcdn.com/ajax/jquery/jquery-1.9.0', //Cdn
            'Scripts/jquery-1.9.0' // your fallback
        ]
    }
});