

var contactCount;
var pagesCount;
var currentPage = 1;
var pageSize = 5;
var searchValue = "";

$(document).ready(function () {
    $.getScript("https://unpkg.com/gijgo@1.9.11/js/gijgo.min.js");
    GetContactData()
    });

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
        if (currentPage < parseInt((contactCount / pageSize) + 1)) {
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
        "<td>" + contact.phonePhoneNumber + "</td>" +
        "<td>" + contact.name + "</td>" +
        "<td>" + contact.surname + "</td>" +
        "<td>" + contact.birthDate.slice(0, 10) + "</td>" +
        "<td>" + contact.gender + "</td>" +
        "<td>" + contact.notes + "</td>" +
        "<td>" + contact.keyWords + "</td>" +
        "<td>" +
        "<button type='button'" +
        "onclick='contactEditAllow(this);'" +
        "class='btn btn-primary'" +
        "data-id='" + contact.id + "'" +
        "data-phonenumber='" + contact.phonePhoneNumber + "'" +
        "data-name='" + contact.name + "'" +
        "data-surname='" + contact.surname + "'" +
        "data-birthdate='" + contact.birthDate.slice(0, 10) + "'" +
        "data-gender='" + contact.gender + "'" +
        "data-notes='" + contact.notes + "'" +
        "data-keywords='" + contact.keyWords + "'" +
        ">" +
        "<span class='glyphicon glyphicon-edit' /> Update" +
        "</button> " +
        " <button type='button' " +
        "onclick='contactDelete(this);'" +
        "class='btn btn-danger' " +
        "data-id='" + contact.id + "'>" +
        "<span class='glyphicon glyphicon-remove' /> Delete" +
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
    var regex = new RegExp("^[+][0-9]{12}");
    if (!regex.test(obj.PhonePhoneNumber)) {
        $("#msg").html("Invalid phone number");
        return;
    }
    obj.Name = $("#name").val();
    obj.Surname = $("#surname").val();
    obj.BirthDate = $("#birthDate").val();
    if (obj.BirthDate == "")
        obj.BirthDate = new Date(0).toLocaleString();
    if (document.getElementById("genderMale").checked) { obj.Gender = "Male"; }
    if (document.getElementById("genderFemale").checked) { obj.Gender = "Female"; }
    obj.Notes = $("#notes").val();
    obj.KeyWords = $("#keywords").val();
    if (obj.Name == null)
        obj.Name = "";
    if (obj.Surname == null)
        obj.Surname = "";
    if (obj.Notes == null)
        obj.Notes = "";
    if (obj.KeyWords == null)
        obj.KeyWords = "";
    console.dir(obj);
    options.data = obj;

    options.success = function (msg) {
        $("#msg").html(msg);
        GetContactData();
    };
    options.error = function () {
            $("#msg").html("Error while calling the Web API!");
    };
    $.ajax(options);
    $("#phoneNumber").val("");
    $("#name").val("");
    $("#surname").val("");
    $("#birthDate").val("");
    document.getElementById("genderMale").checked = true;
    document.getElementById("genderFemale").checked = false;
    $("#notes").val("");
    $("#keywords").val("");
}

function contactEditAllow(item) {
    document.getElementById("AddContactForm").style.display = "block";

    $("#HideShowAddContact").val("Edit contact");
    $("#phoneNumber").val($(item).data("phonenumber"));
    $("#name").val($(item).data("name"));
    $("#surname").val($(item).data("surname"));
    $("#birthDate").val($(item).data("birthdate"));
    var text = $(item).data("gender");
    if (text == "Male")
        document.getElementById("genderMale").checked = true;
    if (text == "Female")
        document.getElementById("genderFemale").checked = true;
    $("#notes").val($(item).data("notes"));
    $("#keywords").val($(item).data("keywords"));
    document.getElementById("insert").textContent = "Save";
    document.getElementById("insert").setAttribute("onclick", "contactUpdate(" + $(item).data("id") + ")");

}

function contactUpdate(idOfUpdatePhone) {
    var id = idOfUpdatePhone;
    var options = {};
    options.url = "/Contact/UpdateContact/"
    options.type = "PUT";

    var obj = Contact;
    obj.Id = id;
    obj.PhonePhoneNumber = $("#phoneNumber").val();
    var regex = new RegExp("^[+][0-9]{12}");
    if (!regex.test(obj.PhonePhoneNumber)) {
        $("#msg").html("Invalid phone number");
        return;
    }
    obj.Name = $("#name").val();
    obj.Surname = $("#surname").val();
    obj.BirthDate = $("#birthDate").val();
    if (obj.BirthDate == "")
        obj.BirthDate = new Date(0).toLocaleString();
    if (document.getElementById("genderMale").checked) { obj.Gender = "Male"; }
    if (document.getElementById("genderFemale").checked) { obj.Gender = "Female"; }
    obj.Notes = $("#notes").val();
    obj.KeyWords = $("#keywords").val();
    console.dir(obj);
    options.data = obj;
    options.success = function (msg) {
        $("#msg").html(msg);
        obj.Id = 0;
        getContactList();
    };
    options.error = function () {
        $("#msg").html("Error while calling the Web API!");
    };
    $.ajax(options);
    document.getElementById("insert").setAttribute("onclick", "onAddContact(this)");
    document.getElementById("AddContactForm").style.display = "none";
    $("#phoneNumber").val("");
    $("#name").val("");
    $("#surname").val("");
    $("#birthDate").val("");
    document.getElementById("genderMale").checked = false;
    document.getElementById("genderFemale").checked = false;
    $("#notes").val("");
    $("#keywords").val("");
    $("#HideShowAddContact").val("Add new contact");
    document.getElementById("insert").textContent = "ADD";

}

function contactDelete(item) {
    var id = $(item).data("id");
    var options = {};
    options.url = "/Contact/DeleteContact/"
        + id;
    options.type = "DELETE";
    options.success = function (msg) {
        console.log('msg= ' + msg);
        $("#msg").html(msg);
        if ((contactCount - 1) % 10 == 0)
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
    if (error != null) {
        msg += "Message" + error.Message + "\n";
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
        pagesCount = contactCount / pageSize + 1;
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

function pageSizeChange(item) {
    pageSize = parseInt(item[item.selectedIndex].text, 10);
    buildNavigationButtons();
    if (pagesCount < currentPage)
        currentPage = pagesCount;
    getContactList();
}

function HideShowFormForAddContact() {
    var x = document.getElementById("AddContactForm");
    if (x.style.display === "none") {
        x.style.display = "block";
    } else {
        x.style.display = "none";
    }
}

function readURL(input) {
    if (input.files && input.files[0]) {

        reader = new FileReader();

        reader.onload = function (e) {
            $('.image-upload-wrap').hide();

            $('.file-upload-image').attr('src', e.target.result);
            $('.file-upload-content').show();

            $('.image-title').html(input.files[0].name);
        };

        reader.readAsDataURL(input.files[0]);

    } else {
        removeUpload();
    }
}

function removeUpload() {
    $('.file-upload-input').replaceWith($('.file-upload-input').clone());
    $('.file-upload-content').hide();
    $('.image-upload-wrap').show();
}
$('.image-upload-wrap').bind('dragover', function () {
    $('.image-upload-wrap').addClass('image-dropping');
});
$('.image-upload-wrap').bind('dragleave', function () {
    $('.image-upload-wrap').removeClass('image-dropping');
});