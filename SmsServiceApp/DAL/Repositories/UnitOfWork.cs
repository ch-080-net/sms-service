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
        private UserManager<ApplicationUser> userManager;
        private IContactRepository contactRepo;
        private IBaseRepository<Phone> phoneRepo;
        private IBaseRepository<Tariff> tariffRepo;

        private IBaseRepository<Operator> operatorRepo;

        public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
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
        public IBaseRepository<Tariff> Tariffs
        {
            get
            {
                if (tariffRepo == null) { tariffRepo = new BaseRepository<Tariff>(context); }
                return tariffRepo;
            }
        }
        public UserManager<ApplicationUser> Users
        {
            get
            {
                return userManager;
            }
        }

        public IBaseRepository<Operator> Operators
        {
            get
            {
                if (operatorRepo == null)
                {
                    operatorRepo = new BaseRepository<Operator>(context);
                }
                return operatorRepo;
            }
        }

        public IContactRepository Contacts {
            get {
                if (contactRepo == null) { contactRepo = new ContactRepository(context); }
                return contactRepo;
            }
        }

        public IBaseRepository<Phone> Phones {
            get {
                if (phoneRepo == null) { phoneRepo = new BaseRepository<Phone>(context); }
                return phoneRepo;
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
