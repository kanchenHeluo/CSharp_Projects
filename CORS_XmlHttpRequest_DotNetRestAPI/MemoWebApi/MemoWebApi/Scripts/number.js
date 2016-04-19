var xmlHttp;
function createXMLHttpRequest() {
    if (window.ActiveXObject) {
        xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    else if (window.XMLHttpRequest) {
        xmlHttp = new XMLHttpRequest();
    }
}
//Get
function AddNumber() {
    createXMLHttpRequest();
    var url = "POC/Add/" + document.getElementById("addNum1").value + "/" + document.getElementById("addNum2").value;
    xmlHttp.open("GET", url, true);
    xmlHttp.onreadystatechange = ShowAddResult;
    xmlHttp.send(null);
}
function ShowAddResult() {
    if (xmlHttp.readyState == 4) {
        if (xmlHttp.status = 200) {
            document.getElementById("addSum").value = xmlHttp.responseText;
        }
    }
}

//Post
function MultipleNumber() {
    createXMLHttpRequest();
    var url = "POC/Multiple";
    var params = JSON.stringify({ Num1: document.getElementById("multipleNum1").value, Num2: document.getElementById("multipleNum2").value });
    xmlHttp.open("POST", url, true);

    xmlHttp.setRequestHeader("Content-type", "application/json; charset=utf-8");

    xmlHttp.onreadystatechange = ShowMultipleResult;
    xmlHttp.send(params);
}

function ShowMultipleResult() {
    if (xmlHttp.readyState == 4) {
        if (xmlHttp.status = 200) {
            document.getElementById("multipleSum").value = xmlHttp.responseText;
        }
    }
}
