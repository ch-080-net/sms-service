using Microsoft.EntityFrameworkCore;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Data;
using WebApp.Models;

namespace DAL.Repositories
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext mainDbContext) : base(mainDbContext)
        {
        }

        public int InsertWithId(Company item)
        {
            context.Add(item);
            context.SaveChanges();
            return item.Id;
        }
    }
}
