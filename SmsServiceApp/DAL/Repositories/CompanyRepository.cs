using Microsoft.EntityFrameworkCore;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Data;
using WebApp.Models;

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

        /// <summary>
        /// Insert new company to db and return id
        /// </summary>
        /// <param name="item">Company entity</param>
        /// <returns>Id</returns>
        public int InsertWithId(Company item)
        {
            context.Add(item);
            context.SaveChanges();
            return item.Id;
        }
    }
}
