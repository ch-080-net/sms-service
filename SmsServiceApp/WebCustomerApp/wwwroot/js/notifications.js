"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("GetNotification", function (notification) {
    var storedNotifications = localStorage.getItem("notifications");
    storedNotifications = JSON.parse(storedNotifications);
    if (storedNotifications == null) {
        storedNotifications = [];
    }
    else if (storedNotifications.length > 10) {
        storedNotifications = storedNotifications.slice(storedNotifications.length - 9);
    }

    storedNotifications.push(notification);
    storedNotifications = JSON.stringify(storedNotifications);
    localStorage.setItem("notifications", storedNotifications);
});

connection.start();

function fillModal() {

    var content;
    var storedNotifications = localStorage.getItem("notifications");
    storedNotifications = JSON.parse(storedNotifications);
    if (storedNotifications == null)
        return;
    for (var i = 0; i < storedNotifications.length; i++) {
        content += "<h4>" + storedNotifications[i].Title + "</h4>" + "<small>" + storedNotifications.Time + "</small>";
        content += "<p>" + storedNotifications[i].Message + "</p>";
        content += "<br />";
    }

    document.getElementById("notificationContent").innerHTML = content;
}

