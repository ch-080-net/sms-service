var contactCount;
var currentPage = 1;
var pageSize = 5;
var searchValue = "";

$(document).ready(GetContactData());

function GetContactData() {
    getContactCount();
}

var Contact = {
    Id: 0,
    PhonePhoneNumber: "",
    Name: "",
    Surname: "",
    BirthDate: "",
    Gender: "",
    Notes: "",
    KeyWords: ""
}

function getContactCount() {
    $.ajax({
        url: '/Contact/GetContactCount/',
        type: 'GET',
        data: { searchValue: searchValue },
        success: function (count) {
            contactCount = count;
            getContactList();
            buildNavigationButtons();
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function nextPage() {
    if (contactCount % pageSize == 0) {
        if (currentPage < contactCount / pageSize) {
            currentPage++;
            getContactList()
        }
    }
    else
        if (currentPage < (contactCount / pageSize) + 1) {
            currentPage++;
            getContactList();
        }
}

function previousPage() {
    if (currentPage > 1) {
        currentPage--;
        getContactList();
    }
}

function getPageByNumber(item) {
    var numberOfPage = item.id;
    numberOfPage = numberOfPage.substr(4);
    currentPage = parseInt(numberOfPage, 10);
    getContactList();
}

// Get all Phones to display
function getContactList() {
    // Call Web API to get a list of Phones
    $.ajax({
        url: '/Contact/GetContactList/',
        type: 'GET',
        data: {
            pageNumber: currentPage,
            pageSize: pageSize,
            searchValue: searchValue
        },
        success: function (contacts) {
            contactListSuccess(contacts);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

// Display Phones returned from Web API call
function contactListSuccess(contacts) {
    // Iterate over the collection of data
    $("#contactTable tbody").remove();
    $.each(contacts, function (index, contact) {
        // Add a row to the phone table
        contactAddRow(contact);
    });
}

// Add phone row to <table>
function contactAddRow(contact) {
    // First check if a <tbody> tag exists, add one if not
    if ($("#contactTable tbody").length == 0) {
        $("#contactTable").append("<tbody></tbody>");
    }

    // Append row to <table>
    $("#contactTable tbody").append(
        contactBuildTableRow(contact));
}

// Build a <tr> for a row of table data
function contactBuildTableRow(contact) {
    var newRow = "<tr>" +
        "<td><input  class='input-phone' type='text' readonly='true' value='" + contact.phonePhoneNumber + "'/></td>" +
        "<td><input  class='input-name'  type='text' readonly='true' value='" + contact.name + "'/></td>" +
        "<td><input  class='input-surname'  type='text' readonly='true' value='" + contact.surname + "'/></td>" +
        "<td><input  class='input-birthdate'  type='text' readonly='true' value='" + contact.birthDate + "'/></td>" +
        "<td><input  class='input-gender'  type='text' readonly='true' value='" + contact.gender + "'/></td>" +
        "<td><input  class='input-notes'  type='text' readonly='true' value='" + contact.notes + "'/></td>" +
        "<td><input  class='input-keywords'  type='text' readonly='true' value='" + contact.keyWords + "'/></td>" +
        "<td>" +
        "<button type='button' " +
        "onclick='phoneEditAllow(this);' " +
        "class='btn btn-default' " +
        "data-id='" + contact.id + "' " +
        "data-phonenumber='" + contact.phonePhoneNumber + "' " +
        "data-name='" + contact.name + "' " +
        "data-surname='" + contact.surname + "' " +
        "data-birthdate='" + contact.birthDate + "' " +
        "data-gender='" + contact.gender + "' " +
        "data-notes='" + contact.notes + "' " +
        "data-keywords='" + contact.keyWords + "' " +
        ">" +
        "<span class='glyphicon glyphicon-edit' /> Update" +
        "</button> " +
        " <button type='button' " +
        "onclick='phoneDelete(this);'" +
        "class='btn btn-default' " +
        "data-id='" + contact.id + "'>" +
        "<span class='glyphicon glyphicon-remove' />Delete" +
        "</button>" +
        "</td>" +
        "</tr>";

    return newRow;
}

function onAddContact(item) {
    var options = {};
    options.url = "/Contact/AddContact";
    options.type = "POST";
    var obj = Contact;
    obj.PhonePhoneNumber = $("#phoneNumber").val();
    obj.Name = $("#name").val();
    obj.Surname = $("#surname").val();
    obj.BirthDate = $("#birthDate").val();
    if (document.getElementById("genderMale").checked) { obj.Gender = "Male"; }
    if (document.getElementById("genderFemale").checked) { obj.Gender = "Female"; }
    obj.Notes = $("#notes").val();
    obj.KeyWords = $("#keywords").val();
    console.dir(obj);
    options.data = obj;

    options.success = function (msg) {
        $("#msg").html(msg);
        GetContactData();
    },
        options.error = function () {
            $("#msg").html("Error while calling the Web API!");
        };
    $.ajax(options);
    $("#phoneNumber").val("");
    $("#name").val("");
    $("#surname").val("");
    $("#birthDate").val("");
    document.getElementById("genderMale").checked = false;
    document.getElementById("genderFemale").checked = false;
    $("#notes").val("");
    $("#keywords").val("");
}

function phoneEditAllow(item) {
    item.removeChild(item.firstChild);
    item.textContent = "";
    $(item).append("<span class='glyphicon glyphicon-floppy-disk' /> Save");
    $(".input-phone", $(item).parent().parent())[0].readOnly = false;
    $(".input-name", $(item).parent().parent())[0].readOnly = false;
    $(".input-surname", $(item).parent().parent())[0].readOnly = false;
    $(".input-birthdate", $(item).parent().parent())[0].readOnly = false;
    $(".input-gender", $(item).parent().parent())[0].readOnly = false;
    $(".input-notes", $(item).parent().parent())[0].readOnly = false;
    $(".input-keywords", $(item).parent().parent())[0].readOnly = false;
    item.setAttribute("onclick", "contactUpdate(this)");

}

function contactUpdate(item) {
    var id = $(item).data("id");
    var options = {};
    options.url = "/Contact/UpdateContact/"
    options.type = "PUT";

    var obj = Contact;
    obj.Id = $(item).data("id");
    obj.PhonePhoneNumber = $(".input-phone", $(item).parent().parent()).val();
    obj.Name = $(".input-name", $(item).parent().parent()).val();
    obj.Surname = $(".input-surname", $(item).parent().parent()).val();
    obj.BirthDate = $(".input-birthdate", $(item).parent().parent()).val();
    obj.Gender = $(".input-notes", $(item).parent().parent()).val();
    obj.Notes = $(".input-notes", $(item).parent().parent()).val();
    obj.KeyWords = $(".input-keywords", $(item).parent().parent()).val();
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
    $(".input-name", $(item).parent().parent())[0].readOnly = true;
    $(".input-surname", $(item).parent().parent())[0].readOnly = true;
    $(".input-birthdate", $(item).parent().parent())[0].readOnly = true;
    $(".input-gender", $(item).parent().parent())[0].readOnly = true;
    $(".input-notes", $(item).parent().parent())[0].readOnly = true;
    $(".input-keywords", $(item).parent().parent())[0].readOnly = true;
    obj.Id = 0;
    item.setAttribute("onclick", "phoneEditAllow(this)");
}

function phoneDelete(item) {
    var id = $(item).data("id");
    var options = {};
    options.url = "/Contact/DeleteContact/"
        + id;
    options.type = "DELETE";
    options.success = function (msg) {
        console.log('msg= ' + msg);
        $("#msg").html(msg);
        if ((phonesCount - 1) % 10 == 0)
            currentPage--;
        GetContactData();
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

function searchContacts() {
    searchValue = $("#searchField").val();
    currentPage = 1;
    GetContactData();
}

function buildNavigationButtons() {
    if (contactCount % pageSize == 0)
        pagesCount = contactCount / pageSize;
    else
        var pagesCount = contactCount / pageSize + 1;
    $("#pageButtons button").remove();
    var button = "<button type='button' class='btn btn -default ' onclick='previousPage()' id='previous'><span class='glyphicon glyphicon-triangle-left' /></button>"
    $("#pageButtons").append(button);
    for (var i = 1; i <= pagesCount; i++) {
        button = "<button type='button' class='btn btn -default' onclick='getPageByNumber(this)' id='Page" + i + "'>" + i + "</button>";
        $("#pageButtons").append(button);
    }
    button = "<button type='button' class='btn btn -default ' onclick='nextPage()' id='next'><span class='glyphicon glyphicon-triangle-right' /></button>";
    $("#pageButtons").append(button);
}