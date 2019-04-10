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