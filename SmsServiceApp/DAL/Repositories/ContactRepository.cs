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
    /// <summary>
    /// Repository for Contacts
    /// Inherited from BaseManager and have additional method for getting Contacts
    /// </summary>
    public class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(ApplicationDbContext mainDbContext) : base(mainDbContext)
        { }

        /// <summary>
        /// Get Contacts on current sized page with filters
        /// </summary>
        /// <param name="pageNumber">Number of current page</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="filter">Filter for query</param>
        /// <param name="orderBy">Order for query</param>
        /// <returns></returns>
        public IEnumerable<Contact> GetContactsByPageNumber(int pageNumber, int pageSize,
            Expression<Func<Contact, bool>> filter = null,
            Func<IQueryable<Contact>,
            IOrderedQueryable<Contact>> orderBy = null)
        {
            return base.Get(filter: filter, orderBy: orderBy).Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
