using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Model.Interfaces;
using WebCustomerApp.Data;
using WebCustomerApp.Models;

namespace DAL.Repositories
{
    public class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(ApplicationDbContext mainDbContext) : base(mainDbContext)
        { }

        public IEnumerable<Contact> GetContactsByPageNumber(int pageNumber, int pageSize,
            Expression<Func<Contact, bool>> filter = null,
            Func<IQueryable<Contact>,
            IOrderedQueryable<Contact>> orderBy = null)
        {
            return base.Get(filter: filter, orderBy: orderBy).Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
