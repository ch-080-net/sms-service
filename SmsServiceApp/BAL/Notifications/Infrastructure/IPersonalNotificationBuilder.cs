using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace BAL.Notifications.Infrastructure
{
    public interface IPersonalNotificationBuilder
    {
        IPersonalNotificationBuilder SetMessage(string title, string message);
        IPersonalNotificationBuilder SetTime(DateTime time);
        IPersonalNotificationBuilder SetHref(string href);
        IPersonalNotificationBuilder GenerateHref(IUrlHelper urlHelper, string controller, string action, object values = null);
        ICollection<Notification> Build();
    }
}
