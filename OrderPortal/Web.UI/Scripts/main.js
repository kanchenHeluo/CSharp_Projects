
$(document).ready(function () {
    console.log("jQuery Loaded");
    $("#divlogintile").on("click", function () {
        window.location.href = $(this).attr("data-url");

    });

    $("#divwindowslivetile").on("click", function () {
        window.location.href = $(this).attr("data-url");

    });
    $(".userinfo").click(function () {
        $(".useroptions").show();
    });
    $(".userinfo").mouseleave(function () {
        $(".useroptions").hide();
    });

});

//to be compatible with toast message which refers to String.format (maybe from old asp.net)
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g,
        function (m, i) {
            return args[i];
        });
}
String.format = function () {
    if (arguments.length == 0)
        return null;

    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
}