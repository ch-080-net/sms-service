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
  
   public class AdminStatisticRepository : BaseRepository<ApplicationGroup>, IAdminStatisticRepository
    {
       public AdminStatisticRepository(ApplicationDbContext mainDbContext) : base(mainDbContext)
       {
       }

       public override IEnumerable<ApplicationGroup> GetAll()
       {
           var result = from r in context.Groups
                   .Include(com => com.Companies)
                   .ThenInclude(com => com.Recipients)
                        select r;
           return result;
       }

       public override IEnumerable<ApplicationGroup> Get(
           Expression<Func<ApplicationGroup, bool>> filter = null,
           Func<IQueryable<ApplicationGroup>,
               IOrderedQueryable<ApplicationGroup>> orderBy = null,
           string includeProperties = "")
       {
           IQueryable<ApplicationGroup> query = context.Groups
               .Include(com => com.Companies)
               .ThenInclude(com => com.Recipients);
               

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
