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
    public class CampaignNotificationRepository : BaseRepository<CampaignNotification>, ICampaignNotificationRepository
    {
        public CampaignNotificationRepository(ApplicationDbContext mainDbContext) : base(mainDbContext)
        {

        }

        public override IEnumerable<CampaignNotification> Get(
            Expression<Func<CampaignNotification, bool>> filter = null,
            Func<IQueryable<CampaignNotification>,
            IOrderedQueryable<CampaignNotification>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<CampaignNotification> query = context.CampaignNotifications
                         .Include(cn => cn.Campaign)
                         .Include(cn => cn.ApplicationUser)
                         .ThenInclude(au => au.ApplicationGroup)
                         .ThenInclude(ag => ag.Phone);

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
