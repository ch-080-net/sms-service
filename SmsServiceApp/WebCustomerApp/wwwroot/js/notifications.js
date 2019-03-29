"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();
window.onload = notificationStartup();

connection.on("GetNotification", function (notification) {
    var storedNotifications = localStorage.getItem("notifications");
    storedNotifications = JSON.parse(storedNotifications);
    if (storedNotifications == null) {
        storedNotifications = [];
        incrementBadge();
    }
    else if (storedNotifications.length > 15) {
        storedNotifications = storedNotifications.slice(storedNotifications.length - 14);
    }
    else {
        incrementBadge();
    }

    storedNotifications.unshift(notification);
    storedNotifications = JSON.stringify(storedNotifications);
    localStorage.setItem("notifications", storedNotifications);
    fillModal();
    connection.invoke("ConfirmReceival", notification.id).catch(function (err) {
        return console.error(err.toString());
    });
});

connection.start();

function incrementBadge() {
    var activeNotifications = localStorage.getItem("numOfActiveNotifications");
    activeNotifications = JSON.parse(activeNotifications);
    if (activeNotifications == null) {
        activeNotifications = 0;
    }

    document.getElementById("notificationBadge").innerHTML = ++activeNotifications;
    activeNotifications = JSON.stringify(activeNotifications);
    localStorage.setItem("numOfActiveNotifications", activeNotifications);
}

function loadBadge() {
    var activeNotifications = localStorage.getItem("numOfActiveNotifications");
    activeNotifications = JSON.parse(activeNotifications);
    if (activeNotifications == null) {
        activeNotifications = "";
    }
    document.getElementById("notificationBadge").innerHTML = activeNotifications;
}

function clearBadge() {
    localStorage.setItem("numOfActiveNotifications", null);
    loadBadge();
}

function fillModal() {
    var content = "";
    var storedNotifications = localStorage.getItem("notifications");
    storedNotifications = JSON.parse(storedNotifications);
    if (storedNotifications == null)
        return;
    for (var i = 0; i < storedNotifications.length; i++) {
        content += "<h4>" + storedNotifications[i].title + "</h4>" + "<small>" + storedNotifications[i].time + "</small>";
        content += "<p>" + storedNotifications[i].message + "</p>";
        content += "<br />";
    }
    document.getElementById("notificationContent").innerHTML = content;
}

function notificationStartup() {
    loadBadge();
    fillModal();
}


