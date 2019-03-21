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
    public class OperatorRepository : BaseRepository<Operator>, IOperatorRepository
    {
        public OperatorRepository(ApplicationDbContext mainDbContext) : base(mainDbContext)
        {
        }

        public override IEnumerable<Operator> GetAll()
        {
            var result = from r in context.Operators
                         .Include(com => com.Tariffs)                         
                         select r;
            return result;
        }

        public override IEnumerable<Operator> Get(
            Expression<Func<Operator, bool>> filter = null,
            Func<IQueryable<Operator>,
            IOrderedQueryable<Operator>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<Operator> query = context.Operators
                         .Include(com => com.Tariffs);

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

        public override Operator GetById(int id)
        {
            var result = (from r in context.Operators
                         .Include(com => com.Tariffs)
                         where r.Id == id
                         select r).FirstOrDefault();
            return result;
        }
    }
}
