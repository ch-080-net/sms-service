using WebCustomerApp.Models;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Data;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        private IBaseRepository<Recipient> recipientRepo;
        private IContactRepository contactRepo;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IBaseRepository<Recipient> Recipients {
            get {
                if (recipientRepo == null) { recipientRepo = new BaseRepository<Recipient>(context); }
                return recipientRepo;
            }
        }

        public IContactRepository Contacts {
            get {
                if (contactRepo == null) { contactRepo = new ContactRepository(context); }
                return contactRepo;
            }
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        private bool isDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                context.Dispose();
            }
            isDisposed = true;
        }
    }
}
