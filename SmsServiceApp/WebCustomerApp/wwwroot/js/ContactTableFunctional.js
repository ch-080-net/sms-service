var delegateToCurrentGetListFunction = function () { getContactList() }
var delegateToCurrentCountFunction = function () { getContactCount(); }
var phonesCount;
var currentPage = 1;
var searchValue = "";

$(document).ready(GetContactData());

function GetPhoneData() {
    delegateToCurrentCountFunction();
}

var Phone = {
    id: 0,
    PhoneNumber: "",
    Name: "",
    Surname: "",
    BirthDate: "",
    Gender: "",
    Priority: "",
    Notes: "",
    KeyWords: ""
}

function getContactCount() {
    $.ajax({
        url: '/Contact/GetPhoneCount/',
        type: 'GET',
        data: { searchValue: searchValue },
        success: function (count) {
            phonesCount = count;
            delegateToCurrentGetListFunction();
            buildNavigationButtons();
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function nextPhonePage() {
    if (phonesCount % 10 == 0) {
        if (currentPage < phonesCount / 10) {
            currentPage++;
            delegateToCurrentGetListFunction();
        }
    }
    else
        if (currentPage < (phonesCount / 10) + 1) {
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

function getPhonePageByNumber(item) {
    var numberOfPage = item.id;
    numberOfPage = numberOfPage.substr(4);
    currentPage = parseInt(numberOfPage, 10);
    delegateToCurrentGetListFunction();
}

// Get all Phones to display
function getPhoneList() {
    // Call Web API to get a list of Phones
    $.ajax({
        url: '/Phone/GetPhoneList/',
        type: 'GET',
        data: {
            numberOfPage: currentPage
        },
        success: function (phones) {
            phoneListSuccess(phones);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

// Display Phones returned from Web API call
function phoneListSuccess(phones) {
    // Iterate over the collection of data
    $("#phoneTable tbody").remove();
    $.each(phones, function (index, phone) {
        // Add a row to the phone table
        phoneAddRow(phone);
    });
}

// Add phone row to <table>
function phoneAddRow(phone) {
    // First check if a <tbody> tag exists, add one if not
    if ($("#phoneTable tbody").length == 0) {
        $("#phoneTable").append("<tbody></tbody>");
    }

    // Append row to <table>
    $("#phoneTable tbody").append(
        phoneBuildTableRow(phone));
}

// Build a <tr> for a row of table data
function phoneBuildTableRow(phone) {
    var newRow = "<tr>" +
        "<td><input  class='input-phone' type='text' readonly='true' value='" + phone.phoneNumber + "'/></td>" +
        "<td><input  class='input-fullname'  type='text' readonly='false' value='" + phone.fullName + "'/></td>" +
        "<td>" +
        "<button type='button' " +
        "onclick='phoneEditAllow(this);' " +
        "class='btn btn-default' " +
        "data-id='" + phone.phoneId + "' " +
        "data-phonenumber='" + phone.phoneNumber + "' " +
        "data-fullname='" + phone.fullName + "' " +
        ">" +
        "<span class='glyphicon glyphicon-edit' /> Update" +
        "</button> " +
        " <button type='button' " +
        "onclick='phoneDelete(this);'" +
        "class='btn btn-default' " +
        "data-id='" + phone.phoneId + "'>" +
        "<span class='glyphicon glyphicon-remove' />Delete" +
        "</button>" +
        "</td>" +
        "</tr>";

    return newRow;
}

function onAddPhone(item) {
    var options = {};
    options.url = "/Phone/AddPhone";
    options.type = "POST";
    var obj = Phone;
    obj.PhoneNumber = $("#phonenumber").val();
    obj.FullName = $("#fullname").val();
    console.dir(obj);
    options.data = obj;

    options.success = function (msg) {
        $("#msg").html(msg);
        GetPhoneData();
    },
        options.error = function () {
            $("#msg").html("Error while calling the Web API!");
        };
    $.ajax(options);
    $("#phonenumber").val("");
    $("#fullname").val("");
}

function phoneEditAllow(item) {
    item.removeChild(item.firstChild);
    item.textContent = "";
    $(item).append("<span class='glyphicon glyphicon-floppy-disk' /> Save");
    $(".input-phone", $(item).parent().parent())[0].readOnly = false;
    $(".input-fullname", $(item).parent().parent())[0].readOnly = false;
    item.setAttribute("onclick", "phoneUpdate(this)");

}

function phoneUpdate(item) {
    var id = $(item).data("id");
    var options = {};
    options.url = "/Phone/UpdatePhone/"
    options.type = "PUT";

    var obj = Phone;
    obj.id = $(item).data("id");
    obj.PhoneNumber = $(".input-phone", $(item).parent().parent()).val();
    obj.FullName = $(".input-fullname", $(item).parent().parent()).val();
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
    $(".input-phone", $(item).parent().parent())[0].readOnly = true;
    $(".input-fullname", $(item).parent().parent())[0].readOnly = true;
    item.setAttribute("onclick", "phoneEditAllow(this)");
}

function phoneDelete(item) {
    var id = $(item).data("id");
    var options = {};
    options.url = "/Phone/DeletePhone/"
        + id;
    options.type = "DELETE";
    options.success = function (msg) {
        console.log('msg= ' + msg);
        $("#msg").html(msg);
        if ((phonesCount - 1) % 10 == 0)
            currentPage--;
        GetPhoneData();
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

function getSearchPhones() {
    $.ajax({
        url: '/Phone/Search/',
        type: 'GET',
        data: {
            searchData: searchValue,
            numberOfPage: currentPage
        },
        success: function (phones) {
            phoneListSuccess(phones);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function getNumberOfSearchPhones() {
    $.ajax({
        url: '/Phone/GetNumberOfSearchPhones/',
        type: 'GET',
        data: {
            searchData: searchValue,
        },
        success: function (count) {
            phonesCount = count;
            delegateToCurrentGetListFunction();
            buildNavigationButtons();
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function searchPhones() {
    searchValue = $("#searchField").val();
    if (searchValue == "") {
        delegateToCurrentCountFunction = function () { getPhonesCount(); }
        delegateToCurrentGetListFunction = function () { getPhoneList(); }
        GetPhoneData();
        return;
    }
    delegateToCurrentCountFunction = function () { getNumberOfSearchPhones(); }
    delegateToCurrentGetListFunction = function () { getSearchPhones(); }
    currentPage = 1;
    GetPhoneData();
}

function buildNavigationButtons() {
    if (phonesCount % 10 == 0)
        pagesCount = phonesCount / 10;
    else
        var pagesCount = phonesCount / 10 + 1;
    $("#pageButtons button").remove();
    var button = "<button type='button' class='btn btn -default ' onclick='previousPhonePage()' id='previousPage'><span class='glyphicon glyphicon-triangle-left' /></button>"
    $("#pageButtons").append(button);
    for (var i = 1; i <= pagesCount; i++) {
        button = "<button type='button' class='btn btn -default' onclick='getPhonePageByNumber(this)' id='Page" + i + "'>" + i + "</button>";
        $("#pageButtons").append(button);
    }
    button = "<button type='button' class='btn btn -default ' onclick='nextPhonePage()' id='nextPage'><span class='glyphicon glyphicon-triangle-right' /></button>";
    $("#pageButtons").append(button);
}