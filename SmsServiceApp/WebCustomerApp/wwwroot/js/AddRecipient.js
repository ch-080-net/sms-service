var operatorId;

function getTariffsList() {

    $.ajax({
        url: '/Company/Tariffs/',
        type: 'GET',
        data: {
            id: operatorId
        },
        success: function (tariffs) {
            tariffListSuccess(tariffs);

        },
        error: function (request, message, error) {
            //handleException(request, message, error);
        }
    });
}


function getTariffLimit(tariffId) {

    $.ajax({
        url: '/Company/GetTariffById/',
        type: 'GET',
        data: {
            id: tariffId
        },
        success: function (limit) {
            return limit;

        },
        error: function (request, message, error) {
            //handleException(request, message, error);
        }
    });
}

function tariffListSuccess(tariffs) {

    $("#tariffTable tbody").remove();
    $.each(tariffs, function (index, tariff) {

        tariffAddRow(tariff);
    });
}


function tariffAddRow(tariff) {

    if ($("#tariffTable tbody").length == 0) {
        $("#tariffTable").append("<tbody></tbody>");
    }


    $("#tariffTable tbody").append(
        tariffBuildTableRow(tariff));
}

function tariffBuildTableRow(tariff) {
    var newRow = "<tr>" +
        "<td>" + tariff.name + "</td>" +
        "<td>" + tariff.description + "</td>" +
        "<td>" + tariff.price + "</td>" +
        "<td>" + tariff.limit + "</td>" +
        "<td>" +
        "<input type='radio' name='tariff' " +
        "onclick='ChooseTariff(this);'" +
        "class='btn btn-danger' " +
        "data-id='" + tariff.id + "'>" +
        " " +
        "</input>" +
        "</td>" +
        "</tr>";

    return newRow;
}

function GetTariff(item) {
    operatorId = $(item).val();
    getTariffsList();
}

function ChooseTariff(item) {
    var tariffId = $(item).data("id");
    $("#tariff").val(tariffId);
}

var contactCount = 0;
var pagesCount = 0;
var currentPage = 1;
var pageSize = 5;
var searchValue = "";
var recipients = [];
var ListOfChoosen = [];

$(document).ready(function () {
    $.getScript("https://unpkg.com/gijgo@1.9.11/js/gijgo.min.js");
});


var Recipient = {
    id: 0,
    phoneNumber: "",
    name: "",
    surname: "",
    birthDate: "",
    gender: "",
    priority: "",
    keyWords: ""
};



function nextPage() {
    if (contactCount % pageSize == 0) {
        if (currentPage < contactCount / pageSize) {
            currentPage++;
            contactListSuccess(recipients);
        }
    }
    else
        if (currentPage < parseInt((contactCount / pageSize) + 1)) {
            currentPage++;
            contactListSuccess(recipients);
        }
}

function previousPage() {
    if (currentPage > 1) {
        currentPage--;
        contactListSuccess(recipients);
    }
}

function getPageByNumber(item) {
    var numberOfPage = item.id;
    numberOfPage = numberOfPage.substr(4);
    currentPage = parseInt(numberOfPage, 10);
    contactListSuccess(recipients);
}
function DeleteRecipient(event) {
    var recid = $(event.target).data("recid");
    recipients.splice(recid - 1, 1);
    index--;
}

// Display Phones returned from Web API call
function contactListSuccess(contacts) {
    // Iterate over the collection of data
    $("#contactTable tbody").remove();
    var showRecepients = recipients.slice((currentPage - 1) * pageSize, currentPage * pageSize);
    $.each(showRecepients, function (index, showRecepient) {
        // Add a row to the phone table
        contactAddRow(showRecepient);
    });
    buildNavigationButtons();
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
        "<td>" + contact.phoneNumber + "</td>" +
        "<td>" + contact.name + "</td>" +
        "<td>" + contact.surname + "</td>" +
        "<td>" + contact.birthDate.slice(0, 10) + "</td>" +
        "<td>" + contact.gender + "</td>" +
        "<td>" + contact.priority + "</td>" +
        "<td>" + contact.keyWords + "</td>" +
        "<td>" +
        "<button type='button'" +
        "onclick='contactEditAllow(this);'" +
        "class='btn btn-primary'" +
        "data-id='" + contact.id + "'" +
        "data-phonenumber='" + contact.phoneNumber + "'" +
        "data-name='" + contact.name + "'" +
        "data-surname='" + contact.surname + "'" +
        "data-birthdate='" + contact.birthDate.slice(0, 10) + "'" +
        "data-gender='" + contact.gender + "'" +
        "data-notes='" + contact.priority + "'" +
        "data-keywords='" + contact.keyWords + "'" +
           +
        "</button> " +
        " <button type='button' " +
        "onclick='DeleteRecipient(this);'" +
        "class='btn btn-danger' " +
        "data-id='" + contact.id + "'>" +
        "<span class='glyphicon glyphicon-remove' /> Delete" +
        "</button>" +
        "</td>" +
        "</tr>";

    return newRow;
}

function onAddContact(item) {
    var obj = {};
    Object.assign(obj, Recipient);
    obj.phoneNumber = $("#phoneNumber").val();
    var regex = new RegExp("^[+][0-9]{12}");
    if (!regex.test(obj.phoneNumber)) {
        $("#msg").html("Invalid phone number");
        return;
    }
    obj.name = $("#name").val();
    obj.surname = $("#surname").val();
    obj.birthDate = $("#birthDate").val();
    if (obj.birthDate == "")
        obj.birthDate = new Date(0).toLocaleString();
    if (document.getElementById("genderMale").checked) { obj.gender = "Male"; }
    if (document.getElementById("genderFemale").checked) { obj.gender = "Female"; }
    obj.priority = $("#notes").val();
    obj.keyWords = $("#keywords").val();
    if (obj.name == null)
        obj.name = "";
    if (obj.surname == null)
        obj.surname = "";
    if (obj.priority == null)
        obj.priority = "";
    if (obj.keyWords == null)
        obj.keyWords = "";

    var RecSimilar = false;
    for (var k = 0; k < recipients.length; k++) {
        if (recipients[k].phoneNumber == obj.phoneNumber || recipients[k].length > tariff.limit) {
            RecSimilar = true;
        }
        var tariffId = $("#tariff").val();
        var limit = getTariffLimit(tariffId);
        if (recipients.length >= limit) {
            RecSimilar = true;
            break;
        }

    }
    if (!RecSimilar) {
      recipients.push(obj);
    }

  
    contactCount++;

    contactListSuccess(recipients);
    

    $("#phoneNumber").val("");
    $("#name").val("");
    $("#surname").val("");
    $("#birthDate").val("");
    document.getElementById("genderMale").checked = true;
    document.getElementById("genderFemale").checked = false;
    $("#notes").val("");
    $("#keywords").val("");
}

function contactDelete(item) {
    var id = $(item).data("id");
    recipients.splice(id, 1);
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
    contactListSuccess(recipients);
}

function HideShowFormForAddContact() {
    var x = document.getElementById("AddContactForm");
    if (x.style.display === "none") {
        x.style.display = "block";
    } else {
        x.style.display = "none";
    }
}

function CreateCampaign() {
    var campaign = {};
    var phoneNumber = $("#campaignPhoneNumber").val();
    console.dir(phoneNumber);
    var name = $("#campaignName").val();
    console.dir(name);
    var description = $("#campaignDescription").val();
    console.dir(description);
    var type = parseInt($("input[type='radio']:checked").val());
    console.dir(type);
    var tariffId = $("#tariff").val();
    console.dir(tariffId);
    var startTime = $(".starttime").val();
    console.dir(startTime);
    var endTime = $(".endtime").val();
    console.dir(endTime);
    var recipientsList = recipients;
    console.dir(recipientsList);
    var message = $("#message").val();
    console.dir(message);
    var sendtime= $("#sendtime").val();
    console.dir(sendtime);
    
    campaign.name = $("#campaignName").val();
    campaign.phoneNumber = $("#campaignPhoneNumber").val();
    campaign.description = $("#campaignDescription").val();
    campaign.type = parseInt($("input[type='radio']:checked").val());
    campaign.tariffId = $("#tariff").val();
    campaign.startTime = $(".starttime").val();
    campaign.endTime = $(".endtime").val();
    campaign.message = $("#message").val();
    campaign.sendtime = $("#sendtime").val();
    console.dir(campaign);

    $.ajax({
        url: '/Company/CreateCampaign/',
        type: 'POST',
        data: {
            item: campaign,
            recipient: recipients
        },
        success: function (tariffs) {
            tariffListSuccess(tariffs);
            window.location.href = tariffs.newUrl;
        },
        error: function (request, message, error) {
            //handleException(request, message, error);
        }
        
            
    });

}

function GetFromFile(tariffId) {
    var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.csv|.txt)$/;
    if (regex.test($("#fileUpload").val().toLowerCase())) {
        if (typeof (FileReader) != "undefined") {
            var reader = new FileReader();
            reader.readAsText($("#fileUpload")[0].files[0]);

            reader.onload = function () {
                var recepientrow = reader.result;
                var lines = recepientrow.split("\n");
                var result = [];
                var headers = lines[0].split(",");
                for (var i = 1; i < lines.length; i++) {

                    var obj = {};
                    var currentline = lines[i].split(",");
                    var found = false;
                    for (var j = 0; j < headers.length; j++) {
                        obj[headers[j]] = currentline[j];
                    }
                    for (var k = 0; k < recipients.length; k++) {   
                        if (recipients[k].phoneNumber == obj.phoneNumber) {
                            found = true;
                            break;
                        }
                        var tariffId = $("#tariff").val();
                        var limit = getTariffLimit(tariffId);
                        if (recipients.length >= limit) {
                            found = true;
                            break;
                        }
                    }
                    if (!found) {
                        recipients.push(obj);
                    }
                    
                    result.push(obj);                   
                }
                console.dir(result);
                contactListSuccess(recipients);
            }
        }
        else {
            alert("This browser does not support HTML5.");
        }
    }
    else {
        alert("Please upload a valid CSV file.");
    }
};