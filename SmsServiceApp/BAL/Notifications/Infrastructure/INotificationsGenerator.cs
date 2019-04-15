using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace BAL.Notifications.Infrastructure
{
    public interface INotificationsGenerator<T>
    {
        T SupplyWithCampaignNotifications(T item);
    }
}
