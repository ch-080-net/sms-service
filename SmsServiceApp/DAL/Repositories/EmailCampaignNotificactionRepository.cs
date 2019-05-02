using System;
using System.Collections.Generic;
using Model.Interfaces;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using WebApp.Data;

namespace DAL.Repositories
{
    public class EmailCampaignNotificactionRepository : BaseRepository<EmailCampaignNotification>, IEmailCampaignNotificationRepository
    {
        public EmailCampaignNotificactionRepository(ApplicationDbContext mainDbContext) : base(mainDbContext)
        {
        }

        public override IEnumerable<EmailCampaignNotification> Get(
            Expression<Func<EmailCampaignNotification, bool>> filter = null,
            Func<IQueryable<EmailCampaignNotification>,
            IOrderedQueryable<EmailCampaignNotification>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<EmailCampaignNotification> query = context.emailCampaignNotifications
                         .Include(cn => cn.EmailCampaign)
                         .ThenInclude(cn => cn.User)
                         .ThenInclude(au => au.ApplicationGroup);            

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
    }
}
