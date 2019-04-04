using System;
using System.Collections.Generic;
using System.Text;
using Model.Interfaces;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using WebApp.Data;

namespace DAL.Repositories
{
    class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext mainDbContext) : base(mainDbContext)
        {

        }

        public override IEnumerable<Notification> Get(
            Expression<Func<Notification, bool>> filter = null,
            Func<IQueryable<Notification>,
            IOrderedQueryable<Notification>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<Notification> query = context.Notifications
                         .Include(n => n.ApplicationUser)
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
