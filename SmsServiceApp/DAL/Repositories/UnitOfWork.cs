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
        private IBaseRepository<Operator> operatorRepo;
        private IBaseRepository<Code> codeRepo;

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

        public IBaseRepository<Code> Codes
        {
            get
            {
                if (codeRepo == null)
                {
                    codeRepo = new BaseRepository<Code>(context);
                }
                return codeRepo;
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
