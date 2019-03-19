using System;
using System.Collections.Generic;
using System.Text;
using Model.Interfaces;
using WebCustomerApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using WebCustomerApp.Data;

namespace DAL.Repositories
{
    /// <summary>
    /// Derived from BaseRepository<Recipient>, Get() and GetAll() overrided to return entities with included 
    /// Company, ApplicationUser, Phone
    /// </summary>
    public class MailingRepository : BaseRepository<Recipient>, IMailingRepository
    {
        public MailingRepository(ApplicationDbContext mainDbContext) : base(mainDbContext)
        {
        }

        public override IEnumerable<Recipient> GetAll()
        {
            var result = from r in context.Recipients
                         .Include(r => r.Company)
                         .ThenInclude(com => com.ApplicationGroup)
                         .ThenInclude(ag => ag.phoneGroupUnsubscribtions)
                         .Include(r => r.Company)
                         .ThenInclude(ag => ag.Phone)
                         .Include(r => r.Phone)
                         select r;
            return result;
        }


        public override IEnumerable<Recipient> Get(
            Expression<Func<Recipient, bool>> filter = null,
            Func<IQueryable<Recipient>,
            IOrderedQueryable<Recipient>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<Recipient> query = context.Recipients
                         .Include(r => r.Company)
                         .ThenInclude(com => com.ApplicationGroup)
                         .ThenInclude(ag => ag.phoneGroupUnsubscribtions)
                         .Include(r => r.Company)
                         .ThenInclude(ag => ag.Phone)
                         .Include(r => r.Phone);

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
