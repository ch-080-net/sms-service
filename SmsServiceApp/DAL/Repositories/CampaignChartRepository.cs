using Microsoft.EntityFrameworkCore;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WebApp.Data;
using WebApp.Models;

namespace DAL.Repositories
{
    public class CampaignChartRepository : BaseRepository<Company>, ICampaignChartRepository
    {
        public CampaignChartRepository(ApplicationDbContext mainDbContext) : base(mainDbContext)
        {
        }

        public override IEnumerable<Company> GetAll()
        {
            var result = from r in context.Companies
                         .Include(com => com.Recipients)
                         
                         .Include(com => com.ApplicationGroup)
                         .ThenInclude(ag => ag.ApplicationUsers)
                         select r;
            return result;
        }

        public override IEnumerable<Company> Get(
            Expression<Func<Company, bool>> filter = null,
            Func<IQueryable<Company>,
            IOrderedQueryable<Company>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<Company> query = context.Companies
                         .Include(com => com.Recipients)
                         .Include(com => com.ApplicationGroup)
                         .ThenInclude(ag => ag.ApplicationUsers);

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
