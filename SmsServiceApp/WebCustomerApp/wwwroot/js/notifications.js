﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();
window.onload = notificationStartup();
var notificationsToShow = 10;

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

function fillModal() {
    connection.invoke("GetNotificationPage", notificationsToShow).catch(function (err) {
        return console.error(err.toString());
    }).then(function (value) {
        var content = "";
        for (var i = 0; i < value.length; i++) {
            content += "<h4>" + value[i].title + "</h4>" + "<small>" + value[i].time + "</small>";
            content += "<p>" + value[i].message + "</p>";
            content += "<br />";
        }
        content += '<button type="button" class="btn btn-primary btn-lg btn-block" onclick="showMoreNotifications()">Load more</button>'
        document.getElementById("notificationContent").innerHTML = content;
    });
}

function fillReport() {
    connection.invoke("GetNotificationReport").catch(function (err) {
        return console.error(err.toString());
    }).then(function (value) {
        var content = "";
        if (value.votingsInProgress == 0) {
            content += "<ul class='menu'><li style='width:99%'><a href='#'>" + "<span class='glyphicon glyphicon-pause' aria-hidden='true'></span>" + "No votings in progress" + "</a></li></ul>"
        }
        else {
            content += "<ul class='menu'><li style='width:99%'><a href='#'>" + "<span class='glyphicon glyphicon-play' aria-hidden='true'></span>" + value.votingsInProgress + " votings in progress" + "</a></li></ul>"
        }

        if (value.campaignsPlannedToday == 0) {
            content += "<ul class='menu'><li style='width:99%'><a href='#'>" + "<span class='glyphicon glyphicon-calendar' aria-hidden='true'></span>" + "No mailings planned for today" + "</a></li></ul>"
        }
        else {
            content += "<ul class='menu'><li style='width:99%'><a href='#'>" + "<span class='glyphicon glyphicon-calendar' aria-hidden='true'></span>" + value.mailingsPlannedToday + " mailings planned for today" + "</a></li></ul>"
        }

        content += "<br />"
        
        for (var i = 0; i < value.notifications.length; i++) {
            content += "<ul class='menu'><li style='width:99%'>";
            if (value.notifications[i].href != null)
                content += "<a href='" + value.notifications[i].href + "'>";
            else {
                content += "<a href='#'>";
            }
            content += value.notifications[i].message + "</a></li></ul>";
        }
        document.getElementById("notificationMenu").innerHTML = content;
    });
}

function showMoreNotifications()
{
    notificationsToShow += 10;
    fillModal();
}

function notificationStartup() {
    loadBadge();
}


