using WebCustomerApp.Models;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Data;
using Microsoft.AspNetCore.Identity;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        private IBaseRepository<Recipient> recipientRepo;
        private IBaseRepository<Company> companyRepo;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IBaseRepository<Company> Companies
        {
            get
            {
                if (companyRepo == null) { companyRepo = new BaseRepository<Company>(context); }
                return companyRepo;
            }
        }

        public IBaseRepository<Recipient> Recipients {
            get {
                if (recipientRepo == null) { recipientRepo = new BaseRepository<Recipient>(context); }
                return recipientRepo;
            }
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        private bool isDisposed = false;

        protected virtual void Grind(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            isDisposed = true;
        }

        public void Dispose()
        {
            Grind(true);
            GC.SuppressFinalize(this);
        }
    }
}
