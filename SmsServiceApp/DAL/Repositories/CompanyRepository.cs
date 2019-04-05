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
    /// <summary>
    /// Company storage in db
    /// </summary>
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext mainDbContext) : base(mainDbContext)
        {
        }

        public override IEnumerable<Company> Get(
            Expression<Func<Company, bool>> filter = null,
            Func<IQueryable<Company>,
            IOrderedQueryable<Company>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<Company> query = context.Companies
                         .Include(com => com.ApplicationGroup)
                         .ThenInclude(com => com.ApplicationUsers);

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

        /// <summary>
        /// Insert new company to db and return id
        /// </summary>
        /// <param name="item">Company entity</param>
        /// <returns>Id</returns>
        public int InsertWithId(Company item)
        {
            context.Companies.Add(item);
            context.SaveChanges();
            return item.Id;
        }

        public int InsertRecieveCampaign(Company item)
        {
            context.Companies.Add(item);
            context.SaveChanges();
            return item.Id;
        }
    }
}
