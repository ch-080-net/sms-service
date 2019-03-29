var operatorId;

function getTariffsList() {
    // Call Web API to get a list of Phones
    $.ajax({
        url: '/Step/Tariffs/',
        type: 'GET',
        data: {
            id: operatorId
        },
        success: function (tariffs) {
            tariffListSuccess(tariffs);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function tariffListSuccess(tariffs) {
    // Iterate over the collection of data
    $("#tariffTable tbody").remove();
    $.each(tariffs, function (index, tariff) {
        // Add a row to the phone table
        tariffAddRow(tariff);
    });
}

// Add phone row to <table>
function tariffAddRow(tariff) {
    // First check if a <tbody> tag exists, add one if not
    if ($("#tariffTable tbody").length == 0) {
        $("#tariffTable").append("<tbody></tbody>");
    }

    // Append row to <table>
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
        " <button type='button' " +
        "onclick='ChooseTariff(this);'" +
        "class='btn btn-danger' " +
        "data-id='" + tariff.id + "'>" +
        "Choose" +
        "</button>" +
        "</td>" +
        "</tr>";

    return newRow;
}

function GetTariff(item) {
    operatorId = $(item).val();
    getTariffsList();
}