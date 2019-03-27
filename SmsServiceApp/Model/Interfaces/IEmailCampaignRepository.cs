using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Models;

namespace Model.Interfaces
{
    public interface IEmailCampaignRepository : IBaseRepository<EmailCampaign>
    {
        int InsertWithId(EmailCampaign item);
    }
}
