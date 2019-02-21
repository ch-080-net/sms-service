var delegateToCurrentGetListFunction = function () { getOperatorsList() }
var delegateToCurrentCountFunction = function () { getOperatorsCount(); }
var operatorsCount;
var currentPage = 1;
var searchValue = "";

$(document).ready(GetOperatorData());

function GetOperatorData() {
    delegateToCurrentCountFunction();
}

var Operator = {
    id: 0,
    Name: "",
    Logo: ""
}

function getOperatorsCount() {
    $.ajax({
        url: '/Operator/GetOperatorsCount/',
        type: 'GET',
        success: function (count) {
            operatorsCount = count;
            delegateToCurrentGetListFunction();
            buildNavigationButtons();
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function nextOperatorPage() {
    if (operatorsCount % 10 == 0) {
        if (currentPage < operatorsCount / 10) {
            currentPage++;
            delegateToCurrentGetListFunction();
        }
    }
    else
        if (currentPage < (operatorsCount / 10) + 1) {
            currentPage++;
            delegateToCurrentGetListFunction();
        }
}

function previousPhonePage() {
    if (currentPage > 1) {
        currentPage--;
        delegateToCurrentGetListFunction();
    }
}

function getOperatorPageByNumber(item) {
    var numberOfPage = item.id;
    numberOfPage = numberOfPage.substr(4);
    currentPage = parseInt(numberOfPage, 10);
    delegateToCurrentGetListFunction();
}

// Get all Operators to display
function getOperatorsList() {
    // Call Web API to get a list of Phones
    $.ajax({
        url: '/Operator/GetOperatorsList/',
        type: 'GET',
        data: {
            numberOfPage: currentPage
        },
        success: function (operators) {
            operatorListSuccess(operators);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

// Display Operators returned from Web API call
function operatorListSuccess(operators) {
    // Iterate over the collection of data
    $("#operatorTable tbody").remove();
    $.each(operators, function (index, operator) {
        // Add a row to the phone table
        operatorAddRow(operator);
    });
}

// Add phone row to <table>
function operatorAddRow(operator) {
    // First check if a <tbody> tag exists, add one if not
    if ($("#operatorTable tbody").length == 0) {
        $("#operatorTable").append("<tbody></tbody>");
    }

    // Append row to <table>
    $("#operatorTable tbody").append(
        operatorBuildTableRow(operator));
}

// Build a <tr> for a row of table data
function operatorBuildTableRow(operator) {
    var newRow = "<tr>" +
        "<td><input  class='input-phone' type='text' readonly='true' value='" + operator.phoneNumber + "'/></td>" +
        "<td><input  class='input-fullname'  type='text' readonly='false' value='" + operator.fullName + "'/></td>" +
        "<td>" +
        "<button type='button' " +
        "onclick='phoneEditAllow(this);' " +
        "class='btn btn-default' " +
        "data-id='" + operator.phoneId + "' " +
        "data-phonenumber='" + operator.phoneNumber + "' " +
        "data-fullname='" + operator.fullName + "' " +
        ">" +
        "<span class='glyphicon glyphicon-edit' /> Update" +
        "</button> " +
        " <button type='button' " +
        "onclick='phoneDelete(this);'" +
        "class='btn btn-default' " +
        "data-id='" + operator.phoneId + "'>" +
        "<span class='glyphicon glyphicon-remove' />Delete" +
        "</button>" +
        "</td>" +
        "</tr>";

    return newRow;
}

function onAddOperator(item) {
    var options = {};
    options.url = "/Operator/AddOperator";
    options.type = "POST";
    var obj = Operator;
    obj.Name = $("#name").val();
    console.dir(obj);
    options.data = obj;

    options.success = function (msg) {
        $("#msg").html(msg);
        GetOperatorData();
    },
        options.error = function () {
            $("#msg").html("Error while calling the Web API!");
        };
    $.ajax(options);
    $("#name").val("");
}

function operatorEditAllow(item) {
    item.removeChild(item.firstChild);
    item.textContent = "";
    $(item).append("<span class='glyphicon glyphicon-floppy-disk' /> Save");
    $(".input-name", $(item).parent().parent())[0].readOnly = false;
    item.setAttribute("onclick", "operatorUpdate(this)");

}

function operatorUpdate(item) {
    var id = $(item).data("id");
    var options = {};
    options.url = "/Operator/UpdateOperator/"
    options.type = "PUT";

    var obj = Operator;
    obj.id = $(item).data("id");
    obj.Name = $(".input-name", $(item).parent().parent()).val();
    console.dir(obj);
    options.data = obj;
    options.success = function (msg) {
        $("#msg").html(msg);
    };
    options.error = function () {
        $("#msg").html("Error while calling the Web API!");
    };
    $.ajax(options);
    item.removeChild(item.firstChild);
    item.textContent = "";
    $(item).append("<span class='glyphicon glyphicon-edit' /> Update");
    $(".input-operator", $(item).parent().parent())[0].readOnly = true;
    item.setAttribute("onclick", "operatorEditAllow(this)");
}

function operatorDelete(item) {
    var id = $(item).data("id");
    var options = {};
    options.url = "/Phone/DeleteOperator/"
        + id;
    options.type = "DELETE";
    options.success = function (msg) {
        console.log('msg= ' + msg);
        $("#msg").html(msg);
        if ((operatorsCount - 1) % 10 == 0)
            currentPage--;
        GetOperatorData();
    };
    options.error = function () {
        $("#msg").html("Error while calling the Web API!");
    };
    $.ajax(options);
}

function handleException(request, message, error) {
    var msg = "";
    msg += "Code: " + request.status + "\n";
    msg += "Text: " + request.statusText + "\n";
    if (request != null) {
        msg += "Message" + request.Message + "\n";
    }

    alert(msg);
}

function getSearchOperators() {
    $.ajax({
        url: '/Operator/Search/',
        type: 'GET',
        data: {
            searchData: searchValue,
            numberOfPage: currentPage
        },
        success: function (operator) {
            operatorListSuccess(operator);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function getNumberOfSearchOperators() {
    $.ajax({
        url: '/Operator/GetNumberOfSearchOperators/',
        type: 'GET',
        data: {
            searchData: searchValue,
        },
        success: function (count) {
            operatorsCount = count;
            delegateToCurrentGetListFunction();
            buildNavigationButtons();
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function searchOperators() {
    searchValue = $("#searchField").val();
    if (searchValue == "") {
        delegateToCurrentCountFunction = function () { getOperatorsCount(); }
        delegateToCurrentGetListFunction = function () { getOperatorsList(); }
        GetOperatorData();
        return;
    }
    delegateToCurrentCountFunction = function () { getNumberOfSearchOperators(); }
    delegateToCurrentGetListFunction = function () { getSearchOperators(); }
    currentPage = 1;
    GetOperatorData();
}

function buildNavigationButtons() {
    if (operatorsCount % 10 == 0)
        pagesCount = operatorsCount / 10;
    else
        var pagesCount = operatorsCount / 10 + 1;
    $("#pageButtons button").remove();
    var button = "<button type='button' class='btn btn -default ' onclick='previousOperatorPage()' id='previousPage'><span class='glyphicon glyphicon-triangle-left' /></button>"
    $("#pageButtons").append(button);
    for (var i = 1; i <= pagesCount; i++) {
        button = "<button type='button' class='btn btn -default' onclick='getOperatorPageByNumber(this)' id='Page" + i + "'>" + i + "</button>";
        $("#pageButtons").append(button);
    }
    button = "<button type='button' class='btn btn -default ' onclick='nextOperatorPage()' id='nextPage'><span class='glyphicon glyphicon-triangle-right' /></button>";
    $("#pageButtons").append(button);
}