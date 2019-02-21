using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WebCustomerApp.Models;

namespace Model.Interfaces
{
    public interface IContactRepository : IBaseRepository<Contact>
    {
        IEnumerable<Contact> GetContactsByPageNumber(int pageNumber, int pageSize,
            Expression<Func<Contact, bool>> filter = null,
            Func<IQueryable<Contact>,
            IOrderedQueryable<Contact>> orderBy = null);
    }
}
