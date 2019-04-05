"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();
window.onload = notificationStartup();

connection.on("GetNotification", function (notification) {
    incrementBadge();
    connection.invoke("ConfirmReceival", notification.id, notification.origin).catch(function (err) {
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
    activeNotifications = JSON.stringify(++activeNotifications);
    localStorage.setItem("numOfActiveNotifications", activeNotifications);
    loadBadge();
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

function fillModal(numOfNotifications = 15) {
    connection.invoke("GetNotificationPage", numOfNotifications).catch(function (err) {
        return console.error(err.toString());
    }).then(function (value) {
        var content = "";
        for (var i = 0; i < value.length; i++) {
            content += "<h4>" + value[i].title + "</h4>" + "<small>" + value[i].time + "</small>";
            content += "<p>" + value[i].message + "</p>";
            content += "<br />";
        }
        document.getElementById("notificationContent").innerHTML = content;
    });
}

function notificationStartup() {
    loadBadge();
}


