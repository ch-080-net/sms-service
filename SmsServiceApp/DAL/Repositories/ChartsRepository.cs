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
    public class ChartsRepository : BaseRepository<Company>, IChartsRepository
    {
        public ChartsRepository(ApplicationDbContext mainDbContext) : base(mainDbContext)
        {
        }

        public override IEnumerable<Company> GetAll()
        {
            var result = from r in context.Companies
                         .Include(com => com.RecievedMessages)
                         .Include(com => com.Recipients)
                         .Include(com => com.AnswersCodes)
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
                         .Include(com => com.RecievedMessages)
                         .Include(com => com.Recipients)

                         .Include(com => com.AnswersCodes)
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
