"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("GetNotification", function (notification) {
    var storedNotifications = localStorage.getItem("notifications");
    storedNotifications = JSON.parse(storedNotifications);
    if (storedNotifications == null) {
        storedNotifications = [];
    }
    storedNotifications.push(notification);
    storedNotifications = JSON.stringify(storedNotifications);
    localStorage.setItem("notifications", storedNotifications);
});

connection.start();